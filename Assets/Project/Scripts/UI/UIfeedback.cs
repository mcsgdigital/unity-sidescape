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
        UpdateLevelText(1);

        ResetVariables();
    }

    public void UpdateLevelText(int level)
    {
        text_LEVEL.text = $"LV {level}";
    }

    public void UpdateGemsText(int gems)
    {
        text_GEMS.text = $"{gems}";
    }

    public void UpdateChargesText(int charges)
    {
        text_CHARGES.text = $"{charges}";
    }

    public void ResetVariables()
    {
        UpdateGemsText(0);
        UpdateChargesText(0);
    }
}
