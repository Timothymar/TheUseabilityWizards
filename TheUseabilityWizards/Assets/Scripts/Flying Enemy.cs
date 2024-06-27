using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingENemy : MonoBehaviour, IDamage
{
    [SerializeField] Renderer Model;
    [SerializeField] NavMeshAgent enemyAgent;
    [SerializeField] Animator anim;
    [SerializeField] Transform shootPos;

    [SerializeField] int HP;
    [SerializeField] int animTransitionSpeed;
    [SerializeField] int playerTargetSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;

    [SerializeField] float shootRate;
    [SerializeField] float shootAngle;
    [SerializeField] GameObject projectile;

    bool isShooting;
    bool playerInRange;
    bool destChosen;

    Vector3 playerDirect;
    Vector3 startingPos;

    float angleToPlayer;
    float stopDistOrig;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stopDistOrig = enemyAgent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        playerDirect = gameManager.instance.player.transform.position - transform.position;

        float agentSpeed = enemyAgent.velocity.normalized.magnitude;
        //anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentSpeed, Time.deltaTime * animTransitionSpeed));

        if(playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            StartCoroutine(roam());
        }
    }

    IEnumerator roam()
    {
        if (!destChosen && enemyAgent.remainingDistance < 0.05f)
        {
            destChosen = true;
            yield return new WaitForSeconds(roamTimer);

            enemyAgent.stoppingDistance = 0;

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            enemyAgent.SetDestination(hit.position);

            destChosen = false;
        }
    }

    bool canSeePlayer()
    {
        playerDirect = gameManager.instance.player.transform.position - shootPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirect.x, playerDirect.y + 1, playerDirect.z), transform.forward);

        //see the angle
        Debug.Log(angleToPlayer);

        Debug.DrawRay(shootPos.position, new Vector3(playerDirect.x, playerDirect.y + 1, playerDirect.z));

        RaycastHit hit;
        if (Physics.Raycast(shootPos.position, playerDirect, out hit))
        {
            //if can see player
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                enemyAgent.stoppingDistance = stopDistOrig;
                enemyAgent.SetDestination(gameManager.instance.player.transform.position);

                if (enemyAgent.remainingDistance < enemyAgent.stoppingDistance)
                {
                    faceTarget();
                }
                
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


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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

    void faceTarget()
    {
        Quaternion rotate = Quaternion.LookRotation(playerDirect);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * playerTargetSpeed);
    }

    IEnumerator shoot()
    {
        isShooting = true;
        //anim.SetTrigger("Shoot");

        Instantiate(projectile, shootPos.position, transform.rotation);
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
        //StartCoroutine(flashDamage());
        enemyAgent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }

    }



}
