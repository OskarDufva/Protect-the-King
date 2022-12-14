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

    //Spawns the tower depending what bools are active and take money when spawned
    public void SpawnTower(bool SpawnKing,int cost,bool SpawnUpgrade)
    {
        if (SpawnKing == false && SpawnUpgrade == false)
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
                _gameManager._CurrentHoveredTile.Tower = temp;
            }
            else
            {
                print("Not enough money!");
            }
        }

        if(SpawnUpgrade == true && _gameManager._CurrentHoveredTile != null)
        {
            if (_gameManager._CurrentHoveredTile.Tower == null)
            {
                return;
            }
            Pawn pawn = _gameManager._CurrentHoveredTile.Tower.GetComponent<Pawn>();
            print(pawn);
            if (pawn == null)
            {
                return;
            }
            if (currencySystem.Gold >= Mathf.Abs(cost)) 
            { 
                currencySystem.ChangeGold(cost);
                GameObject temp = Instantiate(_gameObject);
                temp.transform.position = _gameManager._CurrentHoveredTile.transform.position;
                Destroy(_gameManager._CurrentHoveredTile.Tower);
                _gameManager._CurrentHoveredTile.Tower = temp;
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
            if(_gameManager._CurrentHoveredTile.KingSpot == false)
            {
                return;
            }

            GameObject temp = Instantiate(_gameObject);
            _gameManager._CurrentHoveredTile.OccupiedTile = true;
            _gameManager.ChangePhases(Phases.PreparationPhase);
            _gameManager._CurrentHoveredTile.Tower = temp;
            temp.GetComponent<King>().Position = _gameManager._CurrentHoveredTile.Index;
            temp.transform.position = _gameManager._CurrentHoveredTile.transform.position;
            _gameManager.GoldBoost = _gameManager._CurrentHoveredTile._kingSpotBoost;
            transform.parent.gameObject.SetActive(false);
        }

    }
}
