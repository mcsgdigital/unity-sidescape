using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource audioSource;

    public AudioClip rollClip;
    public AudioClip landClip;
    public AudioClip breakClip;
    public AudioClip teleportClip;
    public AudioClip switchClip;
    public AudioClip whooshClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayRoll()
    {
        audioSource.PlayOneShot(rollClip);
    }

    public void PlayLand()
    {
        audioSource.PlayOneShot(landClip);
    }

    public void PlayBreak()
    {
        audioSource.PlayOneShot(breakClip);
    }

    public void PlayTeleport()
    {
        audioSource.PlayOneShot(teleportClip);
    }

    public void PlaySwitch()
    {
        audioSource.PlayOneShot(switchClip);
    }

    public void PlayWhoosh()
    {
        audioSource.PlayOneShot(whooshClip);
    }
}