using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankOrc : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Transform headPos;

    [SerializeField] int HP;
    [SerializeField] int animatorTranSpeed;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int visionCone;

    [SerializeField] Transform attackPos;
    [SerializeField] Collider weaponCol;
    [SerializeField] GameObject weaponAxe;

    [SerializeField] float attackRate;
    [SerializeField] float attackAngle;

    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;

    bool isAttacking;
    bool isPlayerInRange;
    bool destChosen;

    Vector3 playerDir;
    Vector3 startingPos;

    float angleToPlayer;
    float stoppingDistOrig;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position;

        float agentSpeed = agent.velocity.normalized.magnitude;
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), agentSpeed, Time.deltaTime * animatorTranSpeed));

        if (isPlayerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (!isPlayerInRange)
        {
            StartCoroutine(roam());
        }
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y + 1, playerDir.z), transform.forward);
        //Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, new Vector3(playerDir.x, playerDir.y + 1, playerDir.z));

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            Debug.Log(hit.collider.name);

            // Hey I can see player
            if (hit.collider.CompareTag("Player") && angleToPlayer <= visionCone)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceTarget();
                }

                if (!isAttacking && angleToPlayer <= attackAngle)
                {
                    StartCoroutine(attack());
                }
                return true;
            }
        }

        agent.stoppingDistance = stoppingDistOrig;
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
            GetComponent<LootBag>().InstantiateLoot(transform.position);
        }
    }

    IEnumerator attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    IEnumerator roam()
    {
        if (!destChosen && agent.remainingDistance < 0.05f)
        {
            destChosen = true;
            yield return new WaitForSeconds(roamTimer);

            agent.stoppingDistance = 0;

            // Keep his roam area small
            Vector3 ranPos = Random.insideUnitSphere * roamDist;
            ranPos += startingPos;

            // Keeps on the NavMesh
            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destChosen = false;
        }
    }

    public void createAxe()
    {
        Instantiate(weaponAxe, attackPos.position, transform.rotation);
    }

    public void weaponClubOn()
    {
        weaponCol.enabled = true;
    }

    public void weaponClubOff()
    {
        weaponCol.enabled = false;
    }
}
