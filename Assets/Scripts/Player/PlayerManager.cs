using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameState
{
    public Player player;
    public SimpleMenuAnimation GameCanvas;
    public PauseMenu pauseMenu;

    public TextMeshProUGUI _scoreText;

    public LevelData _levelData;

    void Start()
    {
        _levelData.SetBaseGameInstance(gameObject);
        pauseMenu.Vanish();
        GameCanvas.Vanish();
    }

    void IGameState.StartGame()
    {
        player.NewGame();
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
        if (_levelData._gameRunning)
            _scoreText.text = $"{_levelData.GetScore()}";
        else
            _scoreText.text = "0";
    }
}
