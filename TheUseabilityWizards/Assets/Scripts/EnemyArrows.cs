using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrows : MonoBehaviour
{
    [SerializeField] Rigidbody arrowbody;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        arrowbody.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
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
