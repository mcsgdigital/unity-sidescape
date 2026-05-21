using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType tileType = TileType.Normal;

    public bool IsWalkable()
    {
        return true;
    }
}