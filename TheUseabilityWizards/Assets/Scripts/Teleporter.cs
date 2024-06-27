using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform player, destionation;
    public GameObject playerRg;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRg.SetActive(false);
            player.position = destionation.position;
            playerRg.SetActive(true);
        }
    }
}