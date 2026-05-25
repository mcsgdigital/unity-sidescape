using UnityEngine;

public class DropPreview : MonoBehaviour
{
    public float heightOffset = 0.02f;

    public void Show(Vector3 position)
    {
        gameObject.SetActive(true);

        if (GetComponent<DropPreviewEffect>() != null)
        {
            GetComponent<DropPreviewEffect>().PulseEffect();
        }

        transform.position =
            position + Vector3.up * heightOffset;
    }

    public void Hide()
    {
        if (GetComponent<DropPreviewEffect>() != null)
        {
            GetComponent<DropPreviewEffect>().StopPulseEffect();
        }

        gameObject.SetActive(false);
    }
}