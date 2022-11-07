using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SellPieces : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    [SerializeField] private Canvas _canvas;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private PiecePlacement _placement;
    private GameManager _gameManager;

    public bool IsDragging = false;

    private List<Vector2Int> valid = new List<Vector2Int>();
    private Vector2Int ValidIndex;
    private CurrencySystem _currencySystem;

    private void Awake()
    {
        _currencySystem = FindObjectOfType<CurrencySystem>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _placement = GetComponent<PiecePlacement>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0.6f;
        DisplayAttack();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        DisplayAttack();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _rectTransform.localPosition = Vector3.zero;
        ResetColors();
        _gameManager.ResetColors();
        if(_gameManager._CurrentHoveredTile.Tower == null)
        {
            return;
        }
        Pawn pawn = _gameManager._CurrentHoveredTile.Tower.GetComponent<Pawn>();
        if (pawn != null)
        {
            _currencySystem.ChangeGold(pawn.SellGold);
            Destroy(pawn.gameObject);
            _gameManager._CurrentHoveredTile.OccupiedTile = false;
            _gameManager._CurrentHoveredTile.Tower = null;
            return;
        }

        Rook rook = _gameManager._CurrentHoveredTile.Tower.GetComponent<Rook>();
        if (rook!= null)
        {
            _currencySystem.ChangeGold(rook.SellGold);
            Destroy(rook.gameObject);
            _gameManager._CurrentHoveredTile.OccupiedTile = false;
            _gameManager._CurrentHoveredTile.Tower = null;
            return;
        }

        Knight knight = _gameManager._CurrentHoveredTile.Tower.GetComponent<Knight>();
        if(knight != null)
        {
            _currencySystem.ChangeGold(knight.SellGold);
            Destroy(knight.gameObject);
            _gameManager._CurrentHoveredTile.OccupiedTile = false;
            _gameManager._CurrentHoveredTile.Tower = null;
            return;
        }

        Bishop bishop = _gameManager._CurrentHoveredTile.Tower.GetComponent<Bishop>();
        if (bishop != null)
        {
            _currencySystem.ChangeGold(bishop.SellGold);
            Destroy(bishop.gameObject);
            _gameManager._CurrentHoveredTile.OccupiedTile = false;
            _gameManager._CurrentHoveredTile.Tower = null;
            return;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    private void ResetColors()
    {
        for (int i = 0; i < valid.Count; i++)
        {
            _gameManager.Tiles[valid[i].x].Tiles[valid[i].y].OriginalColor();
        }
    }

    private void DisplayAttack(bool run = false)
    {
        if (_gameManager._CurrentHoveredTile == null)
        {
            return;
        }

        GetValidPositions(run);

        for (int i = 0; i < valid.Count; i++)
        {
            _gameManager.Tiles[valid[i].x].Tiles[valid[i].y].AttackColor();
        }
    }

    private void GetValidPositions(bool run)
    {
        if (_gameManager._CurrentHoveredTile.Index == ValidIndex && run == false)
        {
            return;
        }

        ResetColors();

        valid = new List<Vector2Int>();
        ValidIndex = _gameManager._CurrentHoveredTile.Index;

        for (int x = 0; x < _gameManager.Tiles.Length; x++)
        {
            for (int y = 0; y < _gameManager.Tiles[x].Tiles.Length; y++)
            {
                if (_gameManager.Tiles[x].Tiles[y].Tower == null)
                {
                    continue;
                }

                Pawn pawn = _gameManager.Tiles[x].Tiles[y].Tower.GetComponent<Pawn>();
                Rook rook = _gameManager.Tiles[x].Tiles[y].Tower.GetComponent<Rook>();
                Bishop bishop = _gameManager.Tiles[x].Tiles[y].Tower.GetComponent<Bishop>();
                Knight Knight = _gameManager.Tiles[x].Tiles[y].Tower.GetComponent<Knight>();
                if (pawn != null && rook != null && bishop != null && Knight != null)
                {
                    _gameManager.Tiles[x].Tiles[y].ValidPlacementColor();
                }
            }
        }

    }
}