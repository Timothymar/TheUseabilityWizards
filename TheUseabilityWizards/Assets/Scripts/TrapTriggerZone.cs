using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTriggerZone : MonoBehaviour
{
    
    public List<Collider> colliders = new List<Collider>();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            colliders.Add(other);
        }

    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            colliders.Remove(other);
        }
    }








}
