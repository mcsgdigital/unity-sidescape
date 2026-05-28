using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject LevelSelectWindow;

    private bool isOpen_levelMapWindow = false;


    private void Awake()
    {
        CloseLevelSelect();
    }

    public void OpenLevelSelect()
    {
        Time.timeScale = 0f;
        LevelSelectWindow.gameObject.SetActive(true);
        isOpen_levelMapWindow = true;
    }

    public void CloseLevelSelect()
    {
        Time.timeScale = 1f;
        LevelSelectWindow.gameObject.SetActive(false);
        isOpen_levelMapWindow = false;
    }

    public bool IsOpen()
    {
        return isOpen_levelMapWindow;
    }
}
