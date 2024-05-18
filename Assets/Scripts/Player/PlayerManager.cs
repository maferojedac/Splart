using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameState
{
    public GameObject Player;
    public GameObject GameCanvas;

    public LevelData _levelData;

    void Start()
    {
        _levelData.SetBaseGameInstance(gameObject);
        GameCanvas.SetActive(false);
    }

    void IGameState.StartGame()
    {
        Player.GetComponent<IPlayer>().NewGame();
        GameCanvas.SetActive(true);
    }

    void IGameState.EndGame()
    {
        GameCanvas.SetActive(false);
    }
}
