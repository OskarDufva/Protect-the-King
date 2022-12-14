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
    private Animator _animator;

    private float timer = 0.0f;

    public int SellGold;

    //runs code when the game starts
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _bishopplacement = FindObjectOfType<BishopPlacement>();
        _animator = GetComponent<Animator>();
        GetTargetedTiles();
    }

    //runs every frame deals damage every time timer goes above attacks speed and then resets
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > _attackSpeed)
        {
            DealDamage();
            print("dealt damage");
            timer = 0;
            if (_gameManager.WaveIsActive)
            {
                _animator.Play("Base Layer.bishopstompanim" ,0, 0.25f);
            }
        }

    }

    //get the list of target tiles
    private void GetTargetedTiles()
    {
        for (int i = 0; i < _bishopplacement.PossibleValids.Count; i++)
        {
            _targetedTiles.Add(_bishopplacement.PossibleValids[i]);
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
