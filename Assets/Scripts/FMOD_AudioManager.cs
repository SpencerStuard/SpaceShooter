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

    public bool muteAudio;

    [Header("Music")]
    public FMODUnity.StudioEventEmitter MUS_Battle;
    [Header("Sound Effects")]
    public FMODUnity.StudioEventEmitter SFX_Explosion;
    public FMODUnity.StudioEventEmitter SFX_Gun_Laser;
    public FMODUnity.StudioEventEmitter SFX_Gun_Roller;
    public FMODUnity.StudioEventEmitter SFX_Ship_Rattle;
    [Header("Ambience")]
    public FMODUnity.StudioEventEmitter AMB_Ship;


    void Awake ()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start () 
    {
	    if (muteAudio)
        {
            FMOD.Studio.Bus masterBus;

            FMODUnity.RuntimeManager.StudioSystem.getBus("bus:/", out masterBus);

            masterBus.setMute(true);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {

	}
}
