using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileEnemy : MonoBehaviour {

    public float IdleTime = 0;
    public float AimTime;
    public float AimSpeed;
    public float FireRate;
    public float Accuracy;

    //public Transform EnemyParent;
    public Transform PlayerTrans;

    public Transform GOHat;
    public Transform GOHead;
    public Transform GOBase;
    public Transform GOMissileHolder;
    public GameObject EnemyMissilePrefab;
    public List<Transform> FiringPoints = new List<Transform>();
    int currentFireBarrel = 0;

    int UnitHealth = 5;

    // Use this for initialization
    void Start()
    {
        Invoke("Aim", IdleTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Aim()
    {
        StartCoroutine("AimmingBehavior");
    }

    IEnumerator AimmingBehavior()
    {
        //Vars for the while looks
        float t = AimTime / 3;
        float rotateAmount = 0;
        float c = 0;
        Quaternion LookDirection;
        Quaternion FromDirection;

        //Animate Head
        LookDirection = Quaternion.LookRotation(PlayerTrans.position - transform.position, Vector3.up);
        FromDirection = GOHead.rotation;
        LookDirection.z = 0;
        LookDirection.x = 0;
        while (c < t)
        {
            GOHead.rotation = Quaternion.Slerp(FromDirection, LookDirection, rotateAmount);
            rotateAmount += (Time.deltaTime * AimSpeed);
            c += Time.deltaTime;
            yield return null;

        }

        //Animate Base
        rotateAmount = 0;
        c = 0;
        LookDirection = Quaternion.LookRotation(PlayerTrans.position - transform.position, Vector3.up);
        FromDirection = GOBase.rotation;
        LookDirection.z = 0;
        LookDirection.x = 0;
        while (c < t)
        {
            GOBase.rotation = Quaternion.Slerp(FromDirection, LookDirection, rotateAmount);
            rotateAmount += (Time.deltaTime * AimSpeed);
            c += Time.deltaTime;
            yield return null;

        }

        //Missile Holder Guns
        rotateAmount = 0;
        c = 0;
        LookDirection = Quaternion.LookRotation(PlayerTrans.position - transform.position, Vector3.up);
        Quaternion MissileHolderDirection = GOMissileHolder.rotation;
        MissileHolderDirection.x = 0;
        while (c < t)
        {
            GOMissileHolder.rotation = Quaternion.Slerp(MissileHolderDirection, LookDirection, rotateAmount);
            rotateAmount += (Time.deltaTime * AimSpeed);
            c += Time.deltaTime;
            yield return null;

        }

        StartCoroutine("FiringBehavior");
    }

    IEnumerator FiringBehavior()
    {
        // Play Sound Fire Laser
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Enemies/MissileTurret_FireWeapon", gameObject);

        Transform FiringBarrel = FiringPoints[currentFireBarrel];
        GameObject newMissile = Instantiate(EnemyMissilePrefab, FiringBarrel.position, FiringPoints[currentFireBarrel].rotation) as GameObject;
        newMissile.GetComponent<MissileScript>().LaunchMissile(PlayerTrans, 5f);
        if (currentFireBarrel < FiringPoints.Count - 1)
        {
            currentFireBarrel++;
        }

        yield return new WaitForSeconds(FireRate);

        //KEEP FIRING MISSILES UNTIL WE ARE OUT
        if (currentFireBarrel < FiringPoints.Count - 1)
        {
            StartCoroutine("FiringBehavior");
        }
        else
        {
            //TODO PUT IN A RELOADING MECHANIC
        }
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
        //Fabric.EventManager.Instance.PostEvent("SFX/Enemy/Damage", gameObject);

        //Move Unit
        float ForceAmount = 5;
        Vector3 force_direction = (transform.position - PlayerTrans.position).normalized;
        transform.GetComponent<Rigidbody>().AddForceAtPosition(force_direction * ForceAmount, c.transform.position, ForceMode.Impulse);

        //Stop from firing

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
        //Fabric.EventManager.Instance.PostEvent("SFX/Enemy/Explode", gameObject);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX_Explosion", gameObject.transform.position);
        FMOD_AudioManager.Instance.SFX_Ship_Rattle.Play();
        FMOD_AudioManager.Instance.SFX_Ship_Rattle.SetParameter("EnemyDistance", (gameObject.transform.position - PlayerTrans.position).magnitude);
        //Debug.Log("Enemy is this far away: " + (gameObject.transform.position - PlayerTrans.position).magnitude);


        //Play explosion particle effect
        ParticleManager._instance.SpawnExplosionParticle(c.position, PlayerTrans.gameObject);

        //Add rigid bodies and force Set kill scripts

        //AddTrailParticle effects

        //Destrot gameobject
        Destroy(gameObject);

    }
}
