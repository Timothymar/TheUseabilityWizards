using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPickUp : MonoBehaviour
{
    public int potionPickUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            

            Destroy(gameObject);
        }
    }
}
