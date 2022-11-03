using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class King : MonoBehaviour
{
    public float _health;
    public float _maxHealth;
    public KingHealth healthBar;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    void TakeDamage(float damage)
    {

        _health -= damage;
        healthBar.UpdateHealthBar();
        if (_health <= 0)
        {
            _gameManager.GameOver();
            Destroy(gameObject);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyStats enemyStats = other.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            print(enemyStats);
            TakeDamage(enemyStats.Damage);
            enemyStats.TakeDamage(enemyStats.Health);            
        }        
    }
}
