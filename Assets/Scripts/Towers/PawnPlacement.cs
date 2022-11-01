using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPlacement : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        
    }

    public void SpawnTower()
    {
        if(_gameManager._CurrentHoveredTile == null)
        {
            return;
        }
        if(_gameManager._CurrentHoveredTile.OccupiedTile || _gameManager._CurrentHoveredTile.EnemyPathTile)
        {
            return;
        }

        GameObject temp = Instantiate(_gameObject);
        _gameManager._CurrentHoveredTile.OccupiedTile = true;
        temp.transform.position = _gameManager._CurrentHoveredTile.transform.position;

    }
}
