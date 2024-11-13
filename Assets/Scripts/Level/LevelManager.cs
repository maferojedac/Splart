// Class used to load and manage levels, and to start games. 
// Menu system should have it attached already, so it should not be attached to any other game objects.

// Created by Javier Soto


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IGameState
{

    public GameObject TutorialLevel;    // Tutorial level ref
    public GameObject[] Levels;     // Dynamic level references

    public float ExitTime;  // Time that should be waited for level unloading

    public LevelData _levelData;    // Level communication

    private GameObject _lastLevel;  // Last level reference

    private readonly List<GameObject> _loadedLevels = new();        // Level pooling

    private TraversalMenu _menu;    // Menu component

    void Start()
    {
        _menu = GetComponent<TraversalMenu>();
        _levelData.SetMenuInstance(this);
    }

    public void ButtonStartGameAction()
    {
        StartCoroutine(StartGameSequenceCoroutine());
    }

    public void ButtonStartTutorialAction()
    {
        StartCoroutine(StartTutorialSequenceCoroutine());
    }

    private GameObject SpawnLevel(string name)
    {
        foreach(GameObject level in _loadedLevels)  // If level already exists in memory, return reference
        {
            if(level.name == name)
            {
                return level;
            }
        }

        // Else, 
        GameObject baseRef = null;
        foreach(GameObject level in Levels) // Get the prefab whose name matches the desired name
        {
            if(level.name == name)
            {
                baseRef = level;
                break;
            }
        }

        // Create and return the generated level
        GameObject newLevel = Instantiate(baseRef);
        _loadedLevels.Add(newLevel);
        return newLevel;
    }

    private IEnumerator StartTutorialSequenceCoroutine()
    {
        if (_lastLevel != null) // Do not unload level if level wasn't loaded previously
        {
            _levelData.UnloadPreviousLevel();

            yield return new WaitForSeconds(ExitTime);  // Wait for unloading animation to end

            _lastLevel.SetActive(false);    // Deactivate unloaded level
        }

        string newLevelName = TutorialLevel.name;   // Get new level name
        _lastLevel = SpawnLevel(newLevelName);  // Get reference for loading level

        _lastLevel.SetActive(true);
        _levelData.SetLevelInstance(_lastLevel.GetComponent<IGameState>());     // Set levelData's reference to the loaded level
        _levelData.StartGame();         // Start game
    }

    private IEnumerator StartGameSequenceCoroutine()
    {
        if (_lastLevel != null)  // Do not unload level if level wasn't loaded previously
        {
            _levelData.UnloadPreviousLevel();

            yield return new WaitForSeconds(ExitTime);  // Wait for unloading animation to end

            _lastLevel.SetActive(false);    // Deactivate unloaded level
        }

        string newLevelName = Levels[Random.Range(0, Levels.Length)].name;  // Get new level name
        _lastLevel = SpawnLevel(newLevelName);  // Get reference for loading level

        _lastLevel.SetActive(true);
        _levelData.SetLevelInstance(_lastLevel.GetComponent<IGameState>());         // Set levelData's reference to the loaded level
        _levelData.StartGame();                 // Start game
    }

    void IGameState.EndGame()
    {
        _menu.GoToMain();
    }

    void IGameState.StartGame()
    {
        // Should not be called here as to avoid infinite loops
    }
}
