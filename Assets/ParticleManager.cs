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
}
