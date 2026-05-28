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

    private LevelManager levelManager;


    private void Start()
    {
        levelManager = LevelManager.Instance;
    }

    public void Show()
    {
        panelCanvas.SetActive(true);
    }

    public void ShowEndOfLevel()
    {
        ViewEndOfLevel viewEndOfLevel = panelEndOfLevel.GetComponent<ViewEndOfLevel>();
        int gemsCollected = levelManager.currentLevelTotalGemsCollected;
        var (finalReward, finalGrade) = CalculateReward();

        viewEndOfLevel.Show(messages[finalGrade], finalReward, gemsCollected);

        panelEndOfLevel.SetActive(true);
    }

    public void HideEndOfLevel()
    {
        panelEndOfLevel.SetActive(false);
    }

    private (int finalReward, int grade) CalculateReward()
    {
        int gemsCollected = levelManager.currentLevelTotalGemsCollected;
        int stepsTaken = levelManager.currentLevelTotalStepsTaken;

        int reward = gemsCollected * 100;

        // Movement efficiency score
        float efficiency =
            (float)levelManager.currentLevelTotalTiles /
            Mathf.Max(stepsTaken, 1);

        int movementScore = Mathf.Clamp(
            Mathf.RoundToInt(efficiency * 100f),
            0,
            100
        );

        // Gem collection score
        int gemScore = Mathf.RoundToInt(
            (gemsCollected /
            (float)Mathf.Max(levelManager.currentLevelTotalGems, 1))
            * 100f
        );

        // Weighted final score
        float combinedScore =
            movementScore * 0.7f +
            gemScore * 0.3f;

        int percentageReward =
            Mathf.RoundToInt(combinedScore);

        int grade = Mathf.Clamp(
            Mathf.FloorToInt(percentageReward / 20f),
            0,
            messages.Length - 1
        );

        // Debug.Log(
        //     $"Movement: {movementScore}% | Gems: {gemScore}% | Combined: {percentageReward}% | Grade: {grade}"
        // );

        reward += Mathf.RoundToInt(
            (percentageReward / 100f) * 500
        );

        if (stepsTaken <= levelManager.currentLevelTotalTiles)
        {
            reward += 500;
        }

        reward = Mathf.Max(reward, 0);

        return (reward, grade);
    }
}
