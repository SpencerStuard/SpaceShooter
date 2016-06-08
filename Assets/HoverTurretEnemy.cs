using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoverTurretEnemy : MonoBehaviour {

    public float IdleTime;
    public float AimTime;
    public float AimSpeed;
    public float FireRate;
    public float Accuracy;

    public Transform EnemyParent;
    public Transform PlayerTrans;

    public Transform GOHat;
    public Transform GOHead;
    public Transform GOBase;
    public Transform GOLeftGun;
    public Transform GORightGun;
    public GameObject EnemyLaserPrefab;
    public List<Transform> FiringPoints = new List<Transform>();
    int currentFireBarrel = 0;

    // Use this for initialization
    void Start () {
        Invoke("Aim", IdleTime);
	}
	
	// Update is called once per frame
	void Update () {
	
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
        while (c < t)
        {
            GOHead.rotation = Quaternion.Slerp(FromDirection, LookDirection, rotateAmount );
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
        while (c < t)
        {
            GOBase.rotation = Quaternion.Slerp(FromDirection, LookDirection, rotateAmount);
            rotateAmount += (Time.deltaTime * AimSpeed);
            c += Time.deltaTime;
            yield return null;

        }

        //Animate Guns
        rotateAmount = 0;
        c = 0;
        LookDirection = Quaternion.LookRotation(PlayerTrans.position - transform.position, Vector3.up);
        Quaternion LFromDirection = GOLeftGun.rotation;
        Quaternion RFromDirection = GORightGun.rotation;
        LFromDirection.x = 0;
        RFromDirection.x = 0;
        while (c < t)
        {
            GOLeftGun.rotation = Quaternion.Slerp(LFromDirection, LookDirection, rotateAmount);
            GORightGun.rotation = Quaternion.Slerp(RFromDirection, LookDirection, rotateAmount);
            rotateAmount += (Time.deltaTime * AimSpeed);
            c += Time.deltaTime;
            yield return null;

        }

        StartCoroutine("FiringBehavior");
    }

    IEnumerator FiringBehavior()
    {
        Transform FiringBarrel = FiringPoints[currentFireBarrel];
        GameObject newLaser = Instantiate(EnemyLaserPrefab, FiringBarrel.position,FiringPoints[currentFireBarrel].rotation) as GameObject;
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
}
