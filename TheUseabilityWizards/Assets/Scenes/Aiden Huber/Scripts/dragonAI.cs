using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class dragonAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;
    [SerializeField] Transform firePos;
    [SerializeField] Collider teethCol;

    [SerializeField] int HP;
    [SerializeField] int animTranSpeed;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;

    [SerializeField] float fireballRate;
    [SerializeField] int fireAngle;
    [SerializeField] int fireRange;
    [SerializeField] int attackSpeed; 
    [SerializeField] GameObject fireball;

    bool playerInRange;
    bool destChosen;
    bool isCasting;
    bool isBiting;

    Vector3 playerDir;
    Vector3 startingPos;

    float angleToPlayer;
    float stoppingDistOrig;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position;
        float agentSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentSpeed, Time.deltaTime * animTranSpeed));

        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            StartCoroutine(roam());
        }




        if (playerInRange)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);

            if (agent.remainingDistance < agent.stoppingDistance)
            {
                faceTarget();
            }

            if (!isCasting)
            {
                StartCoroutine(castProjectile());
            }

            if (!isBiting)
            {
                StartCoroutine(bitePlayer());
            }
        }
    }

    IEnumerator roam()
    {
        if (!destChosen && agent.remainingDistance < 0.05f)
        {
            destChosen = true;
            yield return new WaitForSeconds(roamTimer);

            agent.stoppingDistance = 0;

            Vector3 ranPos = Random.insideUnitSphere * roamDist;
            ranPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destChosen = false;
        }

    }
    bool canSeePlayer()
    {
        // Get player pos
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y+1, playerDir.z), transform.forward);

        Debug.DrawRay(headPos.position, new Vector3(playerDir.x, playerDir.y+1, playerDir.z));

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            // Can see the player
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceTarget();
                }

                if (!isCasting && angleToPlayer <= fireAngle && agent.stoppingDistance < agent.remainingDistance)
                {
                    // Coroutines are like timers.
                    StartCoroutine(castProjectile());
                }
                else if (!isBiting && angleToPlayer <= fireAngle && agent.stoppingDistance >= agent.remainingDistance)
                {
                    StartCoroutine(bitePlayer());
                }

                return true;
            }
        }
        // Cannot see the player. 
        agent.stoppingDistance = 0;
        return false;
    }

    // Will still turn within stopping distance.
    void faceTarget()
    {
        // As it is, this makes us snap.
        Quaternion rot = Quaternion.LookRotation(playerDir);
        // So add in a Lerp (being lerp'd, target pos, time)
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
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

    IEnumerator castProjectile()
    {
        isCasting = true;
        anim.SetTrigger("Fireball");

        yield return new WaitForSeconds(fireballRate);
        isCasting = false;
    }

    public void createProjectile()
    {
        Instantiate(fireball, firePos.position, transform.rotation);
    }

    IEnumerator bitePlayer()
    {
        isBiting = true;
        anim.SetTrigger("Bite");

        yield return new WaitForSeconds(attackSpeed);
        isBiting = false;
    }

    public void teethColOn()
    {
        teethCol.enabled = true;
    }
    public void teethColOff()
    {
        teethCol.enabled = false;
    }


    public void takeDamage(int amt)
    {
        HP -= amt;
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
