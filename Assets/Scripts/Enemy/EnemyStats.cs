using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private Wavemanager _wavemanager;

    public float startHealth;
    public float health;
    public int Damage;
    public int GoldGained;

    public Image healthBar;


    private CurrencySystem _currencySystem;
    private bool _touchKing;

    private void Start()
    {
        _wavemanager = FindObjectOfType<Wavemanager>();

        _currencySystem = FindObjectOfType<CurrencySystem>();

        health = startHealth;

        if (_wavemanager == null)
        {
            print("no wavemanager found!");
            Destroy(gameObject);
        }
    }


    //when enemy takes damage removes health from the enemy and destory it when reaches 0
    public void TakeDamage(float damage)
    {
        health -= damage;

        healthBar.fillAmount = health / startHealth;
        //if (_touchKing == true)
        //{
        //    _wavemanager.EnemyDeath();
        //    Destroy(gameObject);
        //}
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    //runs code when destroyed addes gold to the currency manager
    private void OnDestroy()
    {
        _currencySystem.ChangeGold(GoldGained);
    }
}
