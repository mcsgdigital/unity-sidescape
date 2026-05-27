using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private bool levelEnded;
    private FadeUI fadeUI;
    private LevelCompleteUI levelCompleteUI;
    private int currentLevelIndex = 1;

    private void Awake()
    {
        levelEnded = false;
        fadeUI = FindObjectOfType<FadeUI>();
        levelCompleteUI = FindObjectOfType<LevelCompleteUI>();
    }

    public void WinLevel()
    {
        if (levelEnded) return;

        levelEnded = true;
        levelCompleteUI.Show();

        StartCoroutine(LoadNextLevel());
    }

    public void LoseLevel()
    {
        if (levelEnded) return;

        levelEnded = true;

        Debug.Log("LEVEL FAILED");

        StartCoroutine(RestartLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        yield return StartCoroutine(fadeUI.FadeOut());

        yield return new WaitForSeconds(0.2f);

        SceneManager.UnloadSceneAsync(currentLevelIndex);

        currentLevelIndex++;

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
}