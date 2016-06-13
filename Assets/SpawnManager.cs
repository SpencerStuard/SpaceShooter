using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public int musicLayer1TopThreshold;
    public int musicLayer2TopThreshold;
    public int musicLayer3TopThreshold;

    //public int NumberOfEnemiesPerWave;
    int currentWaveEnemyCount;
    int CurrentWaveNumber;
    float timeSinceLastSpawn;
    float spawnWaitTime;
    float timeSinceWaveStarted;
    bool IsInwave;

    public List<int> EnemiesPerWave = new List<int>();


    private static SpawnManager instance = null;

    // Game Instance Singleton
    public static SpawnManager _instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        //SetUpWave();

    }
	
	// Update is called once per frame
	void Update () {

        if (IsInwave) {

            timeSinceWaveStarted += Time.deltaTime;
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn > spawnWaitTime && currentWaveEnemyCount < EnemiesPerWave[CurrentWaveNumber])
            {
                StartCoroutine(SpawnEnemy());
                currentWaveEnemyCount++;
            }

            if (currentWaveEnemyCount == EnemiesPerWave[CurrentWaveNumber] && EnemyParent.childCount == 0)
            {
                IsInwave = false;
                NextWave();
            }
        }

        if (EnemyParent.childCount == 0)
        {
            SetMusicParameters(0,0,0,0);
        }
        else if (EnemyParent.childCount > 0 && EnemyParent.childCount <= musicLayer1TopThreshold)
        {
            SetMusicParameters(1, 0, 0, 0);
        }
        else if (EnemyParent.childCount > musicLayer1TopThreshold && EnemyParent.childCount <= musicLayer2TopThreshold)
        {
            SetMusicParameters(1, 1, 0, 0);
        }
        else if (EnemyParent.childCount > musicLayer2TopThreshold && EnemyParent.childCount <= musicLayer3TopThreshold)
        {
            SetMusicParameters(1, 1, 1, 0);
        }
        else
        {
            SetMusicParameters(1, 1, 1, 1);
        }

        
    }

    void SetMusicParameters(float percussionPerameter, float stringsPerameter, float brassPerameter, float woodwindsPerameter)
    {
        /*Fabric.EventManager.Instance.SetParameter("MUS/Timeline", "PercussionToggle", percussionPerameter, GameManager.Instance.gameObject);
        Fabric.EventManager.Instance.SetParameter("MUS/Timeline", "StringsToggle", stringsPerameter, GameManager.Instance.gameObject);
        Fabric.EventManager.Instance.SetParameter("MUS/Timeline", "BrassToggle", brassPerameter, GameManager.Instance.gameObject);
        Fabric.EventManager.Instance.SetParameter("MUS/Timeline", "WoodwindsToggle", woodwindsPerameter, GameManager.Instance.gameObject);*/

        FMOD_AudioManager.Instance.MUS_Battle.SetParameter("PercussionToggle", percussionPerameter);
        FMOD_AudioManager.Instance.MUS_Battle.SetParameter("StringsToggle", stringsPerameter);
        FMOD_AudioManager.Instance.MUS_Battle.SetParameter("BrassToggle", brassPerameter);
        FMOD_AudioManager.Instance.MUS_Battle.SetParameter("WoodwindsToggle", woodwindsPerameter);

    }

    public void SetUpWave (int WaveNumber)
    {
        CurrentWaveNumber = WaveNumber;
        currentWaveEnemyCount = 0;
        timeSinceLastSpawn = 0;
        spawnWaitTime = 0;
        timeSinceWaveStarted = 0;

        spawnWaitTime = Random.Range(MinSpawnTime,MaxSpawnTime);

        IsInwave = true;
    }

    void NextWave()
    {
        CurrentWaveNumber++;
        GameManager.Instance.StartNextWaveUI(CurrentWaveNumber);
    }

    IEnumerator SpawnEnemy()
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

        //SPAWN WARP IN EFFECT

        ParticleManager._instance.SpawnWarpInEffect(SpawnLocation, PlayerTrans);

        yield return new WaitForSeconds(.52f);

        //newEnemy
        GameObject newEnemy = Instantiate(BasicTurret, SpawnLocation,randomRotation) as GameObject;
        newEnemy.transform.parent = EnemyParent;
        newEnemy.GetComponent<HoverTurretEnemy>().PlayerTrans = PlayerTrans;
        
    }

    public void PlayerDied ()
    {
        IsInwave = false;
        CurrentWaveNumber = 0;

        foreach(Transform T in EnemyParent)
        {
            T.GetComponent<HoverTurretEnemy>().DoKilled(T);

        }
    }
}
