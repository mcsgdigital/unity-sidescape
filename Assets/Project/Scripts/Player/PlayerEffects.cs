using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    public void LandingSquatch()
    {
        StartCoroutine(LandingSquatchRoutine());
    }

    private IEnumerator LandingSquatchRoutine()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 squatchScale = new Vector3(1.2f, 0.8f, 1f);

        float timer = 0f;
        float duration = 0.15f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            transform.localScale = Vector3.Lerp(
                originalScale,
                squatchScale,
                timer / duration
            );

            yield return null;
        }

        // Return to original scale
        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            transform.localScale = Vector3.Lerp(
                squatchScale,
                originalScale,
                timer / duration
            );

            yield return null;
        }
    }
}
