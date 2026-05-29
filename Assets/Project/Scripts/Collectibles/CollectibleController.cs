using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    public enum CollectibleType
    {
        Gem,
        Charge
    }

    public CollectibleType currentType;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (currentType)
            {
                case CollectibleType.Gem:
                    LevelManager.Instance.CollectGem();
                    break;

                case CollectibleType.Charge:
                    LevelManager.Instance.CollectCharge();
                    break;
                default:
                    Debug.LogWarning("Unhandled collectible type: " + currentType);
                    break;
            }

        }
    }
}
