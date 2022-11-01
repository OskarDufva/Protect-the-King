using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;

    private GameManager _gameManager;

    [SerializeField] private List<Vector2Int> _targetedTiles = new List<Vector2Int>();
    private PawnPlacement pawnplacement;

    private float timer = 0.0f;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        pawnplacement = FindObjectOfType<PawnPlacement>();
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
        for (int i = 0; i < pawnplacement.PossibleValids.Count; i++)
        {
            _targetedTiles.Add(pawnplacement.PossibleValids[i]);
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
