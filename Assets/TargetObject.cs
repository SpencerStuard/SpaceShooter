using UnityEngine;
using System.Collections;

public class TargetObject : MonoBehaviour {

	public GameObject explosion = null;
	public AudioSource explosionSFX = null;

	public int scoreAddition = 1;

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Do thing");

		if(other.gameObject.tag == "Laser")
		{
			DestroyObject(other.gameObject);

			GameObject.Instantiate (explosion as Object);
			explosionSFX.Play ();
			AddScore ();

			DestroyObject (gameObject);
		}
	}

	void AddScore (){
		//add score addition to player controller
	}
}
