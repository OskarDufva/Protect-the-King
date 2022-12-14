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
    [SerializeField] private GameObject _kingBoostText;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private List<EnemyStats> enemiesOnTile = new List<EnemyStats>();
    private GameManager _gameManager;
    
    public GameObject Tower;
    public GameObject TileTxt;
    public bool EnemyPathTile;
    public bool KingSpot;
    public float _kingSpotBoost;
    public Vector2Int Index;
    [HideInInspector]
    public bool OccupiedTile;

    public bool Walked { get; set; }
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _gameManager = FindObjectOfType<GameManager>();
        _kingBoostText.GetComponent<TextMeshPro>().text = "Gold " + _kingSpotBoost.ToString() + "X";
    }

    //when called you can change the color of the tile to color you give it when calling the function
    public void ChangeColor(Color color)
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = color;
    }

    //sets the tile color to its original color it started with
    public void OriginalColor()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = _originalColor;
    }

    //turns the color into the color specified for attacks
    public void AttackColor()
    {
        if(_spriteRenderer == null)
        {
         _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = _attackHighlightColor;
    }

    //turns the color into the color specified for validplacements
    public void ValidPlacementColor()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = _validPawnPlacementColor;
    }

    //turns the color into the color specified for Invalidplacements
    public void InvalidPlacementColor()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = _inValidPawnPlacementColor;
    }

    //When an enemy overlaps with the tile colidor it will add it to the tile enemy list
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

    //When an enemy overlaps with the tile colidor it will removes it from the tile enemy list
    private void OnTriggerExit(Collider other)
    {
        EnemyStats enemyStats = other.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            enemiesOnTile.Remove(enemyStats);
        }
    }

    //will deal damage all enemies on the enemylist
    public void DealDamage(float damage)
    {
        for (int i = 0; i < enemiesOnTile.Count; i++)
        {
            if (enemiesOnTile[i] == null)
            {
                enemiesOnTile.RemoveAt(i--);
            }
        }
        
        if (enemiesOnTile.Count != 0) 
        {
            for (int i = 0; i < enemiesOnTile.Count; i++)
            {
                enemiesOnTile[i].TakeDamage(damage);
                if (enemiesOnTile[i].health <= 0)
                {
                    EnemyStats tempEnemy = enemiesOnTile[i];
                    enemiesOnTile.RemoveRange(i--, 1);
                    Destroy(tempEnemy.gameObject);
                }
            }
        }
    }

    //when hovering over the tile with the mouse will display hightlight and when turns it off when no longer hovering the tile
    #region Highlight
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
            HideAttackSpots();
        }
        _gameManager._CurrentHoveredTile = null;

    }
    #endregion

    //when hovering over the a tile with a tower on it will display its targeted spots
    #region AttackSpots
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

        Rook rook = _gameManager._CurrentHoveredTile.Tower.GetComponent<Rook>();
        if(rook != null)
        {
            List<Vector2Int> tempList = new List<Vector2Int>();
            for (int i = 0; i < _gameManager._CurrentHoveredTile.Tower.GetComponent<Rook>()._targetedTiles.Count; i++)
            {
                tempList.Add(_gameManager._CurrentHoveredTile.Tower.GetComponent<Rook>()._targetedTiles[i]);
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                _gameManager.Tiles[tempList[i].x].Tiles[tempList[i].y].AttackColor();
            }
        }

        Bishop bishop = _gameManager._CurrentHoveredTile.Tower.GetComponent<Bishop>();
        if (bishop != null)
        {
            List<Vector2Int> tempList = new List<Vector2Int>();
            for (int i = 0; i < _gameManager._CurrentHoveredTile.Tower.GetComponent<Bishop>()._targetedTiles.Count; i++)
            {
                tempList.Add(_gameManager._CurrentHoveredTile.Tower.GetComponent<Bishop>()._targetedTiles[i]);
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

    #endregion
    //Will set the tower on the to correct sorting layer
    public void OrderLayer()
    {
        if (Tower != null)
        {
            SpriteRenderer tempTower = Tower.GetComponent<SpriteRenderer>();
            tempTower.sortingOrder = _gameManager.height - Index.y;
        }
    }

    //actives and deactivates the text of the king boost spots
    #region KingBoost
    public void ShowKingBoost()
    {
        _kingBoostText.SetActive(true);
    }


    public void HideKingBoost()
    {
        _kingBoostText.SetActive(false);
    }
    #endregion
}
