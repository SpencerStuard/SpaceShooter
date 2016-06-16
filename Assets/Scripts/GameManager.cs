using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;

	enum GameState{Start, Prewave, PostWave, InWave}; 
	GameState CurrentGameStates = GameState.Start;

    public int WaveNumber;

    //PlayerTrans
    public Transform PlayerTrans;

    //UI GameObjects
    public Text MainTextObject;
    public GameObject PlayButtonObject;

    //ENEMY AND WAVE INFO
    public int DebugStartingWave;
    int CurrentWaveNumber = 0;
	//public GameObject EnemySpawner;

	//UI INFO
	public GameObject UIParent;

    public bool InfitineHealth = false;

	public static GameManager Instance
	{
		get
		{
			if(_instance == null)
			{
				//_instance = new GameObject("GameManager");
				//_instance.AddComponent<GameManager>();
			}

			return _instance;
		}
	}


	void Awake()
	{
		_instance = this;
	}


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

	public void StartGame ()
	{
        ShipHealth.Instance.RestartGame();
        if(DebugStartingWave != 0)
        {
            WaveNumber = DebugStartingWave;
        }
        else
        {
            WaveNumber = 0;
        }
        
        StartCoroutine("StartWaveUI");
        //Fabric.EventManager.Instance.PostEvent("MUS/Timeline", GameManager.Instance.gameObject);
        FMOD_AudioManager.Instance.MUS_Battle.Play();
	}

    IEnumerator StartWaveUI()
    {
        MainTextObject.gameObject.SetActive(true);
        MainTextObject.text = "WAVE  " + (WaveNumber + 1).ToString() + " . . . 3";
        yield return new WaitForSeconds(1.4f);
        MainTextObject.text = "WAVE  " + (WaveNumber + 1).ToString() + " . . . 2";
        yield return new WaitForSeconds(1.4f);
        MainTextObject.text = "WAVE  " + (WaveNumber + 1).ToString() + " . . . 1";
        yield return new WaitForSeconds(1.4f);
        MainTextObject.text = "GOOD LUCK!";
        yield return new WaitForSeconds(2f);
        MainTextObject.gameObject.SetActive(false);
        SpawnManager._instance.SetUpWave(WaveNumber);
    }

    public void StartNextWaveUI(int WaveNumber)
    {
        StartCoroutine("NextWaveUI", WaveNumber);
    }

    IEnumerator NextWaveUI(int WaveNumber)
    {
        //Play wave success SFX
        FMOD_AudioManager.Instance.MUS_WinStinger.Play();

        CurrentWaveNumber = WaveNumber;
        MainTextObject.gameObject.SetActive(true);
        MainTextObject.text = "WAVE  " + WaveNumber.ToString() + " . . . 3";
        yield return new WaitForSeconds(1.4f);
        MainTextObject.text = "WAVE  " + WaveNumber.ToString() + " . . . 2";
        yield return new WaitForSeconds(1.4f);
        MainTextObject.text = "WAVE  " + WaveNumber.ToString() + " . . . 1";
        yield return new WaitForSeconds(1.4f);
        MainTextObject.text = "GOOD LUCK!";
        yield return new WaitForSeconds(2f);
        MainTextObject.gameObject.SetActive(false);
        SpawnManager._instance.SetUpWave(WaveNumber);
    }


    public void RestartGame()
    {

    }

    public void PlayerDied ()
    {
        //Kill all current enemies and stop spawning new ones
        SpawnManager._instance.PlayerDied();

        //SetUp End Game UI
        StartCoroutine("StartEndGameUI");

        //Play Lose Music
        FMOD_AudioManager.Instance.MUS_Battle.Stop();
    }

    IEnumerator StartEndGameUI ()
    {
        MainTextObject.gameObject.SetActive(true);
        MainTextObject.text = "GAME OVER / SCORE = 87576";
        yield return new WaitForSeconds(3f);
        MainTextObject.text = "START NEW GAME";
        PlayButtonObject.SetActive(true);


    }
}
