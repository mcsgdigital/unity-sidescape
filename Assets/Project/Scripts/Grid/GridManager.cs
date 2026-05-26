using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    private List<Tile> allTiles = new List<Tile>();

    private void Awake()
    {
        RegisterTiles();
    }

    private void RegisterTiles()
    {
        Tile[] foundTiles = FindObjectsOfType<Tile>();

        foreach (Tile tile in foundTiles)
        {
            RegisterTile(tile);
        }

        foreach (Tile tile in allTiles)
        {
            Vector2Int gridPos = WorldToGrid(tile.transform.position);

            // ONLY register highest tile for movement dictionary
            if (!tiles.ContainsKey(gridPos))
            {
                tiles.Add(gridPos, tile);
            }
            else
            {
                // Keep highest tile
                if (tile.transform.position.y >
                    tiles[gridPos].transform.position.y)
                {
                    tiles[gridPos] = tile;
                }
            }
        }
    }

    public void RegisterTile(Tile tile)
    {
        if (!allTiles.Contains(tile))
        {
            allTiles.Add(tile);
        }

        Vector2Int gridPos = WorldToGrid(tile.transform.position);

        if (!tiles.ContainsKey(gridPos))
        {
            tiles.Add(gridPos, tile);
        }
        else
        {
            if (tile.transform.position.y >
                tiles[gridPos].transform.position.y)
            {
                tiles[gridPos] = tile;
            }
        }
    }

    public void UnregisterTile(Tile tile)
    {
        allTiles.Remove(tile);

        Vector2Int gridPos = WorldToGrid(tile.transform.position);

        if (tiles.ContainsKey(gridPos))
        {
            if (tiles[gridPos] == tile)
            {
                tiles.Remove(gridPos);

                Tile highestReplacement = null;

                foreach (Tile otherTile in allTiles)
                {
                    if (otherTile == null)
                        continue;

                    Vector2Int otherPos =
                        WorldToGrid(otherTile.transform.position);

                    if (otherPos != gridPos)
                        continue;

                    if (highestReplacement == null ||
                        otherTile.transform.position.y >
                        highestReplacement.transform.position.y)
                    {
                        highestReplacement = otherTile;
                    }
                }

                if (highestReplacement != null)
                {
                    tiles.Add(gridPos, highestReplacement);
                }
            }
        }
    }

    public bool HasTileAtPosition(Vector3 worldPos)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);
        return tiles.ContainsKey(gridPos);
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

    public Tile GetTileAtExactHeight(Vector3 position, float height)
    {
        foreach (Tile tile in allTiles)
        {
            if (tile == null)
                continue;

            bool sameXZ =
                Mathf.RoundToInt(tile.transform.position.x) ==
                Mathf.RoundToInt(position.x)
                &&
                Mathf.RoundToInt(tile.transform.position.z) ==
                Mathf.RoundToInt(position.z);

            bool sameHeight =
                Mathf.Abs(tile.transform.position.y - height) < 0.1f;

            if (sameXZ && sameHeight)
            {
                return tile;
            }
        }

        return null;
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.z)
        );
    }

    public Tile GetHighestTileBelow(Vector3 position)
    {
        Tile bestTile = null;

        foreach (Tile tile in allTiles)
        {
            if (tile == null)
                continue;

            bool sameXZ =
                Mathf.RoundToInt(tile.transform.position.x) ==
                Mathf.RoundToInt(position.x)
                &&
                Mathf.RoundToInt(tile.transform.position.z) ==
                Mathf.RoundToInt(position.z);

            if (!sameXZ)
                continue;

            // Tile must be below player
            if (tile.transform.position.y >= position.y)
                continue;

            // Keep highest valid tile below
            if (bestTile == null ||
                tile.transform.position.y > bestTile.transform.position.y)
            {
                bestTile = tile;
            }
        }

        return bestTile;
    }

    public Tile GetHighestTileBelowPosition(Vector3 position)
    {
        Tile bestTile = null;

        foreach (Tile tile in allTiles)
        {
            bool sameXZ =
                Mathf.RoundToInt(tile.transform.position.x) ==
                Mathf.RoundToInt(position.x)
                &&
                Mathf.RoundToInt(tile.transform.position.z) ==
                Mathf.RoundToInt(position.z);

            if (!sameXZ)
                continue;

            if (tile.transform.position.y >= position.y)
                continue;

            if (bestTile == null ||
                tile.transform.position.y > bestTile.transform.position.y)
            {
                bestTile = tile;
            }
        }

        return bestTile;
    }
}