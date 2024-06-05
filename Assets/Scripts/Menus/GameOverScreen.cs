using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public int TestScore;

    [Header("Screen elements")]
    [SerializeField] private TextMeshProUGUI _scoreText;
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

    private int _oldScore;
    private float _timer;

    void Start()
    {
        _displayPosition = Vector3.zero;
        _exitPosition = Vector3.zero + _exitOffset;
    }

    public void Invoke()
    {
        if (!_isMenuIn)
        {
            gameObject.SetActive(true);
            _playerData.Money += _levelData.GetScore();
            _playerData.SaveData();

            _homeButton.interactable = false;

            StartCoroutine(SlideIn());
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

    IEnumerator Animate()
    {
        _timer = 0f;
        _oldScore = _levelData.GetScore();

        while(_timer < 1f)
        {
            _timer += Time.deltaTime;

            _scoreText.text = $"{Mathf.RoundToInt(Mathf.SmoothStep(_oldScore, 0f, _timer))}";
            _moneyText.text = $"+{Mathf.RoundToInt(Mathf.SmoothStep(0f, _oldScore, _timer))}";
            _totalMoneyText.text = $"{Mathf.RoundToInt(Mathf.SmoothStep(_playerData.Money - _oldScore, _playerData.Money, _timer))}";

            yield return null;
        }

        _scoreText.text = "0";
        _moneyText.text = $"+{_oldScore}";
        _totalMoneyText.text = $"{_playerData.Money}";

        _homeButton.interactable = true;
    }

    IEnumerator SlideIn()
    {
        _timer = 0f;

        while (_timer < 1f)
        {
            _timer += Time.deltaTime * 5f;

            transform.localPosition = Vector3.Slerp(_exitPosition, _displayPosition, _timer);

            yield return null;
        }

        transform.localPosition = _displayPosition;

        // yield return new WaitForSeconds(0.5f);

        StartCoroutine(Animate());

        _isMenuIn = true;
    }

    IEnumerator SlideOut()
    {
        _timer = 0f;

        while (_timer < 1f)
        {
            _timer += Time.deltaTime * 5f;
            
            transform.localPosition = Vector3.Slerp(_displayPosition, _exitPosition, _timer);

            yield return null;
        }

        transform.localPosition = _exitPosition;

        _isMenuIn = false;

        gameObject.SetActive(false);
    }
}
