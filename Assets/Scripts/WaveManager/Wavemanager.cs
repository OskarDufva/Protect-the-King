using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavemanager : MonoBehaviour
{
    public Transform SpawnPoint;
    public GameManager GameManager;
    public int EnemiesAlive = 0;
    public WaveList[] _waveList;
    public bool ReadyForNewWave = false;
   
    private CurrencySystem _currencySystem;
    private int _goldThisWave;
    private float Boost;
    private int _enemiesInWave;
    private int _enemiesSpawned;
    private int _totalWaveCount;
    private int _waveCount;
    private float timer = 0.0f;

    //runs code when the game starts
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

        _totalWaveCount = WaveAmountCounter();
    }

    private void Update()
    {
        if (GameManager.WaveIsActive)
        {
            timer += Time.deltaTime;
        }

        if (timer > 1)
        {
            int enemy = FindObjectsOfType<EnemyStats>().Length;
            if (enemy <= 0 && _enemiesInWave == _enemiesSpawned)
            {
                print("new wave is ready");
                NextWave();
            }
            timer = 0;
        }
    }

    //sets up for the next wave and checks victory
    private void NextWave()
    {
        if (GameManager._currentWave == _totalWaveCount)
        {
            Destroy(FindObjectOfType<King>().gameObject);
            GameManager.Victory();
            print("victory");
            return;
        }
        _currencySystem.ChangeGold(_goldThisWave);
        GameManager.ChangePhases(Phases.PreparationPhase);
        GameManager.StartWaveButton.transform.gameObject.SetActive(true);
        GameManager.WaveIsActive = false;
    }

    //counts how many waves are in this level
    public int WaveAmountCounter()
    {
        return _waveList.Length;
    }

    //starts the spawning of enemies
    public void SpawnWave(int waveCount)
    {
        var x = WaveSpawn(_waveList[waveCount]);
        _goldThisWave = Mathf.CeilToInt(_waveList[waveCount].GoldGain * GameManager.GoldBoost);
        Boost = _waveList[waveCount].EnemyBoost;
        StartCoroutine(x);
        _enemiesInWave = 0;
        for (int i = 0; i < _waveList[waveCount].Enemies.Length; i++)
        {
            for (int y = 0; y < _waveList[waveCount].Enemies[i].AmountOfEnemies; y++)
            {
                _enemiesInWave++;
            }
        }
    }

    //Handles the spawning of enemies
    IEnumerator WaveSpawn(WaveList EnemyData)
    {
        GameObject Enemy;
        EnemyStats stats;
        _enemiesSpawned = 0;
        for (int i = 0; i < EnemyData.Enemies.Length; i++)
        {
            for (int g = 0; g < EnemyData.Enemies[i].AmountOfEnemies; g++)
            {
                yield return new WaitForSeconds(EnemyData.Enemies[i].SpawnDelay);
                Enemy = Instantiate(EnemyData.Enemies[i].Enemy, SpawnPoint.transform.position, Quaternion.identity);
                stats = Enemy.GetComponent<EnemyStats>();
                stats.startHealth *= Boost;
                _enemiesSpawned++;
            }
            yield return new WaitForSeconds(EnemyData.Enemies[i].BreakTime);
            
        }
    }
}