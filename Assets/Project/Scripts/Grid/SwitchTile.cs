using UnityEngine;

public class SwitchTile : MonoBehaviour
{
    public Door[] linkedDoors;

    private bool activated;

    public void Activate()
    {
        if (activated)
            return;

        activated = true;

        foreach (Door door in linkedDoors)
        {
            door.Open();
        }
    }
}