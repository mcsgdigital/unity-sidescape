using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    private Vector2 startTouch;
    private Vector2 endTouch;
    private PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            startTouch = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            endTouch = Input.mousePosition;
            DetectSwipe();
        }
    }

    void DetectSwipe()
    {
        Vector2 delta = endTouch - startTouch;

        if (delta.magnitude < 50f) return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
                player.TryMove(Vector2Int.right);
            else
                player.TryMove(Vector2Int.left);
        }
        else
        {
            if (delta.y > 0)
                player.TryMove(Vector2Int.up);
            else
                player.TryMove(Vector2Int.down);
        }
    }
}
