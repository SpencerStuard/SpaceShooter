using UnityEngine;
using System.Collections;

public class KillMeAfterSeconds : MonoBehaviour {

    public float killSeconds;

	// Use this for initialization
	void Start () {
        if (killSeconds != 0)
        {
            KillAfterSeconds(killSeconds);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void KillAfterSeconds(float killTime)
    {
        Invoke("DestroyMe", killTime);
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
