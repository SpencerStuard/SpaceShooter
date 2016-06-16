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
    public GameObject CargoShip;

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
    public List<WaveParameters> WaveValues = new List<WaveParameters>();


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

            if (timeSinceLastSpawn > spawnWaitTime && currentWaveEnemyCount < WaveValues[CurrentWaveNumber].NumberOfEnemies)
            {
                StartCoroutine(SpawnEnemy());
                currentWaveEnemyCount++;
            }

            if (currentWaveEnemyCount == WaveValues[CurrentWaveNumber].NumberOfEnemies && EnemyParent.childCount == 0)
            {
                IsInwave = false;
                NextWave();
            }
        }

        if (EnemyParent.childCount == 0)
        {
            //SetMusicParameters(0,0,0,0);
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("EnemyCount", 0);
        }
        else 
        {
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("EnemyCount", 1);
        }

        // Audio for Hover Turrets
        if (EnemyParent.FindChild("HoverTurretPref(Clone)") !=null)
        {
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("PercussionToggle", 1);
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("HighStringsToggle", 1);
        }
        else
        {
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("PercussionToggle", 0);
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("HighStringsToggle", 0);
        }

        // Audio for Fighters
        if (EnemyParent.FindChild("FighterPref(Clone)") != null)
        {
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("LowStringsToggle", 1);
        }
        else
        {
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("LowStringsToggle", 0);
        }

        // Audio for Missile Turrets
        if (EnemyParent.FindChild("MissleTurretPref(Clone)") != null)
        {
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("HighBrassToggle", 1);
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("MidStringsToggle", 1);
        }
        else
        {
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("HighBrassToggle", 0);
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("MidStringsToggle", 0);
        }

        // Audio for Scan Turrets
        if (EnemyParent.FindChild("ScanTurretPref(Clone)") != null)
        {
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("LowWindsToggle", 1);
        }
        else
        {
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("LowWindsToggle", 0);
        }

        // Audio for Cargo Ships
        if (EnemyParent.FindChild("CargoPref(Clone)") != null)
        {
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("LowBrassToggle", 1);
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("HighWindsToggle", 1);
        }
        else
        {
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("LowBrassToggle", 0);
            FMOD_AudioManager.Instance.MUS_Battle.SetParameter("HighWindsToggle", 0);
        }
      
    }

    void SetMusicParameters(float percussionPerameter, float stringsPerameter, float brassPerameter, float woodwindsPerameter)
    {
        /*
        FMOD_AudioManager.Instance.MUS_Battle.SetParameter("PercussionToggle", percussionPerameter);
        FMOD_AudioManager.Instance.MUS_Battle.SetParameter("StringsToggle", stringsPerameter);
        FMOD_AudioManager.Instance.MUS_Battle.SetParameter("BrassToggle", brassPerameter);
        FMOD_AudioManager.Instance.MUS_Battle.SetParameter("WoodwindsToggle", woodwindsPerameter);
         * */

    }

    public void SetUpWave (int WaveNumber)
    {
        CurrentWaveNumber = WaveNumber;
        currentWaveEnemyCount = 0;
        timeSinceLastSpawn = 0;
        spawnWaitTime = 0;
        timeSinceWaveStarted = 0;

        //GET A FIRST SPAWN TIME
        spawnWaitTime = Random.Range(WaveValues[CurrentWaveNumber].MinSpawnTime, WaveValues[CurrentWaveNumber].MaxSpawnTime);

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
        spawnWaitTime = Random.Range(WaveValues[CurrentWaveNumber].MinSpawnTime, WaveValues[CurrentWaveNumber].MaxSpawnTime);

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
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX_Enemy_Warp", SpawnLocation);

        yield return new WaitForSeconds(.52f);

        //newEnemy
        GameObject tempNewEnemy = PickEnemy();
        GameObject newEnemy = Instantiate(tempNewEnemy, SpawnLocation,randomRotation) as GameObject;
        newEnemy.transform.parent = EnemyParent;
        SetUpNewEnemyStatsFromWave(newEnemy);        
    }

    void SetUpNewEnemyStatsFromWave(GameObject newEnemyRef)
    {
        if (newEnemyRef.name == "HoverTurretPref(Clone)")
        { 
            newEnemyRef.GetComponent<HoverTurretEnemy>().PlayerTrans = PlayerTrans;
            newEnemyRef.GetComponent<HoverTurretEnemy>().Accuracy = WaveValues[CurrentWaveNumber].EnemyAccuracy;
            newEnemyRef.GetComponent<HoverTurretEnemy>().AimSpeed = WaveValues[CurrentWaveNumber].EnemyAimTime;
            newEnemyRef.GetComponent<HoverTurretEnemy>().FireRate = WaveValues[CurrentWaveNumber].EnemyFireRate;
        }
        if (newEnemyRef.name == "MissleTurretPref(Clone)")
        {
            newEnemyRef.GetComponent<HoverTurretEnemy>().PlayerTrans = PlayerTrans;
            newEnemyRef.GetComponent<HoverTurretEnemy>().Accuracy = WaveValues[CurrentWaveNumber].EnemyAccuracy;
            newEnemyRef.GetComponent<HoverTurretEnemy>().AimSpeed = WaveValues[CurrentWaveNumber].EnemyAimTime;
            newEnemyRef.GetComponent<HoverTurretEnemy>().FireRate = WaveValues[CurrentWaveNumber].EnemyFireRate;
        }
        if (newEnemyRef.name == "ScanTurretPref(Clone)")
        {
            newEnemyRef.GetComponent<HoverTurretEnemy>().PlayerTrans = PlayerTrans;
            newEnemyRef.GetComponent<HoverTurretEnemy>().Accuracy = WaveValues[CurrentWaveNumber].EnemyAccuracy;
            newEnemyRef.GetComponent<HoverTurretEnemy>().AimSpeed = WaveValues[CurrentWaveNumber].EnemyAimTime;
            newEnemyRef.GetComponent<HoverTurretEnemy>().FireRate = WaveValues[CurrentWaveNumber].EnemyFireRate;
        }
        if (newEnemyRef.name == "CargoPref(Clone)")
        {
            newEnemyRef.GetComponent<HoverTurretEnemy>().PlayerTrans = PlayerTrans;
            newEnemyRef.GetComponent<HoverTurretEnemy>().Accuracy = WaveValues[CurrentWaveNumber].EnemyAccuracy;
            newEnemyRef.GetComponent<HoverTurretEnemy>().AimSpeed = WaveValues[CurrentWaveNumber].EnemyAimTime;
            newEnemyRef.GetComponent<HoverTurretEnemy>().FireRate = WaveValues[CurrentWaveNumber].EnemyFireRate;
        }
        if (newEnemyRef.name == "FighterPref(Clone)")
        {
            newEnemyRef.GetComponent<FighterEnemy>().PlayerTrans = PlayerTrans;
            newEnemyRef.GetComponent<FighterEnemy>().Accuracy = WaveValues[CurrentWaveNumber].EnemyAccuracy;
            newEnemyRef.GetComponent<FighterEnemy>().AimSpeed = WaveValues[CurrentWaveNumber].EnemyAimTime;
            newEnemyRef.GetComponent<FighterEnemy>().FireRate = WaveValues[CurrentWaveNumber].EnemyFireRate;
        }
    }

    GameObject PickEnemy()
    {
        GameObject EnemyGo = null;
        float EW01 = WaveValues[CurrentWaveNumber].Enemy01Weight;
        float EW02 = WaveValues[CurrentWaveNumber].Enemy02Weight;
        float EW03 = WaveValues[CurrentWaveNumber].Enemy03Weight;
        float EW04 = WaveValues[CurrentWaveNumber].Enemy04Weight;
        float EW05 = WaveValues[CurrentWaveNumber].Enemy05Weight;

        float TotalEnemyPercentage = EW01 + EW02 + EW03 + EW04 + EW05;

        float EnemyPickValue = Random.Range(0f, TotalEnemyPercentage);
        //Debug.Log("ENEMYPICKVALUE = " + EnemyPickValue);

        //PICK ENEMY
        if(EW01 > 0)
        {
            if (EnemyPickValue < EW01)
            {
                EnemyGo = BasicTurret;
            }
        }
        if (EW02 > 0)
        {
            if (EnemyPickValue < EW01 + EW02 && EnemyGo == null)
            {
                EnemyGo = ScanTurret;
            }
        }
        if (EW03 > 0)
        {
            if (EnemyPickValue < EW01 + EW02 +EW03 && EnemyGo == null)
            {
                EnemyGo = MissleTurret;
            }
        }
        if (EW04 > 0)
        {
            if (EnemyPickValue < EW01 + EW02 + EW03 + EW04 && EnemyGo == null)
            {
                EnemyGo = Fighter;
            }
        }
        if (EW05 > 0)
        {
            if (EnemyPickValue < EW01 + EW02 + EW03 + EW04 + EW05 && EnemyGo == null)
            {
                EnemyGo = CargoShip;
            }
        }
        if(EnemyGo == null)
        {
            EnemyGo = null;
            Debug.LogError("HAD PROBLEM PICKING ENEMY");
        }
        //Debug.Log("SELECTED ENEMY = " + EnemyGo.name);
        return EnemyGo;
    }

    public void PlayerDied ()
    {
        IsInwave = false;
        CurrentWaveNumber = 0;

        foreach(Transform T in EnemyParent)
        {
            if (T.GetComponent<HoverTurretEnemy>())
                T.GetComponent<HoverTurretEnemy>().DoKilled(T);

            if (T.GetComponent<FighterEnemy>())
                T.GetComponent<FighterEnemy>().DoKilled(T);

        }
    }
}
