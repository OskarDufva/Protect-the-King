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
        King king = other.GetComponent<King>();
        if (king != null)
        {
            print("hit the king");
            
        }
    }

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
            _wavemanager.EnemyDeath();
        }
    }
    private void OnDestroy()
    {
        _currencySystem.ChangeGold(GoldGained);
    }
}
