using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetector : MonoBehaviour
{
    private CubeInputActions inputActions;

    private Vector2 startPosition;
    private Vector2 endPosition;

    public float minSwipeDistance = 50f;

    private PlayerController player;
    private Camera mainCamera;

    private CanvasManager canvasManager;


    private void Awake()
    {
        inputActions = new CubeInputActions();
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        mainCamera = Camera.main;

        canvasManager =
        FindObjectOfType<CanvasManager>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Gameplay.TouchPress.started += OnTouchStarted;
        inputActions.Gameplay.TouchPress.canceled += OnTouchEnded;
    }

    private void OnDisable()
    {
        inputActions.Gameplay.TouchPress.started -= OnTouchStarted;
        inputActions.Gameplay.TouchPress.canceled -= OnTouchEnded;
        inputActions.Disable();
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        startPosition = inputActions.Gameplay.TouchPosition.ReadValue<Vector2>();
    }

    private void OnTouchEnded(InputAction.CallbackContext context)
    {
        endPosition = inputActions.Gameplay.TouchPosition.ReadValue<Vector2>();
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (canvasManager != null &&
            canvasManager.IsOpen())
        {
            return;
        }

        Vector2 delta = endPosition - startPosition;

        if (delta.magnitude < minSwipeDistance)
            return;

        Vector2Int direction;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            direction =
                delta.x > 0
                ? GetCameraRelativeDirection(Vector2Int.right)
                : GetCameraRelativeDirection(Vector2Int.left);
        }
        else
        {
            direction =
                delta.y > 0
                ? GetCameraRelativeDirection(Vector2Int.up)
                : GetCameraRelativeDirection(Vector2Int.down);
        }

        // Then move
        player.TryMove(direction);
    }

    private Vector2Int GetCameraRelativeDirection(Vector2Int inputDirection)
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection =
            cameraRight * inputDirection.x +
            cameraForward * inputDirection.y;

        moveDirection = moveDirection.normalized;

        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.z))
        {
            return moveDirection.x > 0
                ? Vector2Int.right
                : Vector2Int.left;
        }
        else
        {
            return moveDirection.z > 0
                ? Vector2Int.up
                : Vector2Int.down;
        }
    }
}