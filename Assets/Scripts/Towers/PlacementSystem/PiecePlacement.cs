using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePlacement : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    
    private GameManager _gameManager;

    private CurrencySystem currencySystem;


    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        currencySystem = FindObjectOfType<CurrencySystem>();
    }

    private void Update()
    {
        
    }

    public void SpawnTower(bool SpawnKing,int cost)
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

            if (currencySystem.Gold >= Mathf.Abs(cost))
            {
                currencySystem.ChangeGold(cost);
                GameObject temp = Instantiate(_gameObject);
                _gameManager._CurrentHoveredTile.OccupiedTile = true;
                temp.transform.position = _gameManager._CurrentHoveredTile.transform.position;
            }
            else
            {
                print("Not enough money!");
            }
      
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