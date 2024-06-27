using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] Transform[] spawnPos;

    int spawnCount;
    bool isSpawning;
    bool startSpawning;


    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(numToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && spawnCount < numToSpawn && !isSpawning)
        {
            StartCoroutine(spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;
        int arrayPosition = Random.Range(0, spawnPos.Length);

        Instantiate(objectToSpawn, spawnPos[arrayPosition].position, spawnPos[arrayPosition].rotation);
        spawnCount++;

        yield return new WaitForSeconds(spawnTimer);
        isSpawning = false;

    }







}
