using UnityEngine;
using System.Collections;

[System.Serializable]
public class WaveParameters {

    [Range(0f, 10f)]
    public float MinSpawnTime;
    [Range(0f, 10f)]
    public float MaxSpawnTime;

    public int NumberOfEnemies;

    //0-1
    [Range(0f,1f)]
    public float Enemy01Weight;
    [Range(0f, 1f)]
    public float Enemy02Weight;
    [Range(0f, 1f)]
    public float Enemy03Weight;
    [Range(0f, 1f)]
    public float Enemy04Weight;
    [Range(0f, 1f)]
    public float Enemy05Weight;

    [Range(0f, 1f)]
    public float EnemyAccuracy;
    [Range(0f, 1f)]
    public float EnemyFireRate;
    [Range(1f, 2f)]
    public float EnemyMissileFireRate;
    [Range(0f, 1f)]
    public float EnemyAimTime;

}
