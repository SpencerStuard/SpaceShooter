using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FighterEnemy : MonoBehaviour {

    public enum FighterStates { Arrive, GetAway, TargetPlayer, StrikeRun };
    public FighterStates myState = FighterStates.Arrive;

    public float AimSpeed;
    public float FireRate;
    public float Accuracy;
    public Transform PlayerTrans;
    float distanceToPlayer;

    int UnitHealth = 20;

    public float CurrentSpeed = 0;
    public float TargetSpeed = 1f;
    public float AccelerationRate = 1f;
    public float TurnRate;
    public float BankAmount;
    public float BankSpeed;

    public float StartTurnDistance;
    public float StartStrikeDistance;
    public float EndStrikeDistance;

    float currentBankAmount;
    float targetBankAmount;
    Vector3 GetAwayTarget;
    float lastEulerY;
    int currentFireBarrel = 0;

    //FIRING CANNONS
    public GameObject EnemyLaserPrefab;
    public List<Transform> FiringPoints = new List<Transform>();


    // Use this for initialization
    void Start () {
        TargetSpeed = 20f;
        CurrentSpeed = 5f;
        GetAwayTarget = GetFarPoint();

    }
	
	// Update is called once per frame
	void Update () {

        distanceToPlayer = Vector3.Distance(transform.position, PlayerTrans.position);

        //JUST GOT HERE GET TO FAR AWAY POINT
        if (myState == FighterStates.Arrive && distanceToPlayer > StartTurnDistance)
        {
            TargetSpeed = 45f;
            myState = FighterStates.TargetPlayer;
        }
        if (myState == FighterStates.TargetPlayer && distanceToPlayer < StartStrikeDistance)
        {
            TargetSpeed = 14f;
            myState = FighterStates.StrikeRun;
            StartCoroutine("FiringBehavior");
        }
        if (myState == FighterStates.StrikeRun && distanceToPlayer < EndStrikeDistance)
        {
            GetAwayTarget = GetFarPoint();
            TargetSpeed = 30f;
            StopCoroutine("FiringBehavior");
            myState = FighterStates.GetAway;

        }
        if (myState == FighterStates.GetAway && distanceToPlayer > StartTurnDistance)
        {
            TargetSpeed = 15f;
            myState = FighterStates.TargetPlayer;
        }

        switch (myState)
        {
            case FighterStates.Arrive:
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, Time.deltaTime * AccelerationRate);
                transform.Translate(Vector3.forward * CurrentSpeed * Time.deltaTime, Space.Self);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(GetAwayTarget - transform.position), Time.deltaTime * TurnRate);
                break;
            case FighterStates.GetAway:
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, Time.deltaTime * AccelerationRate);
                transform.Translate(Vector3.forward * CurrentSpeed * Time.deltaTime, Space.Self);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(GetAwayTarget - transform.position), Time.deltaTime * TurnRate);
                break;
            case FighterStates.TargetPlayer:
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, Time.deltaTime * AccelerationRate);
                transform.Translate(Vector3.forward * CurrentSpeed * Time.deltaTime, Space.Self);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(PlayerTrans.position - transform.position), Time.deltaTime * TurnRate);
                break;
            case FighterStates.StrikeRun:
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, Time.deltaTime * AccelerationRate);
                transform.Translate(Vector3.forward * CurrentSpeed * Time.deltaTime, Space.Self);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(PlayerTrans.position - transform.position), Time.deltaTime * TurnRate);
                FireWeapons();
                break;
            default:
                Debug.LogError("PROBLEM FIGHTER ISN'T IN A STATE");
                break;
        }

        if ((gameObject.transform.position - PlayerTrans.position).magnitude <= 10f)
        {
            // Play Sound - Flyby when a certain distance away from player
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Enemies/Fighter_Flyby", gameObject);
        }

        ///BANK CODE NOT QUITE WORKING
        
        Debug.Log("BANK AMOUNT = " + ((transform.eulerAngles.y - lastEulerY) * BankAmount));
        targetBankAmount = (lastEulerY - transform.eulerAngles.y) * BankAmount;
        currentBankAmount = Mathf.Lerp(currentBankAmount, targetBankAmount, Time.deltaTime * BankSpeed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentBankAmount);
        lastEulerY = transform.eulerAngles.y;
        
    }

    void FireWeapons ()
    {
        Debug.Log("FIRING WEAPONGS");

    }

    Vector3 GetFarPoint ()
    {
        int randomSelection = Random.Range(0, 4);

        switch (randomSelection)
        {
            case 0:
                return new Vector3(StartTurnDistance, Random.Range(5f, 20f), Random.Range(-StartTurnDistance, StartTurnDistance));
                break;
            case 1:
                return new Vector3(-StartTurnDistance, Random.Range(5f, 20f), Random.Range(-StartTurnDistance, StartTurnDistance));
                break;
            case 2:
                return new Vector3(Random.Range(-StartTurnDistance, StartTurnDistance), Random.Range(5f, 20f), StartTurnDistance);
                break;
            case 3:
                return new Vector3(Random.Range(-StartTurnDistance, StartTurnDistance), Random.Range(5f, 20f), -StartTurnDistance);
                break;
            default:
                Debug.LogError("PROBLEM GETTING FIGHTER GET AWAY POSITION");
                return Vector3.zero;
                break;
        }
    }

    IEnumerator FiringBehavior()
    {
        // Play Sound Fire Laser
        //Fabric.EventManager.Instance.PostEvent("SFX/Gun/Laser", gameObject);
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Enemies/Fighter_FireWeapon", gameObject);

        Transform FiringBarrel = FiringPoints[currentFireBarrel];
        GameObject newLaser = Instantiate(EnemyLaserPrefab, FiringBarrel.position, FiringPoints[currentFireBarrel].rotation) as GameObject;
        newLaser.GetComponent<LaserScript>().LaserSpeed = -100;
        if (currentFireBarrel < FiringPoints.Count - 1)
        {
            currentFireBarrel++;
        }
        else
        {
            currentFireBarrel = 0;
        }
        yield return new WaitForSeconds(FireRate);

        StartCoroutine("FiringBehavior");
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.transform.tag == "PlayerLaser")
        {
            DoHit(c);
        }

    }

    void DoHit(Collider c)
    {

        //Spawn a particle effect
        ParticleManager._instance.SpawnShildPart(c.transform.position, PlayerTrans.gameObject);

        //Play Sound
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Enemies/Fighter_TakeDamage", gameObject);

        //Reduce health
        UnitHealth--;

        //Check for dead
        if (UnitHealth <= 0)
        {
            DoKilled(c.transform);
        }
    }

    public void DoKilled(Transform c)
    {
        //Playt explosion SFX
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX_Explosion", gameObject.transform.position);
        FMOD_AudioManager.Instance.SFX_Ship_Rattle.Play();
        FMOD_AudioManager.Instance.SFX_Ship_Rattle.SetParameter("EnemyDistance", (gameObject.transform.position - PlayerTrans.position).magnitude);


        //Play explosion particle effect
        ParticleManager._instance.SpawnExplosionParticle(c.position, PlayerTrans.gameObject);

        //Add rigid bodies and force Set kill scripts

        //AddTrailParticle effects

        //Destrot gameobject
        Destroy(gameObject);

    }
}
