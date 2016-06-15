using UnityEngine;
using System.Collections;

public class AttachTo : MonoBehaviour {

    public GameObject objectToAttachTo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (objectToAttachTo != null)
        {
            gameObject.transform.position = objectToAttachTo.gameObject.transform.position;
        }
	}
}
