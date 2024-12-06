// Class to provide communication to all IGameState objects.

// Created by Javier Soto

using UnityEngine;

[CreateAssetMenu(menuName = "Communication/Game State Asset")]
public class GameState : ScriptableObject
{
    public PlayerData _playerData;
    public LevelData _levelData;

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

    public void GameOver(bool Victory)
    {
        _levelInstance.GameOver(Victory);
        _menusInstance.GameOver(Victory);
        _baseGameInstance.GameOver(Victory);

        // Apply money changes!
        if(_levelData.GetScore() > _playerData.MaxScore)
            _playerData.MaxScore = _levelData.GetScore();

        int MoneyBatch = Mathf.RoundToInt(_levelData.GetScore() * _playerData.GetMoneyMultiplier());
        _playerData.LastMoneyBatch = MoneyBatch;
        _playerData.Money += MoneyBatch;
        _playerData.SaveData();
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
