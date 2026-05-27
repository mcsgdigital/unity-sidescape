using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIfeedback : MonoBehaviour
{
    public TextMeshProUGUI text_LEVEL;
    public TextMeshProUGUI text_GEMS;

    private void Start()
    {
        UpdateLevelText(1);
        UpdateGemsText(0);
    }

    public void UpdateLevelText(int level)
    {
        text_LEVEL.text = $"LV {level}";
    }

    public void UpdateGemsText(int gems)
    {
        text_GEMS.text = $"{gems}";
    }
}
