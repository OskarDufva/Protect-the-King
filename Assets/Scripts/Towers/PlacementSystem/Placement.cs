using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Placement : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private bool _placeKing;
    [SerializeField] private bool UpgradePiece;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private PiecePlacement _placement;
    private GameManager _gameManager;
    private Direction _direction;

    public bool IsDragging = false;

    
    public List<Vector2Int> AttackDirectionRight = new List<Vector2Int>();
    public List<Vector2Int> AttackDirectionDown = new List<Vector2Int>();
    public List<Vector2Int> AttackDirectionLeft = new List<Vector2Int>();
    public List<Vector2Int> AttackDirectionUp = new List<Vector2Int>();

    [HideInInspector]
    public List<Vector2Int> PossibleValids = new List<Vector2Int>();
    private Vector2Int ValidIndex;

    public int cost;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _placement = GetComponent<PiecePlacement>();
        _gameManager = FindObjectOfType<GameManager>();

    }

    private void Update()
    {
        if(IsDragging == false)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_direction == Direction.Right)
            {
                _direction = Direction.Down;
                DisplayAttack(true);
            }

            else if (_direction == Direction.Down)
            {
                _direction = Direction.Left;
                DisplayAttack(true);
            }

            else if (_direction == Direction.Left)
            {
                _direction = Direction.Up;
                DisplayAttack(true);
            }

            else if (_direction == Direction.Up)
            {
                _direction = Direction.Right;
                DisplayAttack(true);
            }
            DisplayAttack(true);
        }
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0.6f;
        _gameManager.HighlightUnoccupiedTiles(_placeKing);
        IsDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        _gameManager.HighlightUnoccupiedTiles(_placeKing);
        DisplayAttack(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _rectTransform.localPosition = Vector3.zero;
        _placement.SpawnTower(_placeKing,cost,UpgradePiece);
        ResetColors();
        _gameManager.ResetColors();
        IsDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    private void ResetColors()
    {
        for (int i = 0; i < PossibleValids.Count; i++)
        {
            _gameManager.Tiles[PossibleValids[i].x].Tiles[PossibleValids[i].y].OriginalColor();
        }
    }

    private void DisplayAttack(bool run = false)
    {
        _gameManager.HighlightUnoccupiedTiles(_placeKing);
        if (_gameManager._CurrentHoveredTile == null)
        {
            return;
        }

        GetValidPositions(_direction, run);
        
        for (int i = 0; i < PossibleValids.Count; i++)
        {
            _gameManager.Tiles[PossibleValids[i].x].Tiles[PossibleValids[i].y].AttackColor();
        }

    }

    private void GetValidPositions(Direction direction, bool run)
    {
        if (_gameManager._CurrentHoveredTile.Index == ValidIndex && run == false)
        {
            return;
        }

        ResetColors();

        PossibleValids = new List<Vector2Int>();
        ValidIndex = _gameManager._CurrentHoveredTile.Index;

        int xLength = _gameManager.Tiles.Length;
        int yLength = _gameManager.Tiles[0].Tiles.Length;

        switch (direction)
        {
            case Direction.Right:
                for (int i = 0; i < AttackDirectionRight.Count; i++)
                {
                    Vector2Int temp = new Vector2Int(this.ValidIndex.x + AttackDirectionRight[i].x, this.ValidIndex.y + AttackDirectionRight[i].y);
                    PossibleValids.Add(temp);
                }
                break;

            case Direction.Left:
                for (int i = 0; i < AttackDirectionLeft.Count; i++)
                {
                    Vector2Int temp = new Vector2Int(this.ValidIndex.x + AttackDirectionLeft[i].x, this.ValidIndex.y + AttackDirectionLeft[i].y);
                    PossibleValids.Add(temp);
                }
                break;

            case Direction.Up:
                for (int i = 0; i < AttackDirectionUp.Count; i++)
                {
                    Vector2Int temp = new Vector2Int(this.ValidIndex.x + AttackDirectionUp[i].x, this.ValidIndex.y + AttackDirectionUp[i].y);
                    PossibleValids.Add(temp);
                }
                break;

            case Direction.Down:
                for (int i = 0; i < AttackDirectionDown.Count; i++)
                {
                    Vector2Int temp = new Vector2Int(this.ValidIndex.x + AttackDirectionDown[i].x, this.ValidIndex.y + AttackDirectionDown[i].y);
                    PossibleValids.Add(temp);
                }
                break;

            default:
                break;
        }


        for (int i = 0; i < PossibleValids.Count; i++)
        {
            if (PossibleValids[i].x >= xLength || PossibleValids[i].x < 0)
            {
                PossibleValids.RemoveAt(i--);
                continue;
            }

            if (PossibleValids[i].y >= yLength || PossibleValids[i].y < 0)
            {
                PossibleValids.RemoveAt(i--);
                continue;
            }
            //this will only show the possible attack on the enemy path
            //if (_gameManager.Tiles[PossibleValids[i].x].Tiles[PossibleValids[i].y].EnemyPathTile == false)
            //{
            //    PossibleValids.RemoveAt(i--);
            //    continue;
            //}
        }
    }
}

