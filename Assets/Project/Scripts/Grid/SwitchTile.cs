using UnityEngine;

public class SwitchTile : MonoBehaviour
{
    public Door[] linkedDoors;

    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material activatedMaterial;

    private Renderer objectRenderer;

    private bool activated;


    private void Awake()
    {
        objectRenderer = GetComponentInChildren<Renderer>();
    }

    public void Activate()
    {
        if (activated)
            return;

        activated = true;

        if (objectRenderer != null)
        {
            objectRenderer.material = activatedMaterial;
        }

        foreach (Door door in linkedDoors)
        {
            door.Open();
            door.PlayEffect();
            AudioManager.Instance.PlaySwitch();
            TileEffect tileEffect = GetComponent<TileEffect>();
            if (tileEffect != null)
            {
                tileEffect.PlayEffect();
            }
        }
    }
}