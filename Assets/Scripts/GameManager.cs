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
    private int _currentWave = 0;
    private int _gameSpeed = 1;
    private SellPieces _dragDrop;

    [HideInInspector]
    public int width;
    public int height;

    public Phases Phases = Phases.InitialPhase;

    public Tile _CurrentHoveredTile;

    [SerializeField] private GameObject _gameOverUI;

    public TileArray[] Tiles;
    [SerializeField] public TileArray[] EmptyTiles;

    public GameObject StartWaveButton;
    public float GoldBoost;

    private void Start()
    {
        _waveManager = FindObjectOfType<Wavemanager>();
        _dragDrop = FindObjectOfType<SellPieces>();
        WaveTxt();
    }
    public void GameSpeedChange()
    {
        if (_gameSpeed == 1)
        {
            _gameSpeed = 2;
            _speedTxt.text = ">>";
            ChangeSpeed();
        }
        else if (_gameSpeed == 2)
        {
            _gameSpeed = 4;
            _speedTxt.text = ">>>";
            ChangeSpeed();
        }
        else if (_gameSpeed == 4)
        {
            _gameSpeed = 1;
            _speedTxt.text = ">";
            ChangeSpeed();
        }
    }

    private void ChangeSpeed()
    {
        Time.timeScale = _gameSpeed;
    }

    public void NextWave()
    {
        if (_waveManager.EnemiesAlive <= 0 && Phases == Phases.PreparationPhase)
        {
            print(Phases);
            Phases = Phases.ActionPhase;

            StartWaveButton.transform.gameObject.SetActive(false);

            _waveManager.SpawnWave(_currentWave);
            WaveTxt();
            _currentWave++;
        }
    }

    public void ChangePhases(Phases newPhase)
    {
        Phases = newPhase;
    }

    private void WaveTxt()
    {
        int value = _currentWave + 1;
        _waveTxt.text = "Wave: " + value + "/" + _waveManager.WaveAmountCounter().ToString();
    }

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

    public void ResetColors()
    {
        for (int x = 0; x < Tiles.Length; x++)
        {
            for (int y = 0; y < Tiles[0].Tiles.Length; y++)
            {
                Tiles[x].Tiles[y].OriginalColor();
                Tiles[x].Tiles[y].HideKingBoost();
            }
        }
    }

    public void GameOver()
    {
        _gameOverUI.SetActive(true);
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
