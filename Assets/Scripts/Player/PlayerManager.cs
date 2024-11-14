using System;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameState, ILevelEvent
{
    public Player player;
    public SimpleMenuAnimation GameCanvas;
    public PauseMenu pauseMenu;
    public HeartDisplay heartDisplay;
    public GameOverScreen gameOverScreen;

    public TextMeshProUGUI _scoreText;

    [NonSerialized] public LevelData _levelData;
    [NonSerialized] public GameState _gameState;    // Game communication
    [NonSerialized] public PlayerData _playerData;    

    void Awake()
    {
        CommunicationPrefabScript communicator = GameObject.Find("CommunicationPrefab").GetComponent<CommunicationPrefabScript>();
        _levelData = communicator._levelData;
        _gameState = communicator._gameState;
        _playerData = communicator._playerData;

        _gameState.SetBaseGameInstance(this);
        _levelData.SubscribeToEvents(this);

        pauseMenu.Vanish(); // ram eater
        GameCanvas.Vanish();    // weed eater
        gameOverScreen.Vanish();    // ouch eater
    }

    void IGameState.GameOver()
    {
        gameOverScreen.Invoke();
        pauseMenu.SlideOut();
        GameCanvas.SlideOut();
    }

    void IGameState.StartGame()
    {
        gameOverScreen.Vanish();
        player.NewGame();
        heartDisplay.NewGame();
        pauseMenu.Vanish();
        GameCanvas.SlideIn();
    }

    void IGameState.EndGame()
    {
        player._isActive = false;
        pauseMenu.SlideOut();
        GameCanvas.SlideOut();
    }

    void ILevelEvent.UpdateScore()
    {
        if (_levelData._gameRunning)
            _scoreText.text = $"{_levelData.GetScore()}";
        else
            _scoreText.text = "0";
    }
}
