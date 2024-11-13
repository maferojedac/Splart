// Class that provides communication for all objects in game. Update states, keep track of nodes, etc.
// Created by Javier Soto

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/MovementMap")]
public class LevelData : ScriptableObject
{
    public List<MapNode> _nodes = new();    //  Publicly accesible nodes taht can be called at random

    public IGameState _levelInstance;       // Reference to current loaded level

    public bool _gameRunning;   // Control boolean

    public int _currentScore;   // Game Score

    public float _globalEnemySpeedMultiplier;   // Self explanatory
    public float _globalEnemyWaveSpeedMultiplier;   // For wave speed managing

    private IGameState _menusInstance;      // Reference to menu instance
    private IGameState _baseGameInstance;   // Reference to base game instance

    private void OnEnable()
    {
        _nodes.Clear();
    }

    public void NextWave()  // Communicate next wave to all that apply
    {
        _levelInstance.NextWave();
        _menusInstance.NextWave();
        _baseGameInstance.NextWave();
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

    public void SumScore(int score)
    {
        _currentScore += score;
    }

    public int GetScore()
    {
        return _currentScore;
    }

    public void UnloadPreviousLevel()   // Ask level to unload
    {
        _levelInstance.UnloadLevel();
    }

    public void SetMenuInstance(IGameState menuInstance)
    {
        _menusInstance = menuInstance;
    }

    public void SetBaseGameInstance(IGameState baseGameInstance)
    {
        _baseGameInstance = baseGameInstance;
    }

    public void GameOver()  // Game End by death
    {
        _gameRunning = false;
        Time.timeScale = 1f;
        _levelInstance.GameOver();
        _menusInstance.GameOver();
        _baseGameInstance.GameOver();
    }

    public void EndGame()   // Force Game End
    {
        _gameRunning = false;
        Time.timeScale = 1f;
        _levelInstance.EndGame();
        _menusInstance.EndGame();    
        _baseGameInstance.EndGame(); 
    }

    public void StartGame() // Start new game
    {
        // reset variables
        _nodes.Clear();
        _currentScore = 0;
        _globalEnemySpeedMultiplier = 1f;
        _gameRunning = true;

        // start game events
        _levelInstance.StartGame();
        _menusInstance.StartGame();
        _baseGameInstance.StartGame();
    }

    public void SetLevelInstance(IGameState p_object)
    {
        _levelInstance = p_object;
    }

    public void SetPlayerPosition(Vector3 p_position)
    {
        // Let's try and assume that all levels have player at 0,0 and see how that works :P
        //_levelInstance.transform.position = -p_position;
    }

    public void RegisterNode(MapNode node)
    {
        _nodes.Add(node);
    }

    /*public List<MapNode> MakePathFromNodes(Quaternion p_atDirection, float p_FOVdegrees, Vector3 p_fromPosition)
    {
        List<MapNode> pathNodes = new List<MapNode>();
        foreach (MapNode node in _nodes)
        {
            if (nodeInArea(p_atDirection, p_FOVdegrees, p_fromPosition, node))
                pathNodes.Add(node);
        }

        if(pathNodes.Count > 1)
            for (int i = 0; i < pathNodes.Count - 1; i++)
            {
                for (int j = 0; j < pathNodes.Count - i - 1; j++)
                {
                    float DistanceCurrent = (pathNodes[j].Position - p_fromPosition).magnitude;
                    float DistanceNext = (pathNodes[j + 1].Position - p_fromPosition).magnitude;
                    if (DistanceCurrent > DistanceNext)
                    {
                        MapNode temp = pathNodes[j];
                        pathNodes[j] = pathNodes[j + 1];
                        pathNodes[j + 1] = temp;
                    }
                }
            }

        return pathNodes;
    }

    public MapNode ClosestNodeInPath(Quaternion p_atDirection, float p_FOVdegrees, Vector3 p_fromPosition)
    {
        MapNode closestNode = new MapNode(0, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), false);
        foreach (MapNode node in _nodes)
        {
            if (nodeInArea(p_atDirection, p_FOVdegrees, p_fromPosition, node))
                if ((node.Position - p_fromPosition).magnitude < (closestNode.Position - p_fromPosition).magnitude)
                    closestNode = (node);
        }

        return closestNode;
    } */

    public MapNode RandomNode()
    {
        return _nodes[Random.Range(0, _nodes.Count - 1)];
    }

    /*public bool AnyNodeInPath(Quaternion p_atDirection, float p_FOVdegrees, Vector3 p_fromPosition)
    {
        foreach (MapNode node in _nodes)
        {
            if (nodeInArea(p_atDirection, p_FOVdegrees, p_fromPosition, node))
                return true;
        }
        return false;
    }

    private bool nodeInArea(Quaternion p_atDirection, float p_FOVdegrees, Vector3 p_fromPosition, MapNode p_node)
    {
        // Ignore vertical offset in nodes
        p_fromPosition.y = p_node.Position.y;
        Quaternion transformAngle = Quaternion.FromToRotation(p_atDirection * Vector3.forward, (p_node.Position - p_fromPosition).normalized);
        float diffAngle = Quaternion.Angle(Quaternion.Euler(0, 0, 0), transformAngle);
        if (diffAngle < p_FOVdegrees)
            return true;
        return false;
    }*/

}
