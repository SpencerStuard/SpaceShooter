using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject target;                // The prefab to be spawned.
	public float spawnTime = 10f;            // How long between each spawn.
	private Vector3 spawnPosition;

	// Use this for initialization
	void Start () 
	{
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		int spawncount = 0;
		if(spawncount < 10){
			InvokeRepeating ("Spawn", spawnTime, spawnTime);
			spawncount++;
			spawnTime--;
		}

	}

	void Spawn ()
	{
		spawnPosition.x = Random.Range (-17, 17);
		spawnPosition.y = Random.Range (0, 10);
		spawnPosition.z = Random.Range (-17, 17);

		GameObject o = Instantiate(target, spawnPosition, Quaternion.identity) as GameObject;
		o.transform.parent = transform;
	}
}
