using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;

    private Transform openPosition;
    private Transform closedPosition;
    private ParticleSystem openEffect;


    private void Awake()
    {
        isOpen = false;
        openPosition = transform.Find("Open");
        closedPosition = transform.Find("Closed");

        openPosition.gameObject.SetActive(false);

        openEffect = GetComponentInChildren<ParticleSystem>();
        StopEffect();
    }

    public void Open()
    {
        isOpen = true;

        closedPosition.gameObject.SetActive(false);
        openPosition.gameObject.SetActive(true);
    }

    public void Close()
    {
        isOpen = false;

        closedPosition.gameObject.SetActive(true);
        openPosition.gameObject.SetActive(false);
    }

    public bool IsBlocking()
    {
        return !isOpen;
    }

    public void StopEffect()
    {
        if (openEffect != null)
        {
            openEffect.Stop();
        }
    }

    public void PlayEffect()
    {
        if (openEffect != null)
        {
            openEffect.Play();
        }
    }
}