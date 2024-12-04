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
    public Image progressBar;

    [NonSerialized] public LevelData _levelData;
    [NonSerialized] public GameState _gameState;    // Game communication
    [NonSerialized] public PlayerData _playerData;

    private int _oldColorCount;

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

        UpdateScore();
        _oldColorCount = 0;
        progressBar.fillAmount = 0;
    }

    void IGameState.EndGame()
    {
        player._isActive = false;
        pauseMenu.SlideOut();
        GameCanvas.SlideOut();
    }

    public void UpdateScore()
    {
        if (_levelData._gameRunning)
            _scoreText.text = $"{_levelData.GetScore()}";
        else
            _scoreText.text = "0";
    }

    public void PaintObject()
    {
        float newValue = _levelData._currentColorCount * 1f / _levelData._maxColorCount;
        float oldValue = _oldColorCount * 1f / _levelData._maxColorCount;

        StartCoroutine(AnimateProgressBar(oldValue, newValue));
    }

    IEnumerator AnimateProgressBar(float oldValue, float newValue)
    {
        float timer = 0f;
        float distance = (newValue - oldValue) / 2;

        progressBar.fillAmount = oldValue;

        while (timer < 1f)
        {
            // Map timer to PI
            float progress = Mathf.Lerp(-Mathf.PI / 2, Mathf.PI / 2, timer);
            // Get smoothed sine
            progress = Mathf.Sin(progress);
            // Remap from (-1)-(1) to (0)-(1)
            progress = (progress * distance) + (distance) + oldValue;

            progressBar.fillAmount = progress;

            timer += Time.deltaTime;
            yield return null;
        }

        _oldColorCount = _levelData._currentColorCount;
        progressBar.fillAmount = newValue;
    }

    private float SmoothProgress(float progress)
    {
        // Map timer to PI
        progress = Mathf.Lerp(-Mathf.PI / 2, Mathf.PI / 2, progress);
        // Get smoothed sine
        progress = Mathf.Sin(progress);
        // Remap from (-1)-(1) to (0)-(1)
        progress = (progress / 2f) + 0.5f;

        return progress;
    }
}
