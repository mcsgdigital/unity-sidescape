using UnityEngine;

public class LevelCompleteUI : MonoBehaviour
{
    public GameObject panelCanvas;
    public GameObject panelEndOfLevel;

    private string[] messages = new string[]
    {
        "GOOD!",
        "WELL DONE!",
        "AMAZING!",
        "AWESOME!!",
        "PERFECT!!!",
    };

    public void Show()
    {
        panelCanvas.SetActive(true);
    }

    public void ShowEndOfLevel()
    {
        ViewEndOfLevel viewEndOfLevel = panelEndOfLevel.GetComponent<ViewEndOfLevel>();
        int gemsCollected = LevelManager.Instance.totalGemsCollected;
        viewEndOfLevel.Show(messages[3], 25664, gemsCollected);

        panelEndOfLevel.SetActive(true);
    }

    public void HideEndOfLevel()
    {
        panelEndOfLevel.SetActive(false);
    }
}
