using UnityEngine;

public class DropPreviewManager : MonoBehaviour
{
    [SerializeField]
    private DropPreview previewPrefab;

    private DropPreview[] previews;

    private Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    private PlayerController player;
    private GridManager gridManager;

    private void Start()
    {
        previews = new DropPreview[4];

        for (int i = 0; i < previews.Length; i++)
        {
            previews[i] =
                Instantiate(previewPrefab, transform);

            previews[i].Hide();
        }
    }

    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
            gridManager = FindObjectOfType<GridManager>();
            return;
        }

        if (!player.IsIdle())
        {
            HideAll();
            return;
        }

        UpdatePreviews();
    }

    private void UpdatePreviews()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int direction = directions[i];

            Vector3 nextPos =
                player.transform.position +
                new Vector3(direction.x, 0, direction.y);

            Tile sameLevelTile =
                gridManager.GetTileAtExactHeight(
                    nextPos,
                    Mathf.Round(player.transform.position.y - 0.5f)
                );

            // Normal walkable tile exists
            if (sameLevelTile != null)
            {
                previews[i].Hide();
                continue;
            }

            // Check below
            Tile landingTile =
                gridManager.GetHighestTileBelow(nextPos);

            if (landingTile != null)
            {
                previews[i].Show(
                    landingTile.transform.position
                );
            }
            else
            {
                previews[i].Hide();
            }
        }
    }

    private void HideAll()
    {
        foreach (DropPreview preview in previews)
        {
            preview.Hide();
        }
    }
}