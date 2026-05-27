using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleEffectExplode : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;


    private void Awake()
    {
        explosionEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void PlayExplosionEffect()
    {
        explosionEffect.Play();

        StartCoroutine(DestroyCollectible());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayExplosionEffect();
        }
    }

    private IEnumerator DestroyCollectible()
    {
        yield return new WaitForSeconds(explosionEffect.main.duration);
        Destroy(gameObject);
    }
}
