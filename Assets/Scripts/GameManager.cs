using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _speedTxt;
    [SerializeField] TextMeshProUGUI _waveTxt;

    private Wavemanager _waveManager;
    private int _gameSpeed = 1;
    private SellPieces _dragDrop;

    [HideInInspector]
    public int width;
    public int height;
    public int _currentWave = 0;

    public Phases Phases = Phases.InitialPhase;

    public Tile _CurrentHoveredTile;

    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _VictoryUI;

    public TileArray[] Tiles;
    [SerializeField] public TileArray[] EmptyTiles;

    public GameObject StartWaveButton;
    public float GoldBoost;
    public bool WaveIsActive;

    public List<Vector3> EnemyPathFinding = new List<Vector3>();

    //runs code when the game starts
    private void Start()
    {
        _waveManager = FindObjectOfType<Wavemanager>();
        _dragDrop = FindObjectOfType<SellPieces>();
        WaveTxt();
    }

    //detimantes the gamespeed
    public void GameSpeedChange()
    {
        if (_gameSpeed == 1)
        {
            _gameSpeed = 2;
            _speedTxt.text = "2x";
            ChangeSpeed();
        }
        else if (_gameSpeed == 2)
        {
            _gameSpeed = 3;
            _speedTxt.text = "3x";
            ChangeSpeed();
        }
        else if (_gameSpeed == 3)
        {
            _gameSpeed = 1;
            _speedTxt.text = "1x";
            ChangeSpeed();
        }
    }

    //set the game speed
    private void ChangeSpeed()
    {
        Time.timeScale = _gameSpeed;
    }

    //spawns the next wave
    public void NextWave()
    {
        if (_waveManager.EnemiesAlive <= 0 && Phases == Phases.PreparationPhase)
        {
            Phases = Phases.ActionPhase;

            StartWaveButton.transform.gameObject.SetActive(false);

            _waveManager.SpawnWave(_currentWave);
            WaveTxt();
            _currentWave++;
            StartWaveButton.transform.gameObject.SetActive(false);
            WaveIsActive = true;
            _waveManager.ReadyForNewWave = true;
        }
    }

    //code to change the phases
    public void ChangePhases(Phases newPhase)
    {
        Phases = newPhase;
    }

    //handles the wave text counter
    private void WaveTxt()
    {
        int value = _currentWave + 1;
        _waveTxt.text = "Wave: " + value + "/" + _waveManager.WaveAmountCounter().ToString();
    }

    //finds all the unocupied tiles
    public void HighlightUnoccupiedTiles(bool KingPlacement)
    {
        if(KingPlacement == false)
        {
            for (int x = 0; x < Tiles.Length; x++)
            {
                for (int y = 0; y < Tiles[0].Tiles.Length; y++)
                {
                    if (Tiles[x].Tiles[y] != _CurrentHoveredTile)
                    {
                        if (Tiles[x].Tiles[y].EnemyPathTile == false && Tiles[x].Tiles[y].OccupiedTile == false)
                        {
                            Tiles[x].Tiles[y].ValidPlacementColor();
                        }
                        else
                        {
                            Tiles[x].Tiles[y].InvalidPlacementColor();
                        }
                    }                
                }
            }
        }

        if(KingPlacement == true)
        {
            for (int x = 0; x < Tiles.Length; x++)
            {
                for (int y = 0; y < Tiles[0].Tiles.Length; y++)
                {
                    if (Tiles[x].Tiles[y] != _CurrentHoveredTile)
                    {
                        if (Tiles[x].Tiles[y].KingSpot == true)
                        {
                            Tiles[x].Tiles[y].ValidPlacementColor();
                            Tiles[x].Tiles[y].ShowKingBoost();
                        }
                        else
                        {
                            Tiles[x].Tiles[y].InvalidPlacementColor();
                        }
                    }
                }
            }
        }
    }

    //Resets colors hides the kingboost text and orders the towrs sorting layer
    public void ResetColors()
    {
        for (int x = 0; x < Tiles.Length; x++)
        {
            for (int y = 0; y < Tiles[0].Tiles.Length; y++)
            {
                Tiles[x].Tiles[y].OriginalColor();
                Tiles[x].Tiles[y].HideKingBoost();
                Tiles[x].Tiles[y].OrderLayer();
            }
        }
    }

    public void GameOver()
    {
        _gameOverUI.SetActive(true);
    }

    public void Victory()
    {
        _VictoryUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MaineMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

public enum Phases
{
    InitialPhase,
    PreparationPhase,
    ActionPhase
}
