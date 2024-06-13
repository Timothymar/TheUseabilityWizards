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

    [SerializeField] float shootRate;
    [SerializeField] GameObject projectile;

    bool isShooting;
    bool playerInRange;
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

        float agentSpeed = enemyAgent.velocity.normalized.magnitude;
        //anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentSpeed, Time.deltaTime * animTransitionSpeed));

        if (playerInRange)
        {
            enemyAgent.SetDestination(gameManager.instance.player.transform.position);

            if(enemyAgent.remainingDistance < enemyAgent.stoppingDistance)
            {
                faceTarget();
            }

            if (!isShooting)
            {
                StartCoroutine(shoot());
            }
        }
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
            playerInRange = true;
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
        anim.SetTrigger("Shoot");

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
        StartCoroutine(flashDamage());
        enemyAgent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }

    }



}
