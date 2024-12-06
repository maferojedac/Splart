using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public int TestScore;

    [Header("Level progress elements")]
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _defeatedEnemies;

    [Header("Level performance and rewards elements")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _maxScoreText;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _totalMoneyText;
    [SerializeField] private Button _homeButton;

    [Header("Game elements")]
    [SerializeField] private LevelData _levelData;
    [SerializeField] private PlayerData _playerData;

    [Header("Menu behavior")]
    [SerializeField] private Vector3 _exitOffset;

    private bool _isMenuIn;

    private Vector3 _exitPosition;
    private Vector3 _displayPosition;

    void Start()
    {
        _displayPosition = Vector3.zero;
        _exitPosition = Vector3.zero + _exitOffset;
    }

    public void Invoke(bool Victory)
    {
        if (!_isMenuIn)
        {
            gameObject.SetActive(true);

            _homeButton.interactable = false;

            ResetDisplays(Victory);
            StartCoroutine(SlideIn(Victory));
        }
    }

    public void Vanish()
    {
        transform.position = _exitPosition;
        gameObject.SetActive(false);
    }

    public void EndGameButton()
    {
        _levelData.EndGame();
        StartCoroutine(SlideOut());
    }

    IEnumerator AnimatePerformance()
    {
        float timer = 0f;
        int score = _levelData.GetScore();

        

        while (timer < 1f)
        {
            timer += Time.deltaTime;

            _scoreText.text = $"{Mathf.RoundToInt(Mathf.SmoothStep(0f, score, timer))}";

            _moneyText.text = $"+{Mathf.RoundToInt(Mathf.SmoothStep(0f, _playerData.LastMoneyBatch, timer))}";
            _totalMoneyText.text = $"{Mathf.RoundToInt(Mathf.SmoothStep(_playerData.Money - _playerData.LastMoneyBatch, _playerData.Money, timer))}";

            yield return null;
        }

        _scoreText.text = $"{score}";
        _moneyText.text = $"+{_playerData.LastMoneyBatch}";
        _totalMoneyText.text = $"{_playerData.Money}";

        _homeButton.interactable = true;
    }

    IEnumerator AnimateProgressWin()
    {
        // Get progress
        float levelProgress = 1f;
        int defeatedEnemies = _levelData._enemiesDefeatedCount;

        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime;

            _progressBar.fillAmount = SmoothProgress(timer, levelProgress);
            _defeatedEnemies.text = $"{Mathf.RoundToInt(Mathf.SmoothStep(0f, defeatedEnemies, timer))}";

            yield return null;
        }

        _progressBar.fillAmount = levelProgress;
        _defeatedEnemies.text = $"{Mathf.RoundToInt(Mathf.SmoothStep(0f, defeatedEnemies, timer))}";

        StartCoroutine(AnimatePerformance());
    }

    IEnumerator AnimateProgressLose()
    {

        // Get progress
        float levelProgress = _levelData._currentColorCount * 1f / _levelData._maxColorCount;
        if (levelProgress >= 1f)    // If boss wasn't defeated
            levelProgress = 0.9f;

        int defeatedEnemies = _levelData._enemiesDefeatedCount;

        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime;

            _progressBar.fillAmount = SmoothProgress(timer, levelProgress);
            _defeatedEnemies.text = $"{Mathf.RoundToInt(Mathf.SmoothStep(0f, defeatedEnemies, timer))}";

            yield return null;
        }

        _progressBar.fillAmount = levelProgress;
        _defeatedEnemies.text = $"{Mathf.RoundToInt(Mathf.SmoothStep(0f, defeatedEnemies, timer))}";

        StartCoroutine(AnimatePerformance());
    }

    private void ResetDisplays(bool Victory)
    {
        if(Victory)
            _title.text = $"¡Has pintado a {_levelData._levelName}!";
        else
            _title.text = "Fin del juego";

        _maxScoreText.text = $"Puntuacion Max. {_playerData.MaxScore}";

        _progressBar.fillAmount = 0f;
        _defeatedEnemies.text = "0";

        _scoreText.text = "0";
        _moneyText.text = "+0"; 
        _totalMoneyText.text = "0";
    }

    private float SmoothProgress(float progress, float value)
    {
        // Map timer to PI
        progress = Mathf.Lerp(-Mathf.PI / 2, Mathf.PI / 2, progress);
        // Get smoothed sine
        progress = Mathf.Sin(progress);
        // Remap from (-1)-(1) to (0)-(1)
        progress = (progress / 2f) + 0.5f;

        progress *= value;

        return progress;
    }

    IEnumerator SlideIn(bool Victory)
    {
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime * 5f;

            transform.localPosition = Vector3.Slerp(_exitPosition, _displayPosition, timer);

            yield return null;
        }

        transform.localPosition = _displayPosition;

        // yield return new WaitForSeconds(0.5f);

        if(Victory)
            StartCoroutine(AnimateProgressWin());
        else
            StartCoroutine(AnimateProgressLose());

        _isMenuIn = true;
    }

    IEnumerator SlideOut()
    {
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime * 5f;
            
            transform.localPosition = Vector3.Slerp(_displayPosition, _exitPosition, timer);

            yield return null;
        }

        transform.localPosition = _exitPosition;

        _isMenuIn = false;

        gameObject.SetActive(false);
    }
}
