// Class to provide communication to all IGameState objects.

// Created by Javier Soto

using UnityEngine;

[CreateAssetMenu(menuName = "Communication/Game State Asset")]
public class GameState : ScriptableObject
{
    public IGameState _levelInstance;       // Reference to current loaded level
    private IGameState _menusInstance;      // Reference to menu instance
    private IGameState _baseGameInstance;   // Reference to base game instance

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

    public void SetLevelInstance(IGameState p_object)
    {
        _levelInstance = p_object;
    }

    public void NextWave()  // Communicate next wave to all that apply
    {
        _levelInstance.NextWave();
        _menusInstance.NextWave();
        _baseGameInstance.NextWave();
    }

    public void GameOver()
    {
        _levelInstance.GameOver();
        _menusInstance.GameOver();
        _baseGameInstance.GameOver();
    }

    public void EndGame()
    {
        _levelInstance.EndGame();
        _menusInstance.EndGame();
        _baseGameInstance.EndGame();
    }

    public void StartGame()
    {
        _levelInstance.StartGame();
        _menusInstance.StartGame();
        _baseGameInstance.StartGame();
    }
}
