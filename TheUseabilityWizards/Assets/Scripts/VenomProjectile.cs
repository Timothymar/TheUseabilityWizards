using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] float heightHelp;

    Vector3 directionTowardsPlayer;
    Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = (gameManager.instance.player.transform.position + new Vector3(0, heightHelp, 0));
        directionTowardsPlayer = (targetPosition - transform.position).normalized;
        rb.velocity = directionTowardsPlayer * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();
        if(dmg != null)
        {
            dmg.takeDamage(damage);
        }
        Destroy(gameObject);

    }
}
