using UnityEngine;

public class IceTile : Tile
{
    public override void OnPlayerEnter(PlayerController player)
    {
        player.EnterIce();
    }
}
