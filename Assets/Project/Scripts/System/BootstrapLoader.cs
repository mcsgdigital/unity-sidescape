using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    private void Start()
    {
        LoadLevel(1);
    }

    public void LoadLevel(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
    }
}