using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragonFire : MonoBehaviour
{
    // Collision requires at least one rigid body.
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 playerPos = gameManager.instance.player.transform.position;
        rb.velocity = (new Vector3(playerPos.x, playerPos.y + 0.5f, playerPos.z) - transform.position).normalized * speed;
        Destroy(gameObject, destroyTime);
    }

    // Make sure your collider in Unity has on trigger checked
    private void OnTriggerEnter(Collider other)
    {
        // This will keep him from dying to shooting his own bubble like a doofus.
        if (other.isTrigger)
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
