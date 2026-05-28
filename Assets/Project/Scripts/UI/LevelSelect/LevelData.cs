using System;

[Serializable]
public class LevelData
{
    public int levelIndex;

    public bool unlocked;

    public int gemsCollected;
    public int totalGems;

    public int bestGrade;

    public string unlockRequirement;
}