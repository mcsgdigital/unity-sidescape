using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    private CameraFollow cameraFollow;

    private Vector3 currentOffset;
    private Vector3 targetOffset;

    public float rotateSpeed = 5f;

    private void Start()
    {
        cameraFollow =
            GetComponent<CameraFollow>();

        currentOffset =
            cameraFollow.offset;

        targetOffset =
            currentOffset;
    }

    private void Update()
    {
        currentOffset = Vector3.Lerp(
            currentOffset,
            targetOffset,
            rotateSpeed * Time.deltaTime
        );

        cameraFollow.SetOffset(currentOffset);
    }

    public void RotateRight()
    {
        targetOffset =
            Quaternion.Euler(0, 90, 0) *
            targetOffset;
    }

    public void RotateLeft()
    {
        targetOffset =
            Quaternion.Euler(0, -90, 0) *
            targetOffset;
    }
}