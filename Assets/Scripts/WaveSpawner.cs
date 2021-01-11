using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave
    {
        public string name; // name of wave
        public Transform enemy; // Set the location
        public int count; // set the number of elements to spawn
        public float rate; // How soon after the last enemy does a new enemy appear

    }

    [SerializeField] Wave[] waves; //This will set the number of different waves;
    [SerializeField] Transform[] spawnPoints; // Take the x, y, z values of spawn point
    [SerializeField] float timeBetweenWaves = 5f;

    private int nextWave = 0;
    private float waveCountdown;
    private float searchCountdown = 1f;
    private SpawnState state = SpawnState.COUNTING; // Set the initial state to countdown

    LevelController levelController;

    // Start is called before the first frame update
    void Start()
    {
        //Error checking - check if spawn points have been placed
        if(spawnPoints.Length == 0)
        {
            print("No spawn points referenced");
        }

        waveCountdown = timeBetweenWaves;
        levelController = FindObjectOfType<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == SpawnState.WAITING)
        {
            //Check if enemies are still alive
            if(!EnemyIsAlive())
            {
                //Begin next round
                WaveCompleted();
            }
            else
            {
                return; //wait until the if statement returns true
            }
        }

        //if it's time to start sapwning waves
        if(waveCountdown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                //Start the spawning phase
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }

    }

    IEnumerator SpawnWave(Wave wave)
    {
        print("Spawing Wave: " + wave.name);
        state = SpawnState.SPAWNING;

        //Spawn
        for(int i = 0; i < wave.count; i++)
        {
            //Spawn an enemy, wait and then spawn another
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        state = SpawnState.WAITING;
    }

    void SpawnEnemy(Transform enemy)
    {
        //Choose a random spawn point
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Spawn enemy
        Instantiate(enemy, randomSpawnPoint.position, randomSpawnPoint.rotation);
        print("Spawing Enemy: " + enemy.name);
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            //Reset searchCountdown
            searchCountdown = 1f;
            if(GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;

    }

    void WaveCompleted()
    {
        print("Wave Complited");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length - 1)
        {
            levelController.NextLevel();
            print("Level Complete");
        }
    }
}
