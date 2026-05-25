using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPreviewEffect : MonoBehaviour
{
    [SerializeField] private float pulseDuration = 0.3f;

    private bool isPulsing;
    private Renderer renderer;
    private Color originalColor;
    private Color targetColor;


    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
        targetColor = Color.white;
    }

    public void StopPulseEffect()
    {
        isPulsing = false;
        renderer.material.color = originalColor;
        StopAllCoroutines();
    }

    public void PulseEffect()
    {
        if (!isPulsing)
        {
            isPulsing = true;
            StartCoroutine(PulseColour());
        }
        else
        {
            return;
        }
    }

    private IEnumerator PulseColour()
    {
        float timer = 0f;

        while (timer < pulseDuration)
        {
            timer += Time.deltaTime;

            renderer.material.color = Color.Lerp(
                originalColor,
                targetColor,
                timer / pulseDuration
            );

            yield return null;
        }

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            renderer.material.color = Color.Lerp(
                originalColor,
                targetColor,
                timer / pulseDuration
            );

            yield return null;
        }

        renderer.material.color = originalColor;
        isPulsing = false;
    }
}
