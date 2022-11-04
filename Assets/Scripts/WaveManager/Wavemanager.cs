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
   
    CurrencySystem _currencySystem;
    private int _goldThisWave;
    private float Boost;


    private void Start()
    {
        if (_currencySystem == null)
        {
            _currencySystem = FindObjectOfType<CurrencySystem>();
        }

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
            _currencySystem.ChangeGold(_goldThisWave);
            GameManager.ChangePhases(Phases.PreparationPhase);
            GameManager.StartWaveButton.transform.gameObject.SetActive(true);
        }
    }

    public void SpawnWave(int waveCount)
    {
            var x = WaveSpawn(_waveList[waveCount]);
            EnemyCounter(_waveList[waveCount]);
            _goldThisWave = _waveList[waveCount].GoldGain;
            Boost = _waveList[waveCount].EnemyBoost;
            StartCoroutine(x);
    }

    IEnumerator WaveSpawn(WaveList EnemyData)
    {
        GameObject Enemy;
        EnemyStats stats;
        for (int i = 0; i < EnemyData.Enemies.Length; i++)
        {
            for (int g = 0; g < EnemyData.Enemies[i].AmountOfEnemies; g++)
            {
                yield return new WaitForSeconds(EnemyData.Enemies[i].SpawnDelay);
                Enemy = Instantiate(EnemyData.Enemies[i].Enemy, SpawnPoint.transform.position, Quaternion.identity);
                stats = Enemy.GetComponent<EnemyStats>();
                stats.Health *= Boost;
            }
            yield return new WaitForSeconds(EnemyData.Enemies[i].BreakTime);
            
        }
    }
}