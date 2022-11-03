using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _highlightColor;
    [SerializeField] private Color _attackHighlightColor;
    [SerializeField] private Color _validPawnPlacementColor;
    [SerializeField] private Color _inValidPawnPlacementColor;
    [SerializeField] private bool KingSpot;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private List<EnemyStats> enemiesOnTile = new List<EnemyStats>();
    private GameManager _gameManager;
    
    public GameObject Tower;
    public GameObject TileTxt;
    public bool EnemyPathTile;
    [HideInInspector]
    public bool OccupiedTile;

    public Vector2Int Index;

    public bool Walked { get; set; }
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void ChangeColor(Color color)
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = color;
    }

    public void OriginalColor()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = _originalColor;
    }

    public void AttackColor()
    {
        if(_spriteRenderer == null)
        {
         _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = _attackHighlightColor;
    }

    public void ValidPlacementColor()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = _validPawnPlacementColor;
    }

    public void InvalidPlacementColor()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = _inValidPawnPlacementColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyStats enemyStats = other.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            enemiesOnTile.Add(enemyStats);
        }
        PiecePlacement pawn = other.GetComponent<PiecePlacement>();
        if (pawn != null)
        {
            _spriteRenderer.color = _highlightColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyStats enemyStats = other.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            enemiesOnTile.Remove(enemyStats);
        }
    }

    public void DealDamage(float damage)
    {
        for (int i = 0; i < enemiesOnTile.Count; i++)
        {
            if (enemiesOnTile[i] == null)
            {
                enemiesOnTile.RemoveRange(i, 1);
            }
        }
        
        if (enemiesOnTile.Count != 0) 
        {
            for (int i = 0; i < enemiesOnTile.Count; i++)
            {
                enemiesOnTile[i].TakeDamage(damage);
                if (enemiesOnTile[i].Health <= 0)
                {
                    enemiesOnTile.RemoveRange(i--, 1);
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        _spriteRenderer.color = _highlightColor;
        //TileTxt.SetActive(true);

        if(_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
            print(_gameManager + "Added");
        }
        _gameManager._CurrentHoveredTile = this;
        if(_gameManager._CurrentHoveredTile.Tower != null)
        {
            ShowAttackSpots();
        }
    }

    private void OnMouseExit()
    {
        _spriteRenderer.color = _originalColor;
        //TileTxt.SetActive(false);
        if (_gameManager._CurrentHoveredTile.Tower != null)
        {
            print("hide");
            HideAttackSpots();
        }
        _gameManager._CurrentHoveredTile = null;

    }

    private void ShowAttackSpots()
    {
        Pawn pawn = _gameManager._CurrentHoveredTile.Tower.GetComponent<Pawn>();
        if (pawn != null)
        {
            List<Vector2Int> tempList = new List<Vector2Int>();
            for (int i = 0; i < _gameManager._CurrentHoveredTile.Tower.GetComponent<Pawn>()._targetedTiles.Count; i++)
            {
                tempList.Add(_gameManager._CurrentHoveredTile.Tower.GetComponent<Pawn>()._targetedTiles[i]);
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                _gameManager.Tiles[tempList[i].x].Tiles[tempList[i].y].AttackColor();
            }
        }

        Knight knight = _gameManager._CurrentHoveredTile.Tower.GetComponent<Knight>();
        if (knight != null)
        {
            List<Vector2Int> tempList = new List<Vector2Int>();
            for (int i = 0; i < _gameManager._CurrentHoveredTile.Tower.GetComponent<Knight>()._targetedTiles.Count; i++)
            {
                tempList.Add(_gameManager._CurrentHoveredTile.Tower.GetComponent<Knight>()._targetedTiles[i]);
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                _gameManager.Tiles[tempList[i].x].Tiles[tempList[i].y].AttackColor();
            }
        }
    }

    private void HideAttackSpots()
    {
        for (int x = 0; x < _gameManager.Tiles.Length; x++)
        {
            for (int y = 0; y < _gameManager.Tiles[0].Tiles.Length; y++)
            {
                _gameManager.Tiles[x].Tiles[y].OriginalColor();
            }
        }

    }
}
