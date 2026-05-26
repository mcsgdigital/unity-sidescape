using UnityEngine;

public class LayerVisibilityManager : MonoBehaviour
{
    private PlayerController player;
    private TileVisual[] tileVisuals;

    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();

            if (player == null)
                return;
        }

        if (tileVisuals == null ||
            tileVisuals.Length == 0 ||
            tileVisuals[0] == null)
        {
            tileVisuals = FindObjectsOfType<TileVisual>();
        }

        float playerHeight =
            Mathf.Round(player.transform.position.y - 0.5f);

        foreach (TileVisual tile in tileVisuals)
        {
            if (tile == null)
                continue;

            if (tile.transform.position.y >
                playerHeight)
            {
                tile.SetTransparent();
            }
            else
            {
                tile.SetOpaque();
            }
        }
    }
}