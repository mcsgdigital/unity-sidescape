using System.Collections.Generic;
using UnityEngine;

public class LevelDatabase : MonoBehaviour
{
    public static LevelDatabase Instance;

    public List<LevelData> levels =
        new List<LevelData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        GenerateTestData();
    }

    private void GenerateTestData()
    {
        levels.Clear();

        for (int i = 0; i < 20; i++)
        {
            LevelData level = new LevelData();

            level.levelIndex = i + 1;

            level.unlocked = i < 3;

            level.gemsCollected = Random.Range(0, 6);

            level.totalGems = 5;

            level.bestGrade = Random.Range(0, 5);

            level.unlockRequirement =
                "Collect 10 Gems";

            levels.Add(level);
        }
    }
}