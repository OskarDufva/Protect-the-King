using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Tilemaps;
//No idea how this works now as its multithreading and runs fast but i didnt code this
public class PathFinding : MonoBehaviour
{
    [Header("Algo")]
    [SerializeField] private int Iterations;
    [SerializeField] private int WaitAfterIterations;
    [SerializeField] private int Seed;

    [Header("Setup")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private Tile _startTile;
    [SerializeField] private Tile _endTile;
    [Space(5)]
    public List<Tile> _tiles = new List<Tile>();

    [Header("Visual")]
    [SerializeField] private Gradient _gradient;
    [SerializeField] private bool _useGradient;

    private List<List<Tile>> Listtoendalllists = new List<List<Tile>>();
    private List<TileData> Listtoendalljoblists = new List<TileData>();

    [ContextMenu("DOOOOMS DAY")]

    public async void Fun()
    {
        Listtoendalllists.Clear();
        for (int i = 0; i < Iterations; i++)
        {
            if (i % WaitAfterIterations == 0)
            {
                DisplayLongest(false);
                await Task.Yield();
            }

            FindThePath();
        }

        DisplayLongest(true);
    }

    [ContextMenu("Please just work man")]
    public void VeryFun()
    {
        Wavemanager wavemanager = FindObjectOfType<Wavemanager>();

        NativeArray<TileData> allTiles = new NativeArray<TileData>(_tiles.Count, Allocator.TempJob);
        for (int i = 0; i < allTiles.Length; i++)
        {
            allTiles[i] = new TileData(_tiles.IndexOf(_tiles[i]), false, _tiles[i].EnemyPathTile);
        }

        NativeArray<TileData> thenativearraytoendallnativearrays = new NativeArray<TileData>(allTiles.Length, Allocator.TempJob);
        for (int i = 0; i < thenativearraytoendallnativearrays.Length; i++)
        {
            thenativearraytoendallnativearrays[i] = new TileData(-1, false, false);
        }

        PathJob job = new PathJob()
        {
            Width = _width,
            Height = _height,
            StartTile = new TileData(_tiles.IndexOf(_startTile), false, _startTile.EnemyPathTile),
            EndTile = new TileData(_tiles.IndexOf(_endTile), false, _startTile.EnemyPathTile),
            AllTiles = allTiles,
            Listtoendalllists = thenativearraytoendallnativearrays,
            Random = new Unity.Mathematics.Random((uint)(Seed == 0 ? UnityEngine.Random.Range(1, 10000) : Seed))
        };


        JobHandle handle = job.Schedule(Iterations, 50);

        handle.Complete();

        if (handle.IsCompleted)
        {
            Listtoendalljoblists = job.Listtoendalllists.ToList();

            DisplayLongestJob(true);

            thenativearraytoendallnativearrays.Dispose();
            allTiles.Dispose();
        }

        wavemanager.SpawnPoint = _startTile.transform;
    }

    private void DisplayLongestJob(bool finished)
    {
        if (Listtoendalljoblists.Count == 0)
        {
            Debug.LogError("goose egg");
            return;
        }

        List<TileData> actualListlistToenalllists = Listtoendalljoblists.ToList();
        for (int i = 0; i < actualListlistToenalllists.Count; i++)
        {
            if (actualListlistToenalllists[i].Index == -1)
            {
                actualListlistToenalllists.RemoveAt(i--);
            }
        }

        if (!finished)
        {
            print("Best so far: " + actualListlistToenalllists.Count);
        }
        else
        {
            print("Best Found: <color=green>" + actualListlistToenalllists.Count + "</color>");
            List<Vector3> posistions = new List<Vector3>();
            for (int i = 0; i < actualListlistToenalllists.Count; i++)
            {
                posistions.Add(_tiles[actualListlistToenalllists[i].Index].transform.position);
            }
            FindObjectOfType<GameManager>().EnemyPathFinding = posistions;
        }

        Reset();
        for (int i = 0; i < actualListlistToenalllists.Count; i++)
        {
            float value = (float)i / (float)actualListlistToenalllists.Count;
            if (_useGradient)
            {
            _tiles[actualListlistToenalllists[i].Index].ChangeColor(_gradient.Evaluate(value));
            }
        }
    }

    private void DisplayLongest(bool finished)
    {
        if (Listtoendalllists.Count == 0)
        {
            return;
        }

        int HighestCount = 0;
        int index = 0;
        for (int i = 0; i < Listtoendalllists.Count; i++)
        {
            if (Listtoendalllists[i].Count > HighestCount)
            {
                index = i;
                HighestCount = Listtoendalllists[i].Count;
            }
        }

        if (!finished)
        {
            print("Best so far: " + Listtoendalllists[index].Count);
        }
        else
        {
            print("Best Found: <color=green>" + Listtoendalllists[index].Count + "</color>");
            FindObjectOfType<GameManager>().EnemyPathFinding = Listtoendalllists[index].Select(x => x.transform.position).ToList();
            if (FindObjectOfType<EnemyMovement>())
            {
                FindObjectOfType<EnemyMovement>().HandleMovement();
            }
            
        }

        Reset();
        for (int i = 0; i < Listtoendalllists[index].Count; i++)
        {
            float value = (float)i / (float)Listtoendalllists[index].Count;
            Listtoendalllists[index][i].ChangeColor(_gradient.Evaluate(value));
        }
    }

    public void FindThePath()
    {
        Reset();

        List<Tile> pathTiles = new List<Tile>() { _startTile };
        List<int> validNeighbours = NeighbourChecker(_tiles.IndexOf(_startTile));

        _startTile.Walked = true;
        while (validNeighbours.Count > 0)
        {
            int randomint = Random.Range(0, validNeighbours.Count);
            int chosenIndex = validNeighbours[randomint];
            validNeighbours = NeighbourChecker(chosenIndex);

            _tiles[chosenIndex].Walked = true;
            pathTiles.Add(_tiles[chosenIndex]);        
        }

        if (_endTile != null)
        {
            if (pathTiles[pathTiles.Count - 1] == _endTile)
            {
                Listtoendalllists.Add(pathTiles);
            }
        }
        else
        {
            Listtoendalllists.Add(pathTiles);
        }
    }

    private List<int> NeighbourChecker(int index)
    {
        List<int> valid = new List<int>();
        if ((index % _width) - 1 >= 0)
        {
            valid.Add(index - 1);
        }
        if ((index % _width) + 1 < _height)
        {
            valid.Add(index + 1);
        }
        if (index - _height >= 0)
        {
            valid.Add(index - _height);
        }
        if (index + _height < _tiles.Count)
        {
            valid.Add(index + _height);
        }
        for (int i = 0; i < valid.Count; i++)
        {
            if (!_tiles[valid[i]].EnemyPathTile)
            {
                valid.RemoveAt(i);
                i--;
                continue;
            }
            if (_tiles[valid[i]].Walked)
            {
                valid.RemoveAt(i);
                i--;
                continue;
            }
        }
        return valid;

    }

    private void Reset()
    {
        for (int i = 0; i < _tiles.Count; i++)
        {
            _tiles[i].Walked = false;
            if (_tiles[i].EnemyPathTile)
            {
                if (_useGradient)
                {
                    _tiles[i].ChangeColor(Color.gray);
                }
            }
            
        }
    }
}

[BurstCompile]
public struct PathJob : IJobParallelFor
{
    public Unity.Mathematics.Random Random;

