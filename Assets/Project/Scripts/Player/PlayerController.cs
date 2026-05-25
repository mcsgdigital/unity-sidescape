using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AnimationCurve rollCurve;
    public float rollSpeed = 4f;

    [SerializeField] private float initialPositionY = 3f;

    private GameObject gfx;
    private Vector3 targetPosition;
    private Vector2Int bufferedDirection;
    private bool hasBufferedInput;
    private GridManager gridManager;
    private PlayerEffects playerEffects;

    public enum PlayerState
    {
        Idle,
        Rolling,
        Falling,
        Dead,
        Winning,
        Teleporting
    }

    private PlayerState currentState;
    private LevelManager levelManager;
    private bool isSliding;
    private Vector2Int slideDirection;


    private void Awake()
    {
        currentState = PlayerState.Idle;
        levelManager = FindObjectOfType<LevelManager>();
        gfx = transform.GetChild(0).gameObject;
        playerEffects = GetComponent<PlayerEffects>();

        transform.position = new Vector3(transform.position.x, initialPositionY, transform.position.z);
    }

    private void Start()
    {
        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
        }

        StartCoroutine(Fall());
    }

    public void TryMove(Vector2Int direction)
    {
        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();

            if (gridManager == null)
            {
                Debug.LogError("GridManager not found!");
                return;
            }
        }

        if (currentState != PlayerState.Idle)
        {
            bufferedDirection = direction;
            hasBufferedInput = true;
            return;
        }

        Vector3 nextWorldPos = transform.position + new Vector3(direction.x, 0, direction.y);

        Tile targetTile = gridManager.GetTileAtExactHeight(
            nextWorldPos,
            transform.position.y - 0.5f
        );

        if (targetTile != null)
        {
            nextWorldPos =
                targetTile.transform.position + Vector3.up * 0.5f;
        }

        if (targetTile != null && targetTile.tileType == TileType.Door)
        {
            Door door = targetTile.GetComponent<Door>();
            if (door != null && !door.isOpen)
            {
                return;
            }
        }

        if (targetTile != null && targetTile.tileType == TileType.Wall)
        {
            return;
        }

        StartCoroutine(Roll(nextWorldPos, direction));
    }

    private IEnumerator Roll(Vector3 targetPos, Vector2Int direction)
    {
        currentState = PlayerState.Rolling;
        AudioManager.Instance.PlayRoll();
        slideDirection = direction;

        Vector3 pivot = transform.position +
                        (Vector3.down + new Vector3(direction.x, 0, direction.y)) * 0.5f;

        Vector3 axis = Vector3.Cross(Vector3.up,
                        new Vector3(direction.x, 0, direction.y));

        float duration = 1f / rollSpeed;
        float timer = 0f;
        float previousCurveValue = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);
            float curveValue = rollCurve.Evaluate(progress);
            float deltaCurve = curveValue - previousCurveValue;
            float angleStep = deltaCurve * 90f;
            transform.RotateAround(pivot, axis, angleStep);
            previousCurveValue = curveValue;

            yield return null;
        }

        transform.position = targetPos; // snap fix
        transform.rotation = Quaternion.Euler(
            Mathf.Round(transform.eulerAngles.x / 90) * 90,
            Mathf.Round(transform.eulerAngles.y / 90) * 90,
            Mathf.Round(transform.eulerAngles.z / 90) * 90
        );

        AudioManager.Instance.PlayLand();

        HandleTile();

        if (isSliding)
        {
            ContinueSliding();
        }
    }

    private IEnumerator Fall()
    {
        currentState = PlayerState.Falling;

        float fallSpeed = 5f;

        while (true)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            Vector3 checkPosition = new Vector3(
                transform.position.x,
                Mathf.Round(transform.position.y),
                transform.position.z
            );

            Tile tileBelow = gridManager.GetHighestTileBelow(
                transform.position
            );

            // Found landing tile
            if (tileBelow != null &&
                transform.position.y <= tileBelow.transform.position.y + 0.5f)
            {
                // Snap onto tile
                transform.position = tileBelow.transform.position + Vector3.up * 0.5f;

                currentState = PlayerState.Idle;

                AudioManager.Instance.PlayLand();
                playerEffects.LandingSquatch();

                HandleTile();

                yield break;
            }

            // Fell too far = death
            if (transform.position.y < -10f)
            {
                currentState = PlayerState.Dead;

                yield return new WaitForSeconds(0.5f);

                levelManager.LoseLevel();

                yield break;
            }

            yield return null;
        }
    }

    private void HandleTile()
    {
        Tile currentTile = gridManager.GetTileAtExactHeight(
            transform.position,
            transform.position.y - 0.5f
        );

        if (currentTile == null)
        {
            StartCoroutine(Fall());
            return;
        }

        currentTile.OnPlayerEnter(this);

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

            case TileType.Teleport:
                StartCoroutine(HandleTeleport(currentTile));
                break;

            case TileType.Switch:
                SwitchTile switchTile = currentTile.GetComponent<SwitchTile>();

                if (switchTile != null)
                {
                    switchTile.Activate();
                }

                currentState = PlayerState.Idle;
                TryConsumeBufferedInput();
                break;

            case TileType.Door:
                currentState = PlayerState.Idle;
                TryConsumeBufferedInput();
                break;
        }
    }

    private IEnumerator HandleDisappearingTile(Tile tile)
    {
        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlayBreak();

        // Only remove if player is no longer standing on it
        if (gridManager.GetTileAtPosition(transform.position) == tile)
        {
            tile.RemoveTile();

            StartCoroutine(Fall());
            // CameraShake shake = FindObjectOfType<CameraShake>();
            // if (shake != null)
            // {
            //     shake.Shake();
            // }
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

    private IEnumerator HandleTeleport(Tile tile)
    {
        currentState = PlayerState.Teleporting;
        AudioManager.Instance.PlayTeleport();
        tile.PlayTeleportInEffect();

        yield return new WaitForSeconds(1f);

        // Hide player visuals
        gfx.SetActive(false);

        yield return new WaitForSeconds(0.15f);

        Tile linkedTeleport = tile.GetComponent<TileTeleport>().linkedTeleport;

        if (linkedTeleport != null)
        {
            Vector3 destination =
                linkedTeleport.transform.position;

            transform.position =
                destination + Vector3.up * 0.5f;
        }

        yield return new WaitForSeconds(0.1f);

        currentState = PlayerState.Idle;
        tile.StopEffect();
        linkedTeleport.PlayTeleportOutEffect();

        yield return new WaitForSeconds(0.5f);
        // Show visuals AFTER effect begins
        gfx.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        linkedTeleport.StopEffect();

        TryConsumeBufferedInput();
    }

    public void EnterIce()
    {
        isSliding = true;
    }

    private void StopSliding()
    {
        isSliding = false;
    }

    private void ContinueSliding()
    {
        Vector3 nextPos =
            gridManager.GetNextWorldPosition(transform.position, slideDirection);

        Tile nextTile = gridManager.GetTileAtExactHeight(
            nextPos,
            transform.position.y - 0.5f
        );

        // No tile ahead = fall off edge
        if (nextTile == null)
        {
            StopSliding();

            StartCoroutine(Roll(nextPos, slideDirection));

            return;
        }

        // Closed door or blocking tile
        if (nextTile.IsBlocking())
        {
            StopSliding();

            currentState = PlayerState.Idle;

            return;
        }

        // ALWAYS move onto next valid tile
        StartCoroutine(Roll(nextPos, slideDirection));

        // ONLY continue sliding afterward if next tile is ice
        if (!(nextTile is IceTile))
        {
            StopSliding();
        }
    }
}
