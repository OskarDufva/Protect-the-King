using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;

    private GameManager _gameManager;

    public List<Vector2Int> _targetedTiles = new List<Vector2Int>();
    private BishopPlacement _bishopplacement;

    private float timer = 0.0f;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _bishopplacement = FindObjectOfType<BishopPlacement>();
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
        for (int i = 0; i < _bishopplacement.PossibleValids.Count; i++)
        {
            _targetedTiles.Add(_bishopplacement.PossibleValids[i]);
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
