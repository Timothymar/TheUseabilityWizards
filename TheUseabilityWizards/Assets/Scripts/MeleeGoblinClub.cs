using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeGoblinClub : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

 
    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger || other.gameObject == this.gameObject)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damage);
            }
        }
        //Destroy(gameObject);
    }
}
