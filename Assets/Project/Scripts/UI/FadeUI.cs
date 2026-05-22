using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = timer / fadeDuration;

            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(1f);
    }

    public IEnumerator FadeIn()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = 1f - (timer / fadeDuration);

            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}