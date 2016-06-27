using UnityEngine;
using System.Collections;

public class MissileScript : MonoBehaviour {

    float timeToTarget;
    float missileMaxSpeed;
    float missleAccelerationRate = 1;
    public AnimationCurve MissileSpeedCurve;
    public int DamageAmount = 5;

    public void LaunchMissile(Transform MyTarget, float TimeToTarget)
    {
        //missileMaxSpeed = MissleMaxSpeed;
        //SET SWERVE AMOUNT
        float distanceToTarget = Vector3.Distance(transform.position, MyTarget.position);
        //totalDistance = distanceToTarget;
        float missleSwerveRatio = .25f;
        float missleSwerveAmount = distanceToTarget * missleSwerveRatio;

        //MAKE PATH
        Vector3 Point1 = transform.position;
        Vector3 Point2 = Vector3.Lerp(transform.position, MyTarget.position,.33f);
        Point2 += new Vector3(Random.Range(-missleSwerveAmount,missleSwerveAmount), Random.Range(-missleSwerveAmount, missleSwerveAmount), Random.Range(-missleSwerveAmount, missleSwerveAmount));
        Vector3 Point3 = Vector3.Lerp(transform.position, MyTarget.position, .66f);
        Point3 += new Vector3(Random.Range(-missleSwerveAmount, missleSwerveAmount), Random.Range(-missleSwerveAmount, missleSwerveAmount), Random.Range(-missleSwerveAmount, missleSwerveAmount));
        Vector3 Point4 = MyTarget.position;

        //PUT OBJECT ON PATH
        //timeToTarget = distanceToTarget;
        //StartCoroutine("AccelerateMissile");
        iTween.MoveTo(gameObject, iTween.Hash("path", new Vector3[] { Point1, Point2, Point3, Point4 }, "time", TimeToTarget, "orienttopath", true, "lookTime", 0.2));

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.transform.tag == "PlayerLaser")
        {

            //TODO ADD IN HEALTH FOR MISSILES

            //TODO ADD IN EFFECT FOR KILLING MISSSILES

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemies/MissileTurret_WeaponHit", gameObject.transform.position);

            //KILL THE OBJECT
            Destroy(gameObject);
        }

    }
}
