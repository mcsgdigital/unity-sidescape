using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentLevelTotalGems = 0;
    public int currentLevelTotalGemsCollected = 0;
    public int currentLevelTotalStepsTaken = 0;
    public int currentLevelTotalTiles = 0;

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
        uiFeedback.UpdateGemsText(currentLevelTotalGemsCollected);
    }

    private IEnumerator LoadNextLevel()
    {
        levelCompleteUI.HideEndOfLevel();
        ClearVariables();

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

        ClearVariables();

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
        currentLevelTotalGemsCollected++;
    }

    public void TakeStep()
    {
        currentLevelTotalStepsTaken++;
    }

    public void SetCurrentLevelTotalTiles(int totalTiles)
    {
        currentLevelTotalTiles = totalTiles;
    }

    public void ClearVariables()
    {
        currentLevelTotalGemsCollected = 0;
        currentLevelTotalStepsTaken = 0;
        currentLevelTotalTiles = 0;
    }

    public void SetCurrentLevelTotalGems()
    {
        currentLevelTotalGems = GameObject.FindGameObjectsWithTag("Gem").Length;
    }

    public void HandleLevelSelect(int levelIndex)
    {
        if (levelEnded) return;

        levelEnded = true;

        StartCoroutine(LoadSelectedLevel(levelIndex));
    }

    private IEnumerator LoadSelectedLevel(int levelIndex)
    {
        yield return StartCoroutine(fadeUI.FadeOut());

        ClearVariables();

        yield return new WaitForSeconds(0.2f);

        SceneManager.UnloadSceneAsync(currentLevelIndex);

        currentLevelIndex = levelIndex;
        uiFeedback.UpdateLevelText(currentLevelIndex);

        SceneManager.LoadScene(currentLevelIndex, LoadSceneMode.Additive);

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(fadeUI.FadeIn());

        levelEnded = false;
    }
}