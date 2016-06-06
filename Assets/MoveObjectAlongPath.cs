using UnityEngine;
using System.Collections;

public class MoveObjectAlongPath : MonoBehaviour {

    
    public Transform[] waypointArray;
    public float percentsPerSecond = 0.02f; // %2 of the path moved per second
    public float currentPathPercent = 0.0f; //min 0, max 1

    void Update()
    {
        currentPathPercent += percentsPerSecond * Time.deltaTime;
        iTween.PutOnPath(gameObject, waypointArray, currentPathPercent);
        if (currentPathPercent > 1)
        {
            currentPathPercent = 0;
        }
    }

    void OnDrawGizmos()
    {
        //Visual. Not used in movement
        iTween.DrawPath(waypointArray);
    }
}
