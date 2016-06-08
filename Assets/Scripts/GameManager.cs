using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;

	enum GameState{Start, Prewave, PostWave, InWave}; 

	GameState CurrentGameStates = GameState.Start;

	//ENEMY AND WAVE INFO
	int CurrentWaveNumber = 0;
	public GameObject EnemySpawner;

	//UI INFO
	public GameObject UIParent;

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
		//EnemySpawner.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame ()
	{
		CurrentWaveNumber = 0;
		CurrentGameStates = GameState.InWave;
		//EnemySpawner.SetActive(true);
		UIParent.SetActive(false);
	}
}
