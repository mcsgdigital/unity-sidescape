using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetector : MonoBehaviour
{
    private CubeInputActions inputActions;

    private Vector2 startPosition;
    private Vector2 endPosition;

    public float minSwipeDistance = 50f;

    private PlayerController player;

    private void Awake()
    {
        inputActions = new CubeInputActions();
        player = FindObjectOfType<PlayerController>();
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
        Vector2 delta = endPosition - startPosition;

        if (delta.magnitude < minSwipeDistance)
            return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
                player.TryMove(Vector2Int.left);
            else
                player.TryMove(Vector2Int.right);
        }
        else
        {
            if (delta.y > 0)
                player.TryMove(Vector2Int.down);
            else
                player.TryMove(Vector2Int.up);
        }
    }
}