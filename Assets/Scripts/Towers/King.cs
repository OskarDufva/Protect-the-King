using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class King : MonoBehaviour
{
    public int _health;
    public int _maxHealth;
    public TextMeshProUGUI txt_HealthCount;

    private bool isDead;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        txt_HealthCount.SetText("Health: " + _health.ToString());
        _health = _maxHealth;
    }

    void TakeDamage(int damage)
    {

        _health -= damage;
        txt_HealthCount.SetText("Health: " + _health.ToString());
        if (_health <= 0 && !isDead)
        {
            isDead = true;
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
            enemyStats.TakeDamage(enemyStats.startHealth);            
        }        
    }
}
