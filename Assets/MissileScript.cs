using UnityEngine;
using System.Collections;

public class MissileScript : MonoBehaviour {

    float timeToTarget;
    float missileMaxSpeed;
    float missleAccelerationRate = 1;

    public void LaunchMissile (Transform MyTarget, float MissleMaxSpeed)
    {
        missileMaxSpeed = MissleMaxSpeed;

        //SET SWERVE AMOUNT
        float distanceToTarget = Vector3.Distance(transform.position, MyTarget.position);
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
        timeToTarget = distanceToTarget;
        StartCoroutine("AccelerateMissile");
        iTween.MoveTo(gameObject, iTween.Hash("path", new Vector3[] { Point1, Point2, Point3, Point4 }, "time", timeToTarget, "orienttopath", true));

    }

    IEnumerator AccelerateMissile()
    {
        while (timeToTarget > missileMaxSpeed)
        {
            //TODO CHANGE FROM LINEAR TO A EXPONECIAL RATE
            timeToTarget -= Time.deltaTime * missleAccelerationRate;
            yield return null;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.transform.tag == "PlayerLaser")
        {

            //TODO ADD IN HEALTH FOR MISSILES

            //TODO ADD IN EFFECT FOR KILLING MISSSILES

            //TODO ADD SOUND EFFECT

            //KILL THE OBJECT
            Destroy(gameObject);
        }

    }
}
