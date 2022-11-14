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
    private GameManager _gameManager;

    public bool IsDragging = false;

    private List<Vector2Int> valid = new List<Vector2Int>();
    private CurrencySystem _currencySystem;

    private void Awake()
    {
        _currencySystem = FindObjectOfType<CurrencySystem>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    //When player starts to drag the icon runs this code once
    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0.6f;
    }

    //every time mouse moves it will run this code
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    //when stop dragging (let go of object) will run this code once
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

    //not sure why this is here but when i deleted it and it broke the sctipts so its chilling here
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    //resets the colors for all the tiles
    private void ResetColors()
    {
        for (int i = 0; i < valid.Count; i++)
        {
            _gameManager.Tiles[valid[i].x].Tiles[valid[i].y].OriginalColor();
        }
    }
}