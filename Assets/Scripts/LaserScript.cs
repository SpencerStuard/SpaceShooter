using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

    public float LaserSpeed;
    public float LaserLife;

    // Use this for initialization
    void Start () {
        Invoke("DestroyMe", LaserLife);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0, 0, -LaserSpeed * Time.deltaTime), Space.Self);
	}

    void DestroyMe()
    {
        Destroy(gameObject);
    }
    
}
