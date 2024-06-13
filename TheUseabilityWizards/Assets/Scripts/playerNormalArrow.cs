using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class playerNormalArrow : MonoBehaviour
{

    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
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
            Destroy(gameObject);
        }
        else if (dmg == null)
        {
            rb.velocity = Vector3.zero;
            Destroy(gameObject, destroyTime);
        }
    }
}
