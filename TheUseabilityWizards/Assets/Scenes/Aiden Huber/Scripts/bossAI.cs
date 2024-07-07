using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class bossAI : MonoBehaviour, IDamage
{
    [Header("----Model/Collision----")]
    [SerializeField] Renderer model;
    [SerializeField] Transform headPos;
    [SerializeField] Collider headCol;
    [SerializeField] Collider bodCol;
    [SerializeField] Collider weaponCol;
    [SerializeField] GameObject firedBolt;
    [SerializeField] GameObject grappleMod;
    [SerializeField] Collider grappleCol;

    [Header("----Movement----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;

    bool playerInRange;
    bool destChosen;

    [Header("----Stats----")]
    [SerializeField] int bHP;
    [SerializeField] int maxBHP;
    //[SerializeField] int bStam;
    //[SerializeField] int maxBStam;

    [Header("----Weapon----")]
    [SerializeField] float projRange;
    [SerializeField] float projRate;
    [SerializeField] int projAngle;
    [SerializeField] GameObject projectile;

    bool isShooting;
    bool doesDamage;
    bool isExhausted;

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
        if (gameManager.instance.isBoss == true)
        {
            gameManager.instance.BossHealth();
        } 
        updateBossHealthUI();

        float agentSpeed = agent.velocity.normalized.magnitude;

        if (playerInRange && !canSeePlayer())
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
        if (!destChosen && agent.remainingDistance < 0.05f)
        {
            destChosen = true;
            yield return new WaitForSeconds(roamTimer);     // How long he stops in his chosen position. If it's set to 0, when he reaches his destination, he'll just pick a new one.

            agent.stoppingDistance = 0;

            Vector3 ranPos = Random.insideUnitSphere * roamDist;        // The roam dist is a range.
            ranPos += startingPos;

            NavMeshHit hit;         // This will guarantee the AI's chosen position is on the navmesh, otherwise he'll walk right off the map.
            NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);       // (Where it's at, make sure it hits on the navmesh, how far it is, layer)
            agent.SetDestination(hit.position);

            destChosen = false;
        }

    }

    bool canSeePlayer()
    {
        // Get player position
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y+1, playerDir.z), transform.forward);        // What angle from the enemy is the player? If under a certain angle, player is in the cone.

        // Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, new Vector3(playerDir.x, playerDir.y+1, playerDir.z));         // Draw a ray where the enemy's head is and move it in the dir of the player. This ray is going to check to see if there's anything between enemy and player.

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

                if (!isShooting && angleToPlayer <= projAngle)
                {
                    // Coroutines are like timers.
                    StartCoroutine(ranged());
                }

                return true;
            }
        }
        // Cannot see the player. 
        agent.stoppingDistance = 0;
        return false;
    }

    // This is so he'll still turn within stopping distance.
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

    IEnumerator ranged()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(projRate);
        isShooting = false;
    }

    public void takeDamage(int amt)
    {
        bHP -= amt;
        //firedBolt = gameManager.instance.playerScript.getcomp
        //    weaponList[selectedWeapon].arrowType
        //if(gameManager.instance.player.)
        updateBossHealthUI();
    }

    public void updateBossHealthUI()
    {
        gameManager.instance.bossHP.fillAmount = (float)bHP / maxBHP;
    }

    public void weaponColOn()
    {
        weaponCol.enabled = true;
    }
    public void weaponColOff()
    {
        weaponCol.enabled = false;
    }
}
