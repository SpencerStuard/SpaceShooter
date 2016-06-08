using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    public Transform PlayerTrans;
    public Transform EnemyParent;
    public GameObject BasicTurret;
    public GameObject MissleTurret;
    public GameObject ScanTurret;
    public GameObject Fighter;

    public float MinSpawnTime;
    public float MaxSpawnTime;
    public float MaxSpawnRange;
    public float MinSpawnRange = 4f;
    float timeSinceLastSpawn;
    float spawnWaitTime;
    float timeSinceWaveStarted;

    // Use this for initialization
    void Start () {
        SetUpWave();

    }
	
	// Update is called once per frame
	void Update () {

        timeSinceWaveStarted += Time.deltaTime;
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn > spawnWaitTime)
        {
            SpawnEnemy();
        }

    }

    public void SetUpWave ()
    {
        timeSinceLastSpawn = 0;
        spawnWaitTime = 0;
        timeSinceWaveStarted = 0;

        spawnWaitTime = Random.Range(MinSpawnTime,MaxSpawnTime);
    }

    void SpawnEnemy()
    {
        timeSinceLastSpawn = 0;
        spawnWaitTime = 0;
        spawnWaitTime = Random.Range(MinSpawnTime, MaxSpawnTime);

        //Get spawn location and make sure it is not too close top palyer
        Vector3 SpawnLocation = new Vector3(Random.Range(-MaxSpawnRange, MaxSpawnRange), Random.Range(-5f, MaxSpawnRange/2), Random.Range(-MaxSpawnRange, MaxSpawnRange));
        while (Vector3.Distance(SpawnLocation, PlayerTrans.position) < MinSpawnRange)
        {
            SpawnLocation = new Vector3(Random.Range(-MaxSpawnRange, MaxSpawnRange), Random.Range(-5f, MaxSpawnRange), Random.Range(-MaxSpawnRange, MaxSpawnRange));
        }
        //get a random rotation and zero out the y
        Quaternion randomRotation = Random.rotation;
        randomRotation.x = 0;
        randomRotation.z = 0;

        //newEnemy
        GameObject newEnemy = Instantiate(BasicTurret, SpawnLocation,randomRotation) as GameObject;
        newEnemy.transform.parent = EnemyParent;
        newEnemy.GetComponent<HoverTurretEnemy>().PlayerTrans = PlayerTrans;
        
    }
}
