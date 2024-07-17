using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class playerCrossBowBolts : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int staminaDrain;
    [SerializeField] int healthDrain;
    [SerializeField] int destroyTime;

    private playerContol player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<playerContol>();
        rb.velocity = Camera.main.transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();

        if (other.isTrigger)
            return;

        if (dmg != null)
        {
            dmg.takeDamage(damage);
            if (staminaDrain > 0)
            {
                player.drainStamina(staminaDrain);
            }
            if (healthDrain > 0)
            {
                player.drainHealth(healthDrain);
            }
            Destroy(gameObject);
        }
        else if (dmg == null)
        {
            rb.velocity = Vector3.zero;
            Destroy(gameObject, destroyTime);
        }
    }
}
