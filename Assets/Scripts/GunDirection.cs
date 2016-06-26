using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunDirection : MonoBehaviour
{
	public bool MouseKeyboardControls;
    public float MouseGunHeight;
    public Vector3 MouseCameraOffset;

    //LASER STATS
    public float LaserSpeed;
    public float LaserLife;
	public float FireRate;

    //public AnimationClip LeftAnim;
    public AnimationCurve VertMapping;
    public AnimationCurve HorzMapping;

    public GameObject HeadSet;
    public GameObject CameraObject;
    

    public float RotationDamping = 1f;

    public Transform leftHand;
    public Transform rightHand;

    private Vector3 midpoint;

    private bool LeftFireFlag;
    private bool RightFireFlag;

    //public Rigidbody GunBase;
    public GameObject GunSwivle;
    //public GameObject GunRoot;

    public GameObject leftCannonObj;
    public GameObject rightCannonObj;

    //public SteamVR_TrackedObject trackedObjR;
    public SteamVR_TrackedObject trackedObjL;
    public SteamVR_TrackedObject trackedObjR;

    public Transform LCannonPoint;
    public Transform RCannonPoint;
    public GameObject LaserPrefab;
    public GameObject SmokePartPrefab;

	Vector3 LastMousePosition;
	Vector3 MouseDelta;

    public GameObject MiddleCube;


    void Awake()
    {
        if (!MouseKeyboardControls)
        {
            trackedObjR = leftHand.GetComponent<SteamVR_TrackedObject>();
            trackedObjL = rightHand.GetComponent<SteamVR_TrackedObject>();
        }
    }

    // Use this for initialization
    void Start()
    {
        //Fabric.EventManager.Instance.PostEvent("SFX/Gun/Roller", gameObject);
        FMOD_AudioManager.Instance.SFX_Gun_Roller.Play();
		if (MouseKeyboardControls) {
			SetUpMouseKeyBoardCamera ();
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (MouseKeyboardControls) {

            ///ROTATE GUN FOR MOUSE AND KEYBOARD CONTROLS
            if(LastMousePosition != Vector3.zero)
            {
                MouseDelta = LastMousePosition - Input.mousePosition;
            }
			float NewXRotation = GunSwivle.transform.eulerAngles.x + MouseDelta.y;
			GunSwivle.transform.eulerAngles = new Vector3 (NewXRotation,GunSwivle.transform.eulerAngles.y - MouseDelta.x , GunSwivle.transform.eulerAngles.z);
            HeadSet.transform.eulerAngles = new Vector3(HeadSet.transform.eulerAngles.x, GunSwivle.transform.eulerAngles.y, HeadSet.transform.eulerAngles.z);
            HeadSet.transform.localPosition = HeadSet.transform.TransformDirection(MouseCameraOffset); ;
            LastMousePosition = Input.mousePosition;
		} else {
			midpoint = leftHand.position + (rightHand.position - leftHand.position) / 2;
			MiddleCube.transform.position = midpoint;
			MiddleCube.transform.LookAt (rightHand.transform, Vector3.right);

            //ROTATE BASE LEFT AND RIGHT
            Vector3 lookPos = midpoint - transform.position;
			lookPos = new Vector3 (lookPos.x, 0, lookPos.z);
			Quaternion rotation = Quaternion.LookRotation (lookPos);

            //ROTATE GUN UP AND DOWN
			float averageY = (rightHand.position.y + leftHand.position.y) / 2;
			averageY -= 1;

            ///ROTATE GUN AND BASE
            //GunRoot.transform.rotation = Quaternion.Slerp(GunRoot.transform.rotation, rotation, Time.deltaTime * RotationDamping);
            GunSwivle.transform.eulerAngles = new Vector3 (VertMapping.Evaluate (averageY), MiddleCube.transform.eulerAngles.y - 90f, GunSwivle.transform.localRotation.z);
            GunSwivle.transform.position = MiddleCube.transform.position;
            // Get velocity of chair for movement SFX
            SteamVR_Controller.Device deviceL = SteamVR_Controller.Input((int)trackedObjL.index);
            SteamVR_Controller.Device deviceR = SteamVR_Controller.Input((int)trackedObjR.index);
            FMOD_AudioManager.Instance.SFX_Gun_Roller.SetParameter("Velocity", (deviceR.velocity.magnitude * deviceL.velocity.magnitude));
		}

        FireGuns();
    }

	void FireGuns ()
	{
        if (!MouseKeyboardControls)
        {
            SteamVR_Controller.Device deviceL = SteamVR_Controller.Input((int)trackedObjL.index);
            if (deviceL.GetTouch(SteamVR_Controller.ButtonMask.Trigger) && LeftFireFlag == false)
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
        }
        else
        {
            if (Input.GetMouseButton(0) && RightFireFlag == false)
            {
                RightFireFlag = true;
                StartCoroutine(FireLeftCannon());
            }
            if (Input.GetMouseButton(1) && LeftFireFlag == false)
            {
                LeftFireFlag = true;
                StartCoroutine(FireRightCannon());
            }
        }
	}

	void SetUpMouseKeyBoardCamera ()
	{
        //SET GUN HEIGHT POSITION
        GunSwivle.transform.position = Vector3.zero + (Vector3.up * MouseGunHeight);

        ///TAKE OFF THE TRACKING SCRIPT AND PARENT IT WITH OFFSET TO GUN
        if(HeadSet.GetComponent<SteamVR_TrackedObject>())
        {
            HeadSet.GetComponent<SteamVR_TrackedObject>().enabled = false;
        }
        HeadSet.transform.position = GunSwivle.transform.position + MouseCameraOffset; //TODO SET TO AN OFFSET PUBLIC VAR
        //HeadSet.transform.parent = GunSwivle.transform;
        CameraObject.GetComponent<Camera>().fieldOfView = 130f;
	}

    public IEnumerator FireLeftCannon()
    {
		FireLaser(RCannonPoint);
        //Fabric.EventManager.Instance.PostEvent("SFX/Gun/Laser", leftCannonObj);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX_Gun_Laser", leftCannonObj.transform.position);
		if (!MouseKeyboardControls) {
			SteamVR_Controller.Device deviceL = SteamVR_Controller.Input ((int)trackedObjL.index);
			deviceL.TriggerHapticPulse (3000);
		}
        transform.GetComponent<Animator>().SetBool("New Bool 0",true);
        yield return null;
        transform.GetComponent<Animator>().SetBool("New Bool 0", false);

        //FIRELASER
		yield return new WaitForSeconds(FireRate);
        LeftFireFlag = false;
    }

    public IEnumerator FireRightCannon()
    {
		FireLaser(LCannonPoint);
        //Fabric.EventManager.Instance.PostEvent("SFX/Gun/Laser", rightCannonObj);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX_Gun_Laser", rightCannonObj.transform.position);
		if (!MouseKeyboardControls) {
			SteamVR_Controller.Device deviceR = SteamVR_Controller.Input ((int)trackedObjR.index);
			deviceR.TriggerHapticPulse (3000);
		}
        transform.GetComponent<Animator>().SetBool("New Bool", true);
		yield return null;
        transform.GetComponent<Animator>().SetBool("New Bool", false);

        //FIRELASER
		yield return new WaitForSeconds(FireRate);
        RightFireFlag = false;
    }

    public void FireLaser (Transform CannonSide)
    {
        GameObject newLaser = Instantiate(LaserPrefab,CannonSide.position, Quaternion.identity) as GameObject;
        //TODO particle

        newLaser.transform.rotation = CannonSide.rotation;
        LaserScript Ls = newLaser.AddComponent<LaserScript>();
        Ls.LaserSpeed = LaserSpeed;
        Ls.LaserLife = LaserLife;
    }
}