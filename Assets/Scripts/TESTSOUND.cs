using UnityEngine;
using System.Collections;
using FMOD.Studio;
using System.Collections.Generic;

public class TESTSOUND : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
        //FMOD_AudioManager.Instance.MUS_Battle.Play();
        
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space))
        {

            //FMOD_AudioManager.Instance.MUS_Battle.SetParameter("StringsToggle", 1.0f);
            //FMOD_AudioManager.Instance.SFX_Explosion.Play();
        }
	}
}