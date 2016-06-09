using UnityEngine;
using System.Collections;

public class PlayButtonScr : MonoBehaviour {

	void OnTriggerEnter (Collider C)
	{
		if(C.transform.tag == "PlayerLaser")
		{
			GameManager.Instance.StartGame();
		}

        gameObject.SetActive(false);
	}

    
}
