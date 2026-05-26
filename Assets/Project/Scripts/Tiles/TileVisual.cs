using UnityEngine;

public class TileVisual : MonoBehaviour
{
    private Renderer[] renderers;
    private Material[] materials;

    private float targetAlpha = 1f;
    private float currentAlpha = 1f;

    public float fadeSpeed = 6f;

    private void Awake()
    {
        renderers =
            GetComponentsInChildren<Renderer>();

        // Collect unique material instances
        materials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
        }
    }

    private void Update()
    {
        if (Mathf.Abs(currentAlpha - targetAlpha) < 0.01f) return;

        currentAlpha = Mathf.Lerp(
            currentAlpha,
            targetAlpha,
            Time.deltaTime * fadeSpeed
        );

        for (int i = 0; i < materials.Length; i++)
        {
            Color color = materials[i].color;
            color.a = currentAlpha;
            materials[i].color = color;
        }
    }

    public void SetTransparent()
    {
        targetAlpha = 0.3f;
    }

    public void SetOpaque()
    {
        targetAlpha = 1f;
    }
}