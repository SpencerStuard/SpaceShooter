using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour {

    private static ParticleManager instance = null;

    // Game Instance Singleton
    public static ParticleManager _instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject ShildParticle;
    public GameObject ExplosionParticle;
    public GameObject WarpInEffect;
    public GameObject WarpLightEffect;


    void Awake ()
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
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnShildPart(Vector3 Location, GameObject LookDirection)
    {
        //Spawn and look at
        GameObject newPart = Instantiate(ShildParticle,Location,Quaternion.identity) as GameObject;
        newPart.transform.LookAt(LookDirection.transform);

        //Put on a kill script
        newPart.AddComponent<KillMeAfterSeconds>();
        newPart.GetComponent<KillMeAfterSeconds>().SetUpKillMe(2f);
    }

    public void SpawnExplosionParticle(Vector3 Location, GameObject LookDirection)
    {
        
        //Spawn and look at
        GameObject newPart = Instantiate(ExplosionParticle, Location, Quaternion.identity) as GameObject;
        newPart.transform.LookAt(LookDirection.transform);

        //Put on a kill script
        newPart.AddComponent<KillMeAfterSeconds>();
        newPart.GetComponent<KillMeAfterSeconds>().SetUpKillMe(2f);

    }

    public void SpawnWarpInEffect(Vector3 Location, Transform PlayerTrans)
    {

        //Spawn and look at
        GameObject newPart = Instantiate(WarpInEffect, Location, Quaternion.identity) as GameObject;
        newPart.transform.LookAt(2 * newPart.transform.position - PlayerTrans.position);
        newPart.transform.eulerAngles = new Vector3(0, newPart.transform.eulerAngles.y, newPart.transform.eulerAngles.z);

        //SPAWN WAPR LIGHT
        GameObject newWarpLight = Instantiate(WarpLightEffect, PlayerTrans.position, Quaternion.identity) as GameObject;
        newWarpLight.transform.LookAt(newPart.transform.position);
        newWarpLight.transform.localPosition = newWarpLight.transform.TransformDirection( new Vector3(0, 0, 2f));
        newWarpLight.transform.position = new Vector3(newWarpLight.transform.position.x, 1f, newWarpLight.transform.position.z); 
        newWarpLight.AddComponent<KillMeAfterSeconds>();
        newWarpLight.GetComponent<KillMeAfterSeconds>().SetUpKillMe(1.8f);

        //Put on a kill script
        newPart.AddComponent<KillMeAfterSeconds>();
        newPart.GetComponent<KillMeAfterSeconds>().SetUpKillMe(1.8f);

    }
}
