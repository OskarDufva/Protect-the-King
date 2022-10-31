using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnTower : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;

    private GameManager _gameManager;

    [SerializeField] private List<Vector2Int> _targetedTiles = new List<Vector2Int>();
    private DragDrop _dragDrop;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _dragDrop = FindObjectOfType<DragDrop>();
        for (int i = 0; i < _dragDrop.valid.Count; i++)
        {
            _targetedTiles.Add(_dragDrop.valid[i]);
        }
    }

    private void Update()
    {
        DealDamage();
    }

    private void DealDamage()
    {
        for (int i = 0; i < _targetedTiles.Count; i++)
        {
            _gameManager.Tiles[_targetedTiles[i].x].Tiles[_targetedTiles[i].y].DealDamage(_damage);
        }
        
    }
}
