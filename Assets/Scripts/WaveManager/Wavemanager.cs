using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavemanager : MonoBehaviour
{
    public Transform SpawnPoint;
    public GameManager GameManager;
    public int EnemiesAlive = 0;
    public WaveList[] _waveList;

    public GameObject StartWaveButton;
    private bool isActive; 
    private void Start()
    {
        if(GameManager == null)
        {
            GameManager = FindObjectOfType<GameManager>();
        }
    }

    public int WaveAmountCounter()
    {
        return _waveList.Length;
    }
    //counts every enemy we spawn to keep track of the spanwed enemies.
    public void EnemyCounter(WaveList EnemyData)
    {
        for (int i = 0; i < EnemyData.Enemies.Length; i++)
        {
            for (int g = 0; g < EnemyData.Enemies[i].AmountOfEnemies; g++)
                EnemiesAlive++;
        }
    }
    //Call this everytime an enemy dies to reduce the counter so we know when all enemies have died
    public void EnemyDeath()
    {
        EnemiesAlive--;
        if (EnemiesAlive <= 0)
        {
            GameManager.ChangePhases(Phases.PreparationPhase);
            StartWaveButton.transform.gameObject.SetActive(true);
            isActive = true;
        }
    }

    public void SpawnWave(int waveCount)
    {
            var x = WaveSpawn(_waveList[waveCount]);
            EnemyCounter(_waveList[waveCount]);
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
            }
            yield return new WaitForSeconds(EnemyData.Enemies[i].BreakTime);
            
        }
    }
}