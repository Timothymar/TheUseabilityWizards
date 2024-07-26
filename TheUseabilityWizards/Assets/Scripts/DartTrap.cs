using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrap : MonoBehaviour
{

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject projectile;

    [SerializeField] TrapTriggerZone triggerzone;

    public float shootRate = 1.0f;

    private float shootCooldown = 0f;


    Vector3 playerDirec;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerDirec = (gameManager.instance.player.transform.position - transform.position);

        if (triggerzone.colliders.Count > 0)
        {
            shootCooldown += Time.deltaTime;

            if (shootCooldown >= shootRate)
            {
                Instantiate(projectile, shootPos.position, Quaternion.Euler(playerDirec));

                shootCooldown = 0;
            }

        }
        else
        {
            shootCooldown = 2.0f;
        }
    }

    


}
