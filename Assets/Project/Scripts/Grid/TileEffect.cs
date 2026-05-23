using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffect : MonoBehaviour
{
    public ParticleSystem effect;

    private Vector3 originalEffectPosition;


    private void Awake()
    {
        StopEffect();
        originalEffectPosition = effect.transform.localPosition;
    }

    public void PlayEffect()
    {
        effect.Play();
    }

    public void StopEffect()
    {
        effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void PlayTeleportIn()
    {
        effect.transform.localPosition = originalEffectPosition; // set back to original position
        SetShapeRotation(90f); // Particles travel UP
        effect.Clear();
        effect.Play();
    }

    public void PlayTeleportOut()
    {
        effect.transform.localPosition = originalEffectPosition + Vector3.up * 5f; // Move up 1 unit
        SetShapeRotation(-90f); // Particles travel DOWN
        effect.Clear();
        effect.Play();

        StartCoroutine(ResetPositionAfterEffect());
    }

    private void SetShapeRotation(float zRotation)
    {
        var shape = effect.shape;
        shape.rotation = new Vector3(shape.rotation.x, shape.rotation.y, zRotation);
    }

    private IEnumerator ResetPositionAfterEffect()
    {
        yield return new WaitForSeconds(effect.main.duration);
        effect.transform.localPosition = originalEffectPosition; // reset to original position
    }
}
