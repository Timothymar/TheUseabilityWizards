using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] int hitDamage;
    [SerializeField] int burningDamage;
    [SerializeField] float burnDuration;
    [SerializeField] float burnInterval;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage damageable = other.GetComponent<IDamage>();
            IBurnDamage burnable = other.GetComponent<IBurnDamage>();
            if (damageable != null)
            {
                damageable.takeDamage(hitDamage); // Inital hit damage
            }
            if (burnable != null)
            {
                // Apply Burn Damage
                burnable.applyBurnDamage(burningDamage, burnDuration, burnInterval);
            }

            // Destroy the fireball after hitting the player
            Destroy(gameObject);
        }
    }
}
