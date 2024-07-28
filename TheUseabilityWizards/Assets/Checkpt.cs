using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpt : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.SetCheckpoint();
        }
    }
}
