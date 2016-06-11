using UnityEngine;
using System.Collections;

public class FMOD_AudioManager : MonoBehaviour {

    private static FMOD_AudioManager _instance;

    public static FMOD_AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                //_instance = new GameObject("FMOD_AudioManager");
                //_instance.AddComponent<FMOD_AudioManager>();
            }

            return _instance;
        }
    }

    [Header("Events")]
    public FMODUnity.StudioEventEmitter MUS_Battle;
    public FMODUnity.StudioEventEmitter SFX_Explosion;
    public FMODUnity.StudioEventEmitter SFX_Gun_Laser;
    public FMODUnity.StudioEventEmitter SFX_Gun_Roller;

    void Awake ()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {

	}
}
