using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewEndOfLevel : MonoBehaviour
{
    public TextMeshProUGUI text_message;
    public TextMeshProUGUI text_reward;
    public Transform gemsCollectedParent;


    private void HideGems()
    {
        int gemsCount = gemsCollectedParent.childCount;
        for (int i = 0; i < gemsCount; i++)
        {
            Transform gem = gemsCollectedParent.GetChild(i);
            gem.gameObject.SetActive(false);
        }
    }

    public void Show(string message, int reward, int gemsCollected)
    {
        HideGems();

        text_message.text = message;
        text_reward.text = $"Cr: {reward}";

        for (int i = 0; i < gemsCollected; i++)
        {
            Transform gem = gemsCollectedParent.GetChild(i);
            gem.gameObject.SetActive(i < gemsCollected);
            float xPos = (i - (gemsCollected - 1) / 2f) * 75f;
            gem.localPosition = new Vector3(xPos, gem.localPosition.y, gem.localPosition.z);
        }
    }
}
