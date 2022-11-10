using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;

    private GameManager _gameManager;

    public List<Vector2Int> _targetedTiles = new List<Vector2Int>();
    private RookPlacement _rookplacement;
    private Animator _animator;

    private float timer = 0.0f;

    public int SellGold;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _rookplacement = FindObjectOfType<RookPlacement>();
        _animator = GetComponent<Animator>();
        GetTargetedTiles();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > _attackSpeed)
        {
            DealDamage();
            print("deal damage rook");
            _animator.Play("Base Layer.rookstompanim", 0, 0.25f);
            timer = 0;
        }

    }

    private void GetTargetedTiles()
    {
        for (int i = 0; i < _rookplacement.PossibleValids.Count; i++)
        {
            _targetedTiles.Add(_rookplacement.PossibleValids[i]);
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