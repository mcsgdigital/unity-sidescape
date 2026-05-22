using System.Collections;
using System.Collections.Generic;
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

        StartCoroutine(Break());
    }

    public IEnumerator Break()
    {
        Vector3 originalScale = transform.localScale;

        float timer = 0f;
        float duration = 0.15f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            transform.localScale = Vector3.Lerp(
                originalScale,
                Vector3.zero,
                timer / duration
            );

            yield return null;
        }

        Destroy(gameObject);
    }
}