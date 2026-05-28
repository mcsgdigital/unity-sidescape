using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent;

    [SerializeField]
    private LevelButtonUI levelButtonPrefab;

    private void Start()
    {
        BuildLevelList();
    }

    private void BuildLevelList()
    {
        foreach (LevelData data in LevelDatabase.Instance.levels)
        {
            LevelButtonUI button =
                Instantiate(
                    levelButtonPrefab,
                    contentParent
                );

            button.Setup(data);
        }
    }
}