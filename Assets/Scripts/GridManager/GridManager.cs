using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;
    /// <summary>
    ///creates the grid when pressing create grid button based on height and width that are entered
    /// it will then add all tiles into the pathfinder and gamemanager automaticly
    /// When creating a new grid will also delete old tiles and all old refrences
    /// </summary>
    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.Tiles = new TileArray[_width];

        for (int i = 0; i < gameManager.Tiles.Length; i++)
        {
            gameManager.Tiles[i].Tiles = new Tile[_height];
        }

        PathFinding pathFinding = FindObjectOfType<PathFinding>();
        pathFinding._tiles.Clear();

        gameManager.width = _width;
        gameManager.height = _height;
        
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.Index = new Vector2Int(x, y);

                gameManager.Tiles[x].Tiles[y] = spawnedTile;

                pathFinding._tiles.Add(spawnedTile);

                spawnedTile.transform.parent = this.transform;
                spawnedTile.name = $"Tile {x}, {y}";
                if (spawnedTile.TileTxt != null)
                {
                    spawnedTile.TileTxt.GetComponent<TextMeshPro>().text = spawnedTile.name;
                }
            }
        }
        _cam.transform.position = new Vector3((float)_width/2 -0.5f, (float)_height / 2 - 0.5f, -10);
    }

}

[System.Serializable]
public struct TileArray
{
    public Tile[] Tiles;
}
