using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType tileType = TileType.Normal;

    private GridManager gridManager;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    public void RemoveTile()
    {
        gridManager.UnregisterTile(this);

        Destroy(gameObject);
    }
}