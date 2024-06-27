using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StationaryEnemy : MonoBehaviour, IDamage
{

    [SerializeField] Renderer Model;
    [SerializeField] NavMeshAgent enemyAgent;
    [SerializeField] Animator anim;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;

    [SerializeField] int HP;
    [SerializeField] int animTransitionSpeed;
    [SerializeField] int playerTargetSpeed;
    [SerializeField] int viewAngle;

    bool isShooting;
    bool playerInRange;

    
    float angleToPlayer;

    [SerializeField] float shootRate;
    [SerializeField] int shootAngle;
    [SerializeField] GameObject arrow;

    Vector3 playerDirect;
    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        playerDirect = gameManager.instance.player.transform.position - transform.position;

        
        if (playerInRange && canSeePlayer())
        {
            faceTarget();
        }
    }

    bool canSeePlayer()
    {
        playerDirect = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirect.x, playerDirect.y + 1, playerDirect.z), transform.forward);

        Debug.Log(angleToPlayer);

        Debug.DrawRay(headPos.position, new Vector3(playerDirect.x, playerDirect.y + 1, playerDirect.z));


        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirect, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                faceTarget();


                if (!isShooting && angleToPlayer <= shootAngle)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }

        enemyAgent.stoppingDistance = 0;
        return false;
    }


    void faceTarget()
    {
        Quaternion rotate = Quaternion.LookRotation(playerDirect);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * playerTargetSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");

        Instantiate(arrow, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator flashDamage()
    {
        Model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        Model.material.color = Color.white;
    }


    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());
        //stationary enemy should face the player's last known location upon being shot
        faceTarget();

        if(HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }

    }


}
