using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrows : MonoBehaviour
{
    [SerializeField] Rigidbody arrowbody;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    Vector3 directionTowardsPlayer;
    // Start is called before the first frame update
    void Start()
    {
        directionTowardsPlayer = (gameManager.instance.player.transform.position - transform.position).normalized;
        arrowbody.velocity = directionTowardsPlayer * speed;
        Destroy(gameObject, destroyTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damage);
        }
        Destroy(gameObject);
    }

}
