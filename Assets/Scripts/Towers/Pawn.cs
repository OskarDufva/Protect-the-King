using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;

    private GameManager _gameManager;

    public List<Vector2Int> _targetedTiles = new List<Vector2Int>();
    private PawnPlacement pawnplacement;
    private Animator _animator;

    private float timer = 0.0f;

    public int SellGold;

    //runs code when the game starts
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        pawnplacement = FindObjectOfType<PawnPlacement>();
        _animator = GetComponent<Animator>();
        GetTargetedTiles();
    }

    //runs every frame
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > _attackSpeed)
        {
            DealDamage();
            timer = 0;
            if (_gameManager.WaveIsActive)
            {
                _animator.Play("Base Layer.pawnstompanim", 0, 0.25f);
            }
        }

    }

    //get the list of target tiles
    private void GetTargetedTiles()
    {
        for (int i = 0; i < pawnplacement.PossibleValids.Count; i++)
        {
            _targetedTiles.Add(pawnplacement.PossibleValids[i]);
        }
    }

    //deals damage on the targeted tiles
    private void DealDamage()
    {
        for (int i = 0; i < _targetedTiles.Count; i++)
        {
            _gameManager.Tiles[_targetedTiles[i].x].Tiles[_targetedTiles[i].y].DealDamage(_damage);
        }
        
    }
}
