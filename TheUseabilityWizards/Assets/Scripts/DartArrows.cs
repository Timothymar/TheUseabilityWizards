using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartArrows : MonoBehaviour
{
    [SerializeField] Rigidbody body;
    [SerializeField] int damage;
    [SerializeField] float FlySpeed;
    Vector3 directionTowardsPlayer;
     
    public float projDuration = 5.0f;

    private float reloadTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        directionTowardsPlayer = (gameManager.instance.player.transform.position - transform.position).normalized;
        body.velocity = directionTowardsPlayer * FlySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += FlySpeed * transform.right * Time.deltaTime;

        reloadTime += Time.deltaTime;

        if(reloadTime > projDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
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
