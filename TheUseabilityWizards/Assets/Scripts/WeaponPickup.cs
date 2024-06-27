using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] weaponStats weapon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            weapon.arrowsToShoot = weapon.arrowsShootMax;
            weapon.arrowsQuiver = weapon.arrowsQuiverMax;
            gameManager.instance.playerScript.GetArrowsToShoot();
            gameManager.instance.playerScript.GetArrowsQuiver();
            Destroy(gameObject);
        }
    }
}
