using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    public float smoothSpeed = 5f;

    private Transform target;
    private bool stopFollowing;


    private void LateUpdate()
    {
        if (stopFollowing)
            return;

        if (target == null)
        {
            FindPlayer();
            return;
        }

        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        Quaternion targetRotation =
            Quaternion.LookRotation(
                target.position - transform.position
            );

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                smoothSpeed * Time.deltaTime
            );
    }

    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }

    private void FindPlayer()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }
    }

    public void StopFollowing()
    {
        stopFollowing = true;
    }

    public void StartFollowing()
    {
        stopFollowing = false;
    }
}