using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MageFire : MonoBehaviour
{
    [SerializeField] Renderer Model;
    [SerializeField] NavMeshAgent mageAgent;
    [SerializeField] Animator anim;
    [SerializeField] Transform castPos;
    [SerializeField] Transform headPos;

    [SerializeField] int HP;
    [SerializeField] int animTranSpeed;
    [SerializeField] int playerTargetSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;

    bool isCasting;
    bool playerInRange;
    bool destChosen;

    float angleToPlayer;
    float stoppingDistOrig;

    [SerializeField] float castRate;
    [SerializeField] int castAngle;
    [SerializeField] GameObject fireball;

    Vector3 playerDirec;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager.instance.updateGameGoal(1);

        startingPos = transform.position;
        stoppingDistOrig = mageAgent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        float agentSpeed = mageAgent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentSpeed, Time.deltaTime * animTranSpeed));

        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            StartCoroutine(roam());
        }
    }

    bool canSeePlayer()
    {
        playerDirec = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirec.x, playerDirec.y + 1, playerDirec.z), transform.forward);

        Debug.Log(angleToPlayer);

        Debug.DrawRay(headPos.position, new Vector3(playerDirec.x, playerDirec.y + 1, playerDirec.z));


        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirec, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                faceTarget();


                if (!isCasting && angleToPlayer <= castAngle)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }

        mageAgent.stoppingDistance = 0;
        return false;
    }


    void faceTarget()
    {
        Quaternion rotate = Quaternion.LookRotation(playerDirec);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * playerTargetSpeed);
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

    IEnumerator roam()
    {
        if (!destChosen && mageAgent.remainingDistance < 0.05f)
        {
            destChosen = true;
            yield return new WaitForSeconds(roamTimer);

            mageAgent.stoppingDistance = 0;

            // Keep his roam area small
            Vector3 ranPos = Random.insideUnitSphere * roamDist;
            ranPos += startingPos;

            // Keeps on the NavMesh
            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
            mageAgent.SetDestination(hit.position);

            destChosen = false;
        }
    }

    IEnumerator shoot()
    {
        isCasting = true;
        anim.SetTrigger("Cast");

        Instantiate(fireball, castPos.position, transform.rotation);
        yield return new WaitForSeconds(castRate);
        isCasting = false;
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

        if (HP <= 0)
        {
            //gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }
}
