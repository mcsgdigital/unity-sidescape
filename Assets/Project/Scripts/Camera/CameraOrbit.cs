using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public static CameraOrbit Instance;

    public float rotateSpeed = 5f;
    [Header("Inspect Mode")]
    public Vector3 inspectOffset = new Vector3(0, 2f, -3f);
    public float inspectSmoothSpeed = 8f;

    private CameraFollow cameraFollow;
    private Vector3 currentOffset;
    private Vector3 targetOffset;
    private bool inspectMode;
    private Vector3 normalOffset;


    private void Awake()
    {
        // create instance        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        cameraFollow =
            GetComponent<CameraFollow>();

        currentOffset =
            cameraFollow.offset;

        targetOffset =
            currentOffset;

        normalOffset = currentOffset;
    }

    private void Update()
    {
        Vector3 desiredOffset =
            inspectMode
            ? GetInspectOffset()
            : targetOffset;

        currentOffset = Vector3.Lerp(
            currentOffset,
            desiredOffset,
            inspectSmoothSpeed * Time.deltaTime
        );

        cameraFollow.SetOffset(currentOffset);
    }

    public void RotateRight()
    {
        if (inspectMode) return;

        targetOffset =
            Quaternion.Euler(0, 90, 0) *
            targetOffset;
    }

    public void RotateLeft()
    {
        if (inspectMode) return;

        targetOffset =
            Quaternion.Euler(0, -90, 0) *
            targetOffset;
    }

    private Vector3 GetInspectOffset()
    {
        Vector3 direction =
            targetOffset.normalized;

        return direction * inspectOffset.z
            + Vector3.up * inspectOffset.y;
    }

    public void EnterInspectMode()
    {
        inspectMode = true;
        AudioManager.Instance.PlayWhoosh();
    }

    public void ExitInspectMode()
    {
        inspectMode = false;
        AudioManager.Instance.PlayWhoosh();
    }

    public bool IsInspecting()
    {
        return inspectMode;
    }
}