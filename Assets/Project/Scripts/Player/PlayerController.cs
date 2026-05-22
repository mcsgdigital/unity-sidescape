using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GridManager gridManager;

    public float rollSpeed = 4f;
    private Vector3 targetPosition;
    private Vector2Int bufferedDirection;
    private bool hasBufferedInput;

    public enum PlayerState
    {
        Idle,
        Rolling,
        Falling,
        Dead,
        Winning
    }

    private PlayerState currentState;
    private LevelManager levelManager;

    private void Awake()
    {
        currentState = PlayerState.Idle;
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void TryMove(Vector2Int direction)
    {
        if (currentState != PlayerState.Idle)
        {
            bufferedDirection = direction;
            hasBufferedInput = true;
            return;
        }

        Vector3 nextWorldPos = gridManager.GetNextWorldPosition(transform.position, direction);

        StartCoroutine(Roll(nextWorldPos, direction));
    }

    private IEnumerator Roll(Vector3 targetPos, Vector2Int direction)
    {
        currentState = PlayerState.Rolling;

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
        currentState = PlayerState.Falling;

        float fallSpeed = 5f;

        while (transform.position.y > -5f)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        currentState = PlayerState.Dead;

        yield return new WaitForSeconds(0.5f);

        levelManager.LoseLevel();
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
                currentState = PlayerState.Idle;
                TryConsumeBufferedInput();
                break;

            case TileType.Goal:
                currentState = PlayerState.Winning;
                levelManager.WinLevel();
                break;

            case TileType.Disappearing:
                currentState = PlayerState.Idle;
                TryConsumeBufferedInput();
                StartCoroutine(HandleDisappearingTile(currentTile));
                break;
        }
    }

    private IEnumerator HandleDisappearingTile(Tile tile)
    {
        yield return new WaitForSeconds(1f);

        // Only remove if player is no longer standing on it
        if (gridManager.GetTileAtPosition(transform.position) == tile)
        {
            tile.RemoveTile();

            StartCoroutine(Fall());
        }
        else
        {
            tile.RemoveTile();
        }
    }

    private void TryConsumeBufferedInput()
    {
        if (!hasBufferedInput)
            return;

        if (currentState != PlayerState.Idle)
            return;

        hasBufferedInput = false;

        Vector2Int direction = bufferedDirection;

        TryMove(direction);
    }
}
