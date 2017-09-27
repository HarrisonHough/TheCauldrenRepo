using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    private float timeFromLastSpawn = -1;
    private float timeToNextSpawn = -1;

    public int enemiesToSpawnThisWave;
    private int enemiesKilledThisWave = 0;
    private int totalEnemiesKilled = 0;
    public int TotalEnemiesKilled { get { return totalEnemiesKilled; } }
    private int enemyCount = 0;
    public int EnemyCount { get { return enemyCount; } }
    private int lastSpawnIndex = -1;

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject deadEnemyPrefab;

    private DeadEnemy[] deadEnemyPool;
    private GameObject deadEnemyHolder;
    [SerializeField]
    private int deadEnemyPoolSize = 4;
    private int deadEnemyIndex = 0;

    private bool newGame = true;

    

    private Enemy[] enemyPool;
    [SerializeField]
    private int enemyPoolSize = 8;
    private int enemyIndex = 0;
    private GameObject enemyHolder;

    private Transform player;
    private GameObject closestEnemy;
    public GameObject ClosestEnemy { get { return closestEnemy; } }

    [SerializeField]
    private Transform[] spawners;

    public static bool spawnerOn = false;

    // Use this for initialization
    void Start() {

        Initialize();
        SpawnEnemyPool();
        SpawnDeadEnemyPool();
    }

    void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (spawners.Length == 0)
        {
            spawners = new Transform[transform.childCount];
            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i] = transform.GetChild(i);
            }
        }
    }

    void SpawnEnemyPool()
    {
        // Create empty object to hold enemies for scene organization
        enemyHolder = new GameObject("Enemy Holder");
        // Initialize enemy pool array
        enemyPool = new Enemy[10];

        // create and assign enemies
        for (int i = 0; i < enemyPool.Length; i++) {
            GameObject enemy = Instantiate(enemyPrefab, spawners[0].transform.position, spawners[0].transform.rotation) as GameObject;
            enemy.transform.parent = enemyHolder.transform;
            enemyPool[i] = enemy.GetComponent<Enemy>();
        }
    }

    void SpawnDeadEnemyPool()
    {
        // Create empty object to hold enemies for scene organization
        deadEnemyHolder = new GameObject("Dead Enemy Holder");
        // Initialize enemy pool array
        deadEnemyPool = new DeadEnemy[deadEnemyPoolSize];

        // create and assign enemies
        for (int i = 0; i < deadEnemyPoolSize; i++)
        {
            GameObject deadEnemy = Instantiate(deadEnemyPrefab, new Vector3(0,-10,0), Quaternion.identity) as GameObject;
            deadEnemy.transform.parent = deadEnemyHolder.transform;
            deadEnemyPool[i] = deadEnemy.GetComponent<DeadEnemy>();
        }
    }

    IEnumerator EnemySpawnWaveCoroutine()
    {

        int spawnCount = 0;

        // TODO change to variable
        while (spawnerOn)
        {
            if (!GameManager.gameOver && spawnCount < enemiesToSpawnThisWave)
            {
                if (Time.time >= timeFromLastSpawn + timeToNextSpawn)
                {
                    // spawn a new monster, regenerate the time to next spawn..
                    SpawnEnemy();
                    GenerateTimeToNextSpawn();
                    timeFromLastSpawn = Time.time;
                    Debug.Log("Spawning enemy " + spawnCount);
                    spawnCount++;
                }
            }
            else {
                Debug.Log("turning spawner off at spawn count " + spawnCount);
                spawnerOn = false;
            }
            yield return null;
        }

        Debug.Log("Finished spawning all enemies in wave");
    }

    private void SpawnEnemy()
    {

        // TODO fix this
        if (SoundManager.Instance.soundEffectsMuted)
        {
            //enemy.GetComponent<AudioSource>().mute = true;
        }
        int spawnPointIndex = GetRandomSpawnPoint();
        
        //set position
        enemyPool[enemyIndex].transform.position = spawners[spawnPointIndex].transform.position;
        //set rotation
        enemyPool[enemyIndex].transform.rotation = spawners[spawnPointIndex].transform.rotation;
        //call enemy restart
        enemyPool[enemyIndex].Restart();

        enemyIndex++;
        if (enemyIndex >= deadEnemyPoolSize)
        {
            enemyIndex = 0;
        }
        Debug.Log("Spawning Enemy " + EnemyCount);

        enemyCount++;
        
        // Now that new enemy is spawn update/check which is closest enemy
        UpdateClosestEnemy();
    }

    private int GetRandomSpawnPoint()
    {
        if (GameManager.wavesComplete == 0)
        {
            return 0;
        }
        int index = (int)Random.Range(0, spawners.Length);
        // don't spawn from same point twice in a row
        while ( index == lastSpawnIndex)
        {
            index = (int)Random.Range(0, spawners.Length);
        }
        //update last spawn index
        lastSpawnIndex = index;
        return index;
    }

    public void Reset()
    {
        totalEnemiesKilled = 0;
        enemyIndex = 0;
        ResetEnemies();
    }

    public void StartSpawningWave()
    {
        
        timeFromLastSpawn = Time.time;
        SetEnemiesToSpawnThisLevel();
        GenerateTimeToNextSpawn();

        enemiesKilledThisWave = 0;
        enemyCount = 0;

        // TODO check this works
        spawnerOn = true;
        StartCoroutine(EnemySpawnWaveCoroutine());
    }

    private void GenerateTimeToNextSpawn()
    {
        //level 1 between 8 - 10 seconds..
        if (GameManager.wavesComplete == 1)
        {
            timeToNextSpawn = Random.Range(8, 11);
        }
        else
        {
            //TODO: scale according to level..
            timeToNextSpawn = Random.Range(5, 8);
        }
    }

    private void ResetEnemies()
    {
        for (int i = 0; i < enemyPool.Length; i++)
        {
            enemyPool[i].SetInactive();
        }
    }

    public void EnemyKilled(Vector3 position, Quaternion rotation)
    {
        //subtract from enemy count
        enemyCount--;
        enemiesKilledThisWave++;
        totalEnemiesKilled++;

        CreateDeadEnemy(position, rotation);

        if (enemiesKilledThisWave  >= enemiesToSpawnThisWave)
        {
            //wave complete
            GameManager.Instance.WaveComplete();
            spawnerOn = false;
            StopCoroutine(EnemySpawnWaveCoroutine());

        }
    }

    void SetEnemiesToSpawnThisLevel()
    {
        enemiesToSpawnThisWave = GameManager.wavesComplete + 1;
        Debug.Log("Enemies to spawn this wave " + enemiesToSpawnThisWave);
    }

    private GameObject GetClosestEnemy()
    {

        // TODO find better way to do this later
        int closestEnemyIndex = -1;
        float distance = 50f;
        for (int i = 0; i < enemyPool.Length; i++)
        {
            float tempDistance = GetDistanceToPlayer(enemyPool[i].gameObject);
            if (enemyPool[i].alive && tempDistance < distance)
            {
                closestEnemyIndex = i;
                distance = tempDistance;
            }
        }

        if (closestEnemyIndex < 0)
        {
            return null;
        }
        return enemyPool[closestEnemyIndex].gameObject;
    }

    private float GetDistanceToPlayer(GameObject gameObject)
    {
        float distance =  Mathf.Abs( Vector3.Distance(gameObject.transform.position, player.position));
        return distance;
    }

    public void UpdateClosestEnemy()
    {
        closestEnemy = GetClosestEnemy();
    }

    public void CreateDeadEnemy(Vector3 position, Quaternion rotation)
    {
        deadEnemyPool[deadEnemyIndex].transform.position = position;
        deadEnemyPool[deadEnemyIndex].transform.rotation = rotation;
        deadEnemyPool[deadEnemyIndex].StartDeathEffect();
        deadEnemyIndex++;
        if (deadEnemyIndex >= deadEnemyPoolSize)
        {
            deadEnemyIndex = 0;
        }


    }
}
