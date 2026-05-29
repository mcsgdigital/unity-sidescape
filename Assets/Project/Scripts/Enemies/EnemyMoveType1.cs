using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveType1 : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 3f;

    [SerializeField] private Transform targetDestination;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMovingForward = true;


    private void Start()
    {
        startPosition = transform.position;
        targetPosition = targetDestination.position;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // Move towards the target position and back to the start position
        float step = moveSpeed * Time.deltaTime;

        if (isMovingForward)
        {
            // Move towards target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                isMovingForward = false;
            }
        }
        else
        {
            // Move back to start
            transform.position = Vector3.MoveTowards(transform.position, startPosition, step);

            if (Vector3.Distance(transform.position, startPosition) < 0.001f)
            {
                isMovingForward = true;
            }
        }
    }
}
