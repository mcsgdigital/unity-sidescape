using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;

    private Transform openPosition;
    private Transform closedPosition;


    private void Awake()
    {
        isOpen = false;
        openPosition = transform.Find("Open");
        closedPosition = transform.Find("Closed");

        openPosition.gameObject.SetActive(false);
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
}