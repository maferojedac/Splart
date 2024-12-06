// Class that provides communication for all objects in game. Update states, keep track of nodes, etc.
// Created by Javier Soto

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Communication/Level Data Asset")]
public class LevelData : ScriptableObject
{
    public List<MapNode> _nodes = new();        //  Publicly accesible nodes taht can be called at random
    public List<MapNode> _sourceNodes = new();    //  Publicly accesible nodes taht can be called at random

    public GameState _gameState;    // Communication with other game systems

    public bool _gameRunning;   // Control boolean

    public int _currentScore;   // Game Score
    public int _currentAccumulatedMoney;

    public float _globalEnemySpeedMultiplier;   // Self explanatory
    public float _globalEnemyWaveSpeedMultiplier;   // For wave speed managing

    public int _maxColorCount;
    public int _currentColorCount;

    public int _enemiesDefeatedCount;

    public bool _bossBeaten;

    public string _levelName;

    private List<ILevelEvent> _listenerObjects = new(); // Listeners to levle events

    private void OnEnable()
    {
        _nodes.Clear();
        _sourceNodes.Clear();
    }

    public void SetGlobalSpeedMultiplier(float val)
    {
        _globalEnemySpeedMultiplier = val;
    }

    public void SetGlobalSpeedWaveMultiplier(float val)
    {
        _globalEnemyWaveSpeedMultiplier = val;
    }

    public float GetGlobalSpeedMultiplier()
    {
        return _globalEnemySpeedMultiplier;
    }
    public float GetGlobalSpeedWaveMultiplier()
    {
        return _globalEnemyWaveSpeedMultiplier;
    }

    public void SubscribeToEvents(ILevelEvent listener)
    {
        _listenerObjects.Add(listener);
    }

    public void SetMaxPaintableColors(int count)
    {
        _currentColorCount = 0;
        _maxColorCount = count;
    }

    public void SetLevelName(string name)
    {
        _levelName = name;
    }

    public void PaintObject()
    {
        _currentColorCount++;
        foreach (ILevelEvent listener in _listenerObjects)
        {
            listener.PaintObject();
        }
    }

    public void SpawnBoss()
    {
        // No boss sequence right now
        GameObject.Find("WaveManager").GetComponent<WaveManager>().SpawnBoss();
    }

    public void KillBoss()
    {
        _bossBeaten = true;
        GameOver();
    }

    public void SumScore(int score)
    {
        _currentScore += score;
        _enemiesDefeatedCount++;
        foreach (ILevelEvent listener in _listenerObjects)
        {
            listener.UpdateScore();
        }
    }

    public void SumMoney(int money)
    {
        _currentAccumulatedMoney += money;
        foreach (ILevelEvent listener in _listenerObjects)
        {
            listener.UpdateMoney();
        }
    }

    public int GetScore()
    {
        return _currentScore;
    }

    public void GameOver()  // Game End by death
    {
        _gameRunning = false;
        Time.timeScale = 1f;

        Debug.Log("End of game!! Palette size of level > " + _maxColorCount + ", total painted objects > " + _currentColorCount);
        bool Victory = _bossBeaten && (_maxColorCount == _currentColorCount);

        _gameState.GameOver(Victory);
    }

    public void EndGame()   // Force Game End
    {
        _gameRunning = false;
        Time.timeScale = 1f;

        _gameState.EndGame();
    }

    public void NextWave()  // Communicate next wave to all that apply
    {
        _gameState.NextWave();
    }

    public void StartGame() // Start new game
    {
        // reset variables
        _nodes.Clear();
        _sourceNodes.Clear();
        _currentScore = 0;
        _currentAccumulatedMoney = 0;
        _enemiesDefeatedCount = 0;
        _globalEnemySpeedMultiplier = 1f;
        _gameRunning = true;
        _bossBeaten = false;

        // start game events
        _gameState.StartGame();
    }

    public void RegisterNode(MapNode node)
    {
        _nodes.Add(node);
    }

    public void RegisterSourceNode(MapNode node)
    {
        _sourceNodes.Add(node);
    }

    public MapNode RandomNode()
    {
        return _nodes[Random.Range(0, _nodes.Count - 1)];
    }

    public MapNode RandomSourceNode()
    {
        return _sourceNodes[Random.Range(0, _sourceNodes.Count - 1)];
    }
}
