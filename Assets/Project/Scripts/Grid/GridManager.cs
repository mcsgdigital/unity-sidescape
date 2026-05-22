using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Dictionary<Vector2Int, Tile> tiles =
        new Dictionary<Vector2Int, Tile>();

    private void Awake()
    {
        RegisterTiles();
    }

    private void RegisterTiles()
    {
        Tile[] foundTiles = FindObjectsOfType<Tile>();

        foreach (Tile tile in foundTiles)
        {
            Vector2Int gridPos = WorldToGrid(tile.transform.position);

            if (!tiles.ContainsKey(gridPos))
            {
                tiles.Add(gridPos, tile);
            }
        }
    }

    public bool HasTileAtPosition(Vector3 worldPos)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);
        return tiles.ContainsKey(gridPos);
    }

    public Tile GetTileAtPosition(Vector3 worldPos)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);

        if (tiles.ContainsKey(gridPos))
            return tiles[gridPos];

        return null;
    }

    public Vector3 GetNextWorldPosition(Vector3 currentPos, Vector2Int direction)
    {
        Vector3 nextPos = currentPos + new Vector3(direction.x, 0, direction.y);

        return new Vector3(
            Mathf.Round(nextPos.x),
            currentPos.y,
            Mathf.Round(nextPos.z)
        );
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.z)
        );
    }

    public void UnregisterTile(Tile tile)
    {
        Vector2Int gridPos = WorldToGrid(tile.transform.position);

        if (tiles.ContainsKey(gridPos))
        {
            tiles.Remove(gridPos);
        }
    }
}