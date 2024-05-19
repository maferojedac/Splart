using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameState
{
    public Player player;
    public GameObject GameCanvas;
    public PauseMenu pauseMenu;

    public LevelData _levelData;

    void Start()
    {
        _levelData.SetBaseGameInstance(gameObject);
        pauseMenu.Vanish();
        GameCanvas.SetActive(false);
    }

    void IGameState.StartGame()
    {
        player.NewGame();
        pauseMenu.Vanish();
        GameCanvas.SetActive(true);
    }

    void IGameState.EndGame()
    {
        player._isActive = false;
        pauseMenu.SlideOut();
        GameCanvas.SetActive(false);
    }
}
