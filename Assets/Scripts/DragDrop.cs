using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    [SerializeField] private Canvas _canvas;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private PiecePlacement _placement;
    private GameManager _gameManager;
    private Direction _direction;

    public bool IsDragging = false;

    public List<Vector2Int> valid = new List<Vector2Int>();
    private Vector2Int ValidIndex;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _placement = GetComponent<PiecePlacement>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            print("Changed direction");
            if(_direction == Direction.Right)
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
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0.6f;
        _gameManager.HighlightUnoccupiedTiles(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("Drag");
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        _gameManager.HighlightUnoccupiedTiles(false);
        DisplayAttack();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _rectTransform.localPosition = Vector3.zero;
        _placement.SpawnTower(false,1,false);
        ResetColors();
        _gameManager.ResetColors();
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

        GetValidPositions(_direction, run);

        for (int i = 0; i < valid.Count; i++)
        {
            _gameManager.Tiles[valid[i].x].Tiles[valid[i].y].AttackColor();
        }
    }
    
    private void GetValidPositions(Direction direction, bool run)
    {
        if(_gameManager._CurrentHoveredTile.Index == ValidIndex && run == false)
        {
            return;
        }

        ResetColors();

        valid = new List<Vector2Int>();
        ValidIndex = _gameManager._CurrentHoveredTile.Index;

        int xLength = _gameManager.Tiles.Length;
        int yLength = _gameManager.Tiles[0].Tiles.Length;
        Vector2Int attackPos1 = new Vector2Int(ValidIndex.x + 1, ValidIndex.y + 1); // top right
        Vector2Int attackPos2 = new Vector2Int(ValidIndex.x + 1, ValidIndex.y - 1); // bottom right
        Vector2Int attackPos3 = new Vector2Int(ValidIndex.x - 1, ValidIndex.y - 1); // bottom left
        Vector2Int attackPos4 = new Vector2Int(ValidIndex.x - 1, ValidIndex.y + 1); // top left

        switch (direction)
        {
            case Direction.Right:
                valid.Add(attackPos1);
                valid.Add(attackPos2);
                break;

            case Direction.Left:
                valid.Add(attackPos3);
                valid.Add(attackPos4);
                break;

            case Direction.Up:
                valid.Add(attackPos4);
                valid.Add(attackPos1);
                break;

            case Direction.Down:
                valid.Add(attackPos3);
                valid.Add(attackPos2);
                break;

            default:
                break;
        }

        for (int i = 0; i < valid.Count; i++)
        {
            if (valid[i].x >= xLength || valid[i].x < 0)
            {
                valid.RemoveAt(i--);
                continue;
            }

            if (valid[i].y >= yLength || valid[i].y < 0)
            {
                valid.RemoveAt(i--);
                continue;
            }

            if (_gameManager.Tiles[valid[i].x].Tiles[valid[i].y].EnemyPathTile == false)
            {
                valid.RemoveAt(i--);
                continue;
            }
        }
    }
}

public enum Direction
{
    Right,
    Left,
    Up,
    Down
}