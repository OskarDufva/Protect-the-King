using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavemanager : MonoBehaviour
{
    public Transform SpawnPoint;
    public GameManager GameManager;
    public int EnemiesAlive = 0;
    public WaveList[] _waveList;

    private void Start()
    {
        //GameManager = FindObjectOfType<GameManager>();
    }

    public int WaveAmountCounter()
    {
        return _waveList.Length;
    }
    //counts every enemy we spawn to keep track of the spanwed enemies.
    public void EnemyCounter()
    {
        EnemiesAlive++;
    }
    //Call this everytime an enemy dies to reduce the counter so we know when all enemies have died
    public void EnemyDeath()
    {
        EnemiesAlive--;
    }

    public void SpawnWave(int waveCount)
    {
        var x = WaveSpawn(_waveList[waveCount]);
        StartCoroutine(x);
    }

    IEnumerator WaveSpawn(WaveList EnemyData)
    {
        for (int i = 0; i < EnemyData.Enemies.Length; i++)
        {
            for (int g = 0; g < EnemyData.Enemies[i].AmountOfEnemies; g++)
            {
                yield return new WaitForSeconds(EnemyData.Enemies[i].SpawnDelay);
                Instantiate(EnemyData.Enemies[i].Enemy, SpawnPoint.transform.position, Quaternion.identity);
                EnemyCounter();
            }
            yield return new WaitForSeconds(EnemyData.Enemies[i].BreakTime);
            
        }
    }
}