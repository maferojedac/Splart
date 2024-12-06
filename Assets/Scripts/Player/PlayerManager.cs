using System;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, IGameState, ILevelEvent
{
    [Header("Setup")]
    public Player player;

    [Header("UI Setup")]
    public SimpleMenuAnimation GameCanvas;
    public PauseMenu pauseMenu;
    public HeartDisplay heartDisplay;
    public GameOverScreen gameOverScreen;
    public TextMeshProUGUI _scoreText;
    public TextMeshProUGUI _moneyText;

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

    void IGameState.GameOver(bool Victory)
    {
        gameOverScreen.Invoke(Victory);
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

        UpdateScore();
        UpdateMoney();
    }

    void IGameState.EndGame()
    {
        player._isActive = false;
        pauseMenu.SlideOut();
        GameCanvas.SlideOut();
    }

    public void UpdateMoney()
    {
        _moneyText.text = $"{_playerData.Money + _levelData._currentAccumulatedMoney}";
    }

    public void UpdateScore()
    {
        if (_levelData._gameRunning)
            _scoreText.text = $"{_levelData.GetScore()}";
        else
            _scoreText.text = "0";
    }
}
