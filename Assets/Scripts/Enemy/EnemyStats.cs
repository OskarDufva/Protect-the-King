using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private Wavemanager _wavemanager;
    [SerializeField] private SpriteRenderer _spriterender;

    public float Health;
    public int Damage;
    public int Cost;

    private void Start()
    {
        _wavemanager = FindObjectOfType<Wavemanager>();
        print(_spriterender);
        if (_wavemanager == null)
        {
            print("no wavemanager found!");
            Destroy(gameObject);
        }
    }


    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            _wavemanager.EnemyDeath();
            Destroy(gameObject);
        }
        _spriterender.color = Color.red;
        
    }
}
