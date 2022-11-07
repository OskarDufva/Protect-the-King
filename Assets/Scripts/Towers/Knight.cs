using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;

    private GameManager _gameManager;

    public List<Vector2Int> _targetedTiles = new List<Vector2Int>();
    private KnightPlacement _knightplacement;

    private float timer = 0.0f;

    public int SellGold;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _knightplacement = FindObjectOfType<KnightPlacement>();
        GetTargetedTiles();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > _attackSpeed)
        {
            DealDamage();
            timer = 0;
        }

    }

    private void GetTargetedTiles()
    {
        for (int i = 0; i < _knightplacement.PossibleValids.Count; i++)
        {
            _targetedTiles.Add(_knightplacement.PossibleValids[i]);
        }
    }
    private void DealDamage()
    {
        for (int i = 0; i < _targetedTiles.Count; i++)
        {
            _gameManager.Tiles[_targetedTiles[i].x].Tiles[_targetedTiles[i].y].DealDamage(_damage);
        }
        
    }
}
