using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Transform LevelSelectWindow;

    private void Awake()
    {
        CloseLevelSelect();
    }

    public void OpenLevelSelect()
    {
        Time.timeScale = 0f;
        LevelSelectWindow.gameObject.SetActive(true);
    }

    public void CloseLevelSelect()
    {
        Time.timeScale = 1f;
        LevelSelectWindow.gameObject.SetActive(false);
    }
}
