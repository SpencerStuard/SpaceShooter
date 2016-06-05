using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunDirection : MonoBehaviour
{
    //LASER STATS
    public float LaserSpeed;
    public float LaserLife;

    public AnimationClip LeftAnim;
    public AnimationCurve VertMapping;
    public AnimationCurve HorzMapping;

    public GameObject HeadSet;
    public GameObject MiddleCube;

    public GameObject GunRoot;
    public float RotationDamping = 1f;

    public Rigidbody GunBase;

    public GameObject GunSwivle;

    public Transform leftHand;
    public Transform rightHand;

    private Vector3 midpoint;

    private bool LeftFireFlag;
    private bool RightFireFlag;

    public GameObject leftCannonObj;
    public GameObject rightCannonObj;

    public GameObject leftGrabPoint;
    public GameObject rightGrabPoint;

    //public SteamVR_TrackedObject trackedObjR;
    public SteamVR_TrackedObject trackedObjL;
    public SteamVR_TrackedObject trackedObjR;

    public Transform LCannonPoint;
    public Transform RCannonPoint;
    public GameObject LaserPrefab;
    public GameObject SmokePartPrefab;

    void Awake()
    {
        //trackedObjR = GetComponent<SteamVR_TrackedObject>();
        //trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Use this for initialization
    void Start()
    {
        Fabric.EventManager.Instance.PostEvent("SFX/Gun/Roller", gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.DrawLine(leftHand.position, rightHand.position);
        midpoint = leftHand.position + (rightHand.position - leftHand.position) / 2;

        MiddleCube.transform.position = midpoint;
        MiddleCube.transform.LookAt(rightHand.transform,Vector3.right);
        //Debug.DrawRay(midpoint, Vector3.Cross(GunSwivle.right.normalized, GunSwivle.up.normalized));
        float angle = Vector3.Angle(rightHand.transform.position, leftHand.transform.position);

        //Debug.Log( angle );
        
        //Rotate Base
        Vector3 lookPos = midpoint - transform.position;
        lookPos = new Vector3(lookPos.x, 0, lookPos.z);
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        GunRoot.transform.rotation = Quaternion.Slerp(GunRoot.transform.rotation, rotation, Time.deltaTime * RotationDamping);
        
        //GunSwivle
        //GunSwivle.transform.rotation = rightHand.transform.rotation;

        float averageY = (rightHand.position.y + leftHand.position.y) / 2;

        averageY -= 1;
        float ControllerDistance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
        if (Vector3.Distance(HeadSet.transform.position, rightHand.transform.position) > Vector3.Distance(HeadSet.transform.position, leftHand.transform.position))
        {
            ControllerDistance *= -1f;
            //HorzMapping.Evaluate(ControllerDistance);
        }
        else
        {
            //HorzMapping.Evaluate(ControllerDistance);
        }

        GunSwivle.transform.eulerAngles = new Vector3(VertMapping.Evaluate(averageY), MiddleCube.transform.eulerAngles.y -90f, GunSwivle.transform.localRotation.z);
        
        //Debug.Log(ControllerDistance);

        //UpdateHands();

        SteamVR_Controller.Device deviceL = SteamVR_Controller.Input((int)trackedObjL.index);
        if(deviceL.GetTouch(SteamVR_Controller.ButtonMask.Trigger) && LeftFireFlag == false)
        {
            LeftFireFlag = true;
            StartCoroutine(FireLeftCannon());
        }

        SteamVR_Controller.Device deviceR = SteamVR_Controller.Input((int)trackedObjR.index);
        if (deviceR.GetTouch(SteamVR_Controller.ButtonMask.Trigger) && RightFireFlag == false)
        {
            RightFireFlag = true;
            StartCoroutine(FireRightCannon());
        }
        
        // Get velocity of chair for movement SFX
        Debug.Log("Chair spin velocity is: " + (deviceR.velocity.magnitude * deviceL.velocity.magnitude));
        Fabric.EventManager.Instance.SetParameter("SFX/Gun/Roller", "Velocity", (deviceR.velocity.magnitude * deviceL.velocity.magnitude), gameObject);

    }

    public void UpdateHands()
    {
        const float gripBreakDist = 0.25f;
        Vector3 leftPointToHand = leftHand.transform.position - leftGrabPoint.transform.position;
        Vector3 rightPointToHand = rightHand.transform.position - rightGrabPoint.transform.position;
        Vector3 swivelToLeftHand = leftHand.transform.position - GunSwivle.transform.position;
        Vector3 swivelToRightHand = rightHand.transform.position - GunSwivle.transform.position;
        Vector3 swivelToCenter = (swivelToLeftHand + swivelToRightHand) / 2f;
        Vector3 swivelToRightGrab = rightGrabPoint.transform.position - GunSwivle.transform.position;
        Vector3 swivelToLeftGrab = leftGrabPoint.transform.position - GunSwivle.transform.position;
        Vector3 swivelToGrabCenter = (swivelToLeftGrab + swivelToRightGrab) / 2f;

        Vector3 dragVector = Vector3.zero;
        if(leftPointToHand.magnitude <= gripBreakDist)
        {
            dragVector += leftPointToHand;
        }
        if (rightPointToHand.magnitude <= 0.1f)
        {
            dragVector += rightPointToHand;
        }

        Vector3 newTargetCenter = (leftGrabPoint.transform.position + rightGrabPoint.transform.position) / 2f + dragVector;

        Vector3 lookRot = newTargetCenter - GunBase.transform.position;
        lookRot.y = 0f;
        GunBase.transform.rotation = Quaternion.LookRotation(lookRot);

        float angle = Vector3.Angle(swivelToCenter, swivelToGrabCenter);

        GunSwivle.transform.rotation = Quaternion.LookRotation(-swivelToCenter);
    }

    public IEnumerator FireLeftCannon()
    {
        Fabric.EventManager.Instance.PostEvent("SFX/Gun/Laser", leftCannonObj);

        SteamVR_Controller.Device deviceL = SteamVR_Controller.Input((int)trackedObjL.index);
        deviceL.TriggerHapticPulse(3000);
        transform.GetComponent<Animator>().SetBool("New Bool",true);
        yield return new WaitForSeconds(.25f);
        transform.GetComponent<Animator>().SetBool("New Bool", false);

        //FIRELASER
        FireLaser(LCannonPoint);
        LeftFireFlag = false;
    }

    public IEnumerator FireRightCannon()
    {
        Fabric.EventManager.Instance.PostEvent("SFX/Gun/Laser", rightCannonObj);

        SteamVR_Controller.Device deviceR = SteamVR_Controller.Input((int)trackedObjR.index);
        deviceR.TriggerHapticPulse(3000);
        transform.GetComponent<Animator>().SetBool("New Bool 0", true);
        yield return new WaitForSeconds(.25f);
        transform.GetComponent<Animator>().SetBool("New Bool 0", false);

        //FIRELASER
        FireLaser(RCannonPoint);
        RightFireFlag = false;
    }

    public void FireLaser (Transform CannonSide)
    {
        GameObject newLaser = Instantiate(LaserPrefab,CannonSide.position, Quaternion.identity) as GameObject;
        //GameObject newSmoke = Instantiate(SmokePartPrefab, CannonSide.position, Quaternion.identity) as GameObject;
        //newSmoke.transform.parent = CannonSide;

        newLaser.transform.rotation = CannonSide.rotation;
        LaserScript Ls = newLaser.AddComponent<LaserScript>();
        Ls.LaserSpeed = LaserSpeed;
        Ls.LaserLife = LaserLife;
    }
}