using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	enum GameState{Start, Prewave, PostWave, InWave}; 

	GameState CurrentGameStates = GameState.Start;

	int CurrentWaveNumber = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StartGame ()
	{
		CurrentWaveNumber = 0;
		CurrentGameStates = GameState.InWave;
	}
}
