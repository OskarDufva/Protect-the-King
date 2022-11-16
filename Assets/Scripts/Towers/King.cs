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
    public Vector2Int Position;

    private bool isDead;
    private GameManager _gameManager;


    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        txt_HealthCount.SetText("Health: " + _health.ToString());
        _health = _maxHealth;
    }

    //the king will take damge based on the value you give the function when calling it
    void TakeDamage(int damage)
    {

        _health -= damage;

        //Color when taking damge

        txt_HealthCount.SetText("Health: " + _health.ToString());
        if (_health <= 0 && !isDead)
        {
            isDead = true;
            _gameManager.GameOver();
            Destroy(gameObject);
        }
    }

    //when a piece overlaps with the king that holds the enemystats script it will deal damage to the king and kill the enemy
    private void OnTriggerEnter(Collider other)
    {
        EnemyStats enemyStats = other.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            TakeDamage(enemyStats.Damage);
            _gameManager.Tiles[Position.x].Tiles[Position.y].DealDamage(enemyStats.startHealth);
            enemyStats.TakeDamage(enemyStats.startHealth);            
        }        
    }
}
