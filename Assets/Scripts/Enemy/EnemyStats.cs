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

    private void OnTriggerEnter(Collider other)
    {
        if (Collider.FindObjectOfType<King>())
        {
            _touchKing = true;
        }
    }


    public void TakeDamage(float damage)
    {
        health -= damage;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && _touchKing == true)
        {
            _wavemanager.EnemyDeath();
            Destroy(gameObject);
        }
        else if (health <= 0)
        {
            _wavemanager.EnemyDeath();
            _currencySystem.ChangeGold(GoldGained);
            Destroy(gameObject);
        }
    }
}
