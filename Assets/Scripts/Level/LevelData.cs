// Class that provides communication for all objects in game. Update states, keep track of nodes, etc.
// Created by Javier Soto

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Communication/Level Data Asset")]
public class LevelData : ScriptableObject
{
    public List<MapNode> _nodes = new();    //  Publicly accesible nodes taht can be called at random

    public GameState _gameState;    // Communication with other game systems

    public bool _gameRunning;   // Control boolean

    public int _currentScore;   // Game Score

    public float _globalEnemySpeedMultiplier;   // Self explanatory
    public float _globalEnemyWaveSpeedMultiplier;   // For wave speed managing

    private List<ILevelEvent> _listenerObjects = new(); // Listeners to levle events

    private void OnEnable()
    {
        _nodes.Clear();
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

    public void SumScore(int score)
    {
        _currentScore += score;
        foreach (ILevelEvent listener in _listenerObjects)
        {
            listener.UpdateScore();
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

        _gameState.GameOver();
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
        _currentScore = 0;
        _globalEnemySpeedMultiplier = 1f;
        _gameRunning = true;

        // start game events
        _gameState.StartGame();
    }

    public void RegisterNode(MapNode node)
    {
        _nodes.Add(node);
    }

    public MapNode RandomNode()
    {
        return _nodes[Random.Range(0, _nodes.Count - 1)];
    }
}
