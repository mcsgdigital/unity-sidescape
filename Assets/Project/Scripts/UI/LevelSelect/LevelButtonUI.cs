using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonUI : MonoBehaviour
{
    [Header("Unlocked")]
    public GameObject unlockedGroup;

    public TMP_Text levelText;
    public TMP_Text gemsText;
    public TMP_Text gradeText;

    public Button playButton;

    [Header("Locked")]
    public GameObject lockedGroup;

    public TMP_Text unlockRequirementText;

    public Image lockIcon;

    public void Setup(LevelData data)
    {
        bool unlocked = data.unlocked;

        unlockedGroup.SetActive(unlocked);
        lockedGroup.SetActive(!unlocked);

        if (unlocked)
        {
            levelText.text =
                "LEVEL " + data.levelIndex;

            gemsText.text =
                data.gemsCollected +
                "/" +
                data.totalGems +
                " GEMS";

            gradeText.text =
                GetGradeText(data.bestGrade);

            playButton.onClick.RemoveAllListeners();

            playButton.onClick.AddListener(() =>
            {
                CanvasManager canvasManager = FindObjectOfType<CanvasManager>();
                canvasManager.CloseLevelSelect();
                LevelManager.Instance.HandleLevelSelect(data.levelIndex);
            });
        }
        else
        {
            unlockRequirementText.text =
                data.unlockRequirement;
        }
    }

    private string GetGradeText(int grade)
    {
        switch (grade)
        {
            case 0: return "GOOD";
            case 1: return "WELL DONE";
            case 2: return "AMAZING";
            case 3: return "AWESOME";
            case 4: return "PERFECT";
        }

        return "";
    }
}