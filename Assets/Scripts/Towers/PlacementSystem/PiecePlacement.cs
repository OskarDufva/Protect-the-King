using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePlacement : MonoBehaviour
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

    public void SpawnTower(bool SpawnKing)
    {
        if(SpawnKing == false)
        {
            if (_gameManager._CurrentHoveredTile == null)
            {
                return;
            }
            if (_gameManager._CurrentHoveredTile.OccupiedTile || _gameManager._CurrentHoveredTile.EnemyPathTile)
            {
                print("not valid spot");
                return;
            }

            GameObject temp = Instantiate(_gameObject);
            _gameManager._CurrentHoveredTile.OccupiedTile = true;
            temp.transform.position = _gameManager._CurrentHoveredTile.transform.position;
        }

        if (SpawnKing == true)
        {

            if (_gameManager._CurrentHoveredTile == null)
            {
                return;
            }
            if(_gameManager._CurrentHoveredTile.EnemyPathTile == false)
            {
                return;
            }

            GameObject temp = Instantiate(_gameObject);
            _gameManager._CurrentHoveredTile.OccupiedTile = true;
            _gameManager.ChangePhases(Phases.PreparationPhase);
            temp.transform.position = _gameManager._CurrentHoveredTile.transform.position;
        }

    }
}
