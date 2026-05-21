using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GridManager gridManager;

    public float rollSpeed = 4f;
    private bool isMoving;
    private Vector3 targetPosition;

    public void TryMove(Vector2Int direction)
    {
        if (isMoving) return;

        Vector3 nextWorldPos = gridManager.GetNextWorldPosition(transform.position, direction);

        StartCoroutine(Roll(nextWorldPos, direction));
    }

    private IEnumerator Roll(Vector3 targetPos, Vector2Int direction)
    {
        isMoving = true;

        Vector3 pivot = transform.position +
                        (Vector3.down + new Vector3(direction.x, 0, direction.y)) * 0.5f;

        Vector3 axis = Vector3.Cross(Vector3.up,
                        new Vector3(direction.x, 0, direction.y));

        float angle = 0f;

        while (angle < 90f)
        {
            float step = rollSpeed * Time.deltaTime * 90f;
            transform.RotateAround(pivot, axis, step);
            angle += step;
            yield return null;
        }

        transform.position = targetPos; // snap fix
        transform.rotation = Quaternion.Euler(
            Mathf.Round(transform.eulerAngles.x / 90) * 90,
            Mathf.Round(transform.eulerAngles.y / 90) * 90,
            Mathf.Round(transform.eulerAngles.z / 90) * 90
        );

        HandleTile();
    }

    private IEnumerator Fall()
    {
        isMoving = true;

        float fallSpeed = 5f;

        while (transform.position.y > -5f)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        Debug.Log("Player Fell!");
    }

    private void HandleTile()
    {
        Tile currentTile = gridManager.GetTileAtPosition(transform.position);

        if (currentTile == null)
        {
            StartCoroutine(Fall());
            return;
        }

        switch (currentTile.tileType)
        {
            case TileType.Normal:
                isMoving = false;
                break;

            case TileType.Goal:
                Debug.Log("LEVEL COMPLETE");
                isMoving = false;
                break;

            case TileType.Disappearing:
                StartCoroutine(HandleDisappearingTile(currentTile));
                break;
        }
    }

    private IEnumerator HandleDisappearingTile(Tile tile)
    {
        yield return new WaitForSeconds(0.2f);

        gridManager.RemoveTile(tile.transform.position);

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(Fall());
    }
}
