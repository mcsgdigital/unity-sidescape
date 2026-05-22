using UnityEngine;

public class LevelCompleteUI : MonoBehaviour
{
    public GameObject panel;

    public void Show()
    {
        panel.SetActive(true);
    }
}
