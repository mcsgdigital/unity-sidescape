using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private bool levelEnded;
    private FadeUI fadeUI;
    private LevelCompleteUI levelCompleteUI;

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

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("GAME COMPLETE");
            yield break;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    private IEnumerator RestartLevel()
    {
        yield return StartCoroutine(fadeUI.FadeOut());

        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartCurrentLevel()
    {
        if (levelEnded) return;

        levelEnded = true;

        StartCoroutine(RestartLevel());
    }
}