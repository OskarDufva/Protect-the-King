using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _highlightColor;
    [SerializeField] private Color _attackHighlightColor;
    [SerializeField] private Color _validPawnPlacementColor;
    [SerializeField] private Color _inValidPawnPlacementColor;

    private DragDrop _dragDrop;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private List<EnemyStats> enemiesOnTile = new List<EnemyStats>();
    private GameManager _gameManager;
    
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
        PawnPlacement pawn = other.GetComponent<PawnPlacement>();
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
    }

    private void OnMouseExit()
    {
        _spriteRenderer.color = _originalColor;
        //TileTxt.SetActive(false);
        _gameManager._CurrentHoveredTile = null;
    }

}