    public TileData StartTile;
    public TileData EndTile;

    public int Width;
    public int Height;

    [ReadOnly]
    public NativeArray<TileData> AllTiles;

    [NativeDisableParallelForRestriction]
    public NativeArray<TileData> Listtoendalllists;

    public void Execute(int index)
    {
        NativeArray<TileData> LocalAllTiles = new NativeArray<TileData>(AllTiles.Length, Allocator.Temp);
        for (int i = 0; i < AllTiles.Length; i++)
        {
            LocalAllTiles[i] = new TileData(AllTiles[i].Index, false, AllTiles[i].IsEnemyPath);
        }

        NativeArray<TileData> pathTiles = new NativeArray<TileData>(LocalAllTiles.Length, Allocator.Temp);
        for (int i = 0; i < pathTiles.Length; i++)
        {
            if (i == 0)
            {
                pathTiles[i] = new TileData(StartTile.Index, true, true);
            }
            else
            {
                pathTiles[i] = new TileData(-1, false, false);
            }
        }

        NativeArray<int> validNeighbours = NeighbourChecker(LocalAllTiles, StartTile.Index);
         
        int length = GetLength(validNeighbours); 
        int pathIndex = 1;
        while (length > 0)
        {
            int chosenIndex = -1; 
            while (chosenIndex == -1)
            {
                int randomint = Random.NextInt(0, 4);
                chosenIndex = validNeighbours[randomint];
            }

            validNeighbours = NeighbourChecker(LocalAllTiles, chosenIndex);
            length = GetLength(validNeighbours);

            LocalAllTiles[chosenIndex] = new TileData(LocalAllTiles[chosenIndex].Index, true, LocalAllTiles[chosenIndex].IsEnemyPath);
            pathTiles[pathIndex++] = LocalAllTiles[chosenIndex];
        }

        if (EndTile.Index != -1)
        {
            if (pathTiles[GetLength(pathTiles) - 1].Index == EndTile.Index) 
            {
                if (GetLength(pathTiles) > GetLength(Listtoendalllists))
                {
                    for (int i = 0; i < pathTiles.Length; i++)
                    {
                        Listtoendalllists[i] = pathTiles[i];
                    }
                }
            }
        }
        else
        {
            if (GetLength(pathTiles) > GetLength(Listtoendalllists))
            {
                for (int i = 0; i < pathTiles.Length; i++)
                {
                    Listtoendalllists[i] = pathTiles[i];
                }
            }
        }

        pathTiles.Dispose();
        validNeighbours.Dispose();
        LocalAllTiles.Dispose();
    }

