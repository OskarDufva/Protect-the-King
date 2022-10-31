using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveList
{
    public string WaveName;
    public EnemyData[] Enemies;
    //public float rate;
}

[System.Serializable]
public struct EnemyData
{
    public GameObject Enemy;
    public float SpawnDelay;
    public int AmountOfEnemies;
    public float BreakTime;
}
