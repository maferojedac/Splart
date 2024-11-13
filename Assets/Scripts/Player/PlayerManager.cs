using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameState
{
    public Player player;
    public SimpleMenuAnimation GameCanvas;
    public PauseMenu pauseMenu;
    public HeartDisplay heartDisplay;
    public GameOverScreen gameOverScreen;

    public TextMeshProUGUI _scoreText;

    public LevelData _levelData;

    void Start()
    {
        _levelData.SetBaseGameInstance(this);
        pauseMenu.Vanish();
        GameCanvas.Vanish();
        gameOverScreen.Vanish();
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

    void Update()
    {
        if (_levelData._gameRunning)    // weed eater
            _scoreText.text = $"{_levelData.GetScore()}";
        else
            _scoreText.text = "0";
    }
}