    public int GetLength(NativeArray<int> arrya)
    {
        int length = 0;
        for (int i = 0; i < arrya.Length; i++)
        {
            if (arrya[i] != -1)
            {
                length++;
            }
        }

        return length;
    }

    public int GetLength(NativeArray<TileData> arrya)
    {
        int length = 0;
        for (int i = 0; i < arrya.Length; i++)
        {
            if (arrya[i].Index != -1)
            {
                length++;
            }
        }

        return length;
    }

    private NativeArray<int> NeighbourChecker(NativeArray<TileData> tiles, int index)
    {
        NativeArray<int> valid = new NativeArray<int>(4, Allocator.Temp);
        for (int i = 0; i < valid.Length; i++)
        {
            valid[i] = -1;
        }

        if ((index % Width) - 1 >= 0)
        {
            valid[0] = (index - 1);
        }
        if ((index % Width) + 1 < Height)
        {
            valid[1] = (index + 1);
        }
        if (index - Height >= 0)
        {
            valid[2] = (index - Height);
        }
        if (index + Height < tiles.Length)
        {
            valid[3] = (index + Height);
        }
        for (int i = 0; i < valid.Length; i++)
        {
            if (valid[i] == -1)
            {
                continue;
            }

            if (!tiles[valid[i]].IsEnemyPath)
            {
                valid[i] = -1;
                continue;
            }
            if (tiles[valid[i]].Walked)
            {
                valid[i] = -1;
                continue;
            }
        }

        return valid;
    }
}

public struct TileData
{
    public int Index;
    public bool Walked;
    public bool IsEnemyPath;

    public TileData(int index, bool walked, bool isEnemyPath)
    {
        Index = index;
        Walked = walked;
        IsEnemyPath = isEnemyPath;
    }
}