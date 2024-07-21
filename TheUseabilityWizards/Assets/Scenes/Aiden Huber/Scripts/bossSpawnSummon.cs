using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossSpawnSummon : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;      // Spawn enemy types
    [SerializeField] int numToSpawn;        // How many will spawn
    [SerializeField] int spawnTimer;        // Spawn timing
    [SerializeField] Transform[] spawnPos;      // Where will they spawn
    [SerializeField] AudioSource aud;     // Having this here for later.
    [SerializeField] AudioClip summonSound;

    int spawnCount;

    bool isSpawning;        // Is the spawner on?
    bool startSpawning;
    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(numToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        // For testing purposes.
        if (Input.GetButtonDown("Jump"))
        {
            AudioClip[] audInstMaster = gameManager.instance.audMaster;

            SpawnTrigger();
            aud.PlayOneShot(audInstMaster[Random.Range(0, audInstMaster.Length)]);

        }

        if (startSpawning && spawnCount < numToSpawn && !isSpawning)
        {
            StartCoroutine(spawn());
            startSpawning = false;
        }
    }

    private void SpawnTrigger()
    {
        startSpawning = true;
    }

    IEnumerator spawn()
    {
        isSpawning = true;
        yield return new WaitForSeconds(spawnTimer);        // Spawn immediately and wait.

        int arrayPos = Random.Range(0, spawnPos.Length);
        Instantiate(objectToSpawn, spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        spawnCount++;

        isSpawning = false;
    }
}
