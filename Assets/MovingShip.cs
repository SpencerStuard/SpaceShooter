using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingShip : MonoBehaviour {

    public Transform[] waypointArray;
    public float percentsPerSecond = 0.02f; // %2 of the path moved per second
    public float currentPathPercent = 0.0f; //min 0, max 1
    public float speed;

    void Awake ()
    {
        
    }

    void Start ()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", waypointArray, "speed", speed));
        transform.position = waypointArray[0].position;

    }

    void Update()
    {
        //currentPathPercent += percentsPerSecond * Time.deltaTime;
        //iTween.PutOnPath(gameObject, waypointArray, currentPathPercent);
    }

    void OnDrawGizmos()
    {
        //Visual. Not used in movement
        //iTween.DrawPath(waypointArray);
    }
}
