using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private Wavemanager _wavemanager;

    public float startHealth;
    private float health;
    public int Damage;
    public int Cost;

    public Image healthBar;

    private void Start()
    {
        _wavemanager = FindObjectOfType<Wavemanager>();

        health = startHealth;

        if (_wavemanager == null)
        {
            print("no wavemanager found!");
            Destroy(gameObject);
        }
    }


    public void TakeDamage(float damage)
    {
        health -= damage;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            _wavemanager.EnemyDeath();

            Destroy(gameObject);
        }        
    }
}
