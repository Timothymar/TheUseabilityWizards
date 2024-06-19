using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MageEnemyAI : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anima;
    [SerializeField] Transform CastPos;

    [SerializeField] int animTranSpeed;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int HP;
    [SerializeField] float castRate;

    [SerializeField] GameObject fireBall;

    bool isCasting;
    bool playerInRange;

    Vector3 playerDir;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position;

        float agentSpeed = agent.velocity.normalized.magnitude;
        anima.SetFloat("Speed", Mathf.Lerp(anima.GetFloat("Speed"), agentSpeed, Time.deltaTime * animTranSpeed));

        if (playerInRange)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);

            if (agent.remainingDistance < agent.stoppingDistance)
            {
                faceTarget();
            }

            if (!isCasting)
            {
                StartCoroutine(cast());
            }
        }
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }

    IEnumerator cast()
    {
        isCasting = true;
        anima.SetTrigger("Cast");

        Instantiate(fireBall, CastPos.position, transform.rotation);

        yield return new WaitForSeconds(castRate);
        isCasting = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
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
