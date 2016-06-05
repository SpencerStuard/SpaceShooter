using UnityEngine;
using System.Collections;

public class PlayButtonScr : MonoBehaviour {

	void OnTriggerEnter (Collider C)
	{
		if(C.transform.tag == "Laser")
		{
			GameManager.Instance.StartGame();
		}
	}
}
