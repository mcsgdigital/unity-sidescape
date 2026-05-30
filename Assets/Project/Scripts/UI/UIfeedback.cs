using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIfeedback : MonoBehaviour
{
    public TextMeshProUGUI text_LEVEL;
    public TextMeshProUGUI text_GEMS;
    public TextMeshProUGUI text_CHARGES;

    private void Start()
    {
        UpdateLevelText();

        UpdateTexts();
    }

    public void UpdateLevelText()
    {
        text_LEVEL.text = $"LV {UserData.Instance.currentLevel}";
    }

    public void UpdateGemsText()
    {
        text_GEMS.text = $"{UserData.Instance.gemsCollected}";
    }

    public void UpdateChargesText()
    {
        text_CHARGES.text = $"{UserData.Instance.chargeCollected}";
    }

    public void UpdateTexts()
    {
        UpdateGemsText();
        UpdateChargesText();
    }
}
