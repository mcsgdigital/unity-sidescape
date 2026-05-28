using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int totalGemsCollected = 0;

    public static LevelManager Instance { get; private set; }

    private bool levelEnded;
    private FadeUI fadeUI;
    private LevelCompleteUI levelCompleteUI;
    private UIfeedback uiFeedback;
    private int currentLevelIndex = 1;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        levelEnded = false;
        fadeUI = FindObjectOfType<FadeUI>();
        levelCompleteUI = FindObjectOfType<LevelCompleteUI>();
        levelCompleteUI.HideEndOfLevel();
        uiFeedback = FindObjectOfType<UIfeedback>();
    }

    public void WinLevel()
    {
        if (levelEnded) return;

        levelEnded = true;
        levelCompleteUI.Show();

        StartCoroutine(ShowEndOfLevelResult());
    }

    public void LoseLevel()
    {
        if (levelEnded) return;

        levelEnded = true;

        Debug.Log("LEVEL FAILED");

        StartCoroutine(RestartLevel());
    }

    public void HandleEndOfLevelButtonOK()
    {
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator ShowEndOfLevelResult()
    {
        yield return StartCoroutine(fadeUI.FadeOutToAlpha(0.5f));

        yield return new WaitForSeconds(0.2f);

        levelCompleteUI.ShowEndOfLevel();
        // TODO: Later add some animation to show the end of level result. Example: gems collected to fly to the gems counter in the UI
        uiFeedback.UpdateGemsText(totalGemsCollected);
    }

    private IEnumerator LoadNextLevel()
    {
        levelCompleteUI.HideEndOfLevel();

        yield return StartCoroutine(fadeUI.FadeOut());

        yield return new WaitForSeconds(0.2f);

        SceneManager.UnloadSceneAsync(currentLevelIndex);

        currentLevelIndex++;
        uiFeedback.UpdateLevelText(currentLevelIndex);

        if (currentLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("GAME COMPLETE");
            yield break;
        }

        SceneManager.LoadScene(currentLevelIndex, LoadSceneMode.Additive);

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(fadeUI.FadeIn());

        levelEnded = false;
    }

    private IEnumerator RestartLevel()
    {
        yield return StartCoroutine(fadeUI.FadeOut());

        yield return new WaitForSeconds(0.2f);

        SceneManager.UnloadSceneAsync(currentLevelIndex);

        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(currentLevelIndex, LoadSceneMode.Additive);

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(fadeUI.FadeIn());

        levelEnded = false;
        Camera.main.GetComponent<CameraFollow>().StartFollowing();
    }

    public void RestartCurrentLevel()
    {
        if (levelEnded) return;

        levelEnded = true;

        StartCoroutine(RestartLevel());
    }

    public void CollectGem()
    {
        totalGemsCollected++;
    }
}