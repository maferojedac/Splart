using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostersShowing : MonoBehaviour
{
    [Header("Booster buttons")]
    public Button BoostSlow;
    public Button BoostThunder;
    public Button BoostClean;
    public Button BoostLifeShield;
    public Button BoostScore;

    [Header("Data")]
    public PlayerData _playerData;
    public LevelData _levelData;

    [Header("Visuals and effects")]
    public Image _visualTimer;
    public GameObject _pressedSparkles;
    public GameObject _darkenEffect;
    public GameObject _thunderObject;

    private float _counterTimer;
    private float _counterGoal;

    private bool _activeBooster;

    private Transform _lastPressedButton;

    void Start()
    {
        if (_playerData.BoosterSlow <= 0) BoostSlow             .interactable = false;
        if (_playerData.BoosterThunder <= 0) BoostThunder       .interactable = false;
        if (_playerData.BoosterClean <= 0) BoostClean           .interactable = false;
        if (_playerData.BoosterLife <= 0) BoostLifeShield       .interactable = false;
        if (_playerData.Booster_ScoreUpgrade <= 0) BoostScore   .interactable = false;

        _visualTimer.fillAmount = 0f;
        _pressedSparkles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerData.BoosterSlow > 0 && !_activeBooster)             BoostSlow       .interactable = true; else BoostSlow        .interactable = false;
        if (_playerData.BoosterThunder > 0 && !_activeBooster)          BoostThunder    .interactable = true; else BoostThunder     .interactable = false;
        if (_playerData.BoosterClean > 0 && !_activeBooster)            BoostClean      .interactable = true; else BoostClean       .interactable = false;
        if (_playerData.BoosterLife > 0 && !_activeBooster)             BoostLifeShield .interactable = true; else BoostLifeShield  .interactable = false;
        if (_playerData.Booster_ScoreUpgrade > 0 && !_activeBooster)    BoostScore      .interactable = true; else BoostScore       .interactable = false;
        if (_activeBooster)
        {
            _pressedSparkles.SetActive(true);
            _lastPressedButton.localScale = Vector3.one * 0.5f;
            _pressedSparkles.transform.position = _lastPressedButton.transform.position;
        }
        else
        {
            if(_lastPressedButton != null)
            {
                _lastPressedButton.localScale = Vector3.one;
            }
            _pressedSparkles.SetActive(false);
        }
    }

    public void UsedBoosterSlow()
    {
        StartCoroutine(SlowBoost());
        _playerData.BoosterSlow--;
        _lastPressedButton = BoostSlow.transform;
    }

    IEnumerator SlowBoost()
    {
        Debug.Log("Slow activated");

        _activeBooster = true;

        _counterTimer = 0;
        _counterGoal = 10f;

        _visualTimer.fillAmount = 1f;

        List<GameObject> enemies = Entity.GetAll();

        _levelData.SetGlobalSpeedMultiplier(0.2f);

        while( _counterTimer < _counterGoal)
        {
            _counterTimer += Time.deltaTime;

            _visualTimer.fillAmount = 1 - (_counterTimer / _counterGoal);

            yield return null;
        }

        // yield return new WaitForSeconds(10.0f);

        _levelData.SetGlobalSpeedMultiplier(1f);

        _visualTimer.fillAmount = 0f;
        _activeBooster = false;
    }

    IEnumerator ThunderBoost()
    {
        _activeBooster = true;

        _counterTimer = 0;
        _counterGoal = 1.5f;

        _visualTimer.fillAmount = 1f;

        while (_counterTimer < _counterGoal)
        {
            _counterTimer += Time.deltaTime;

            _visualTimer.fillAmount = 1 - (_counterTimer / _counterGoal);

            yield return null;
        }

        _visualTimer.fillAmount = 0f;
        _activeBooster = false;
    }

    public void UsedBoosterThunder()
    {
        Debug.Log("Thunder activated");

        _lastPressedButton = BoostThunder.transform;
        _playerData.BoosterThunder--;
        StartCoroutine(ThunderBoost());

        List<GameObject> enemies = Entity.GetAll();
        Instantiate(_darkenEffect);

        foreach (GameObject enemy in enemies)
        {
            GameObject newThunder = Instantiate(_thunderObject);
            newThunder.GetComponent<Thunder>()._target = enemy;
        }
    }

    IEnumerator CleanBoost()
    {
        _activeBooster = true;

        _counterTimer = 0;
        _counterGoal = 0.5f;

        _visualTimer.fillAmount = 1f;

        while (_counterTimer < _counterGoal)
        {
            _counterTimer += Time.deltaTime;

            _visualTimer.fillAmount = 1 - (_counterTimer / _counterGoal);

            yield return null;
        }

        _visualTimer.fillAmount = 0f;
        _activeBooster = false;
    }

    public void UsedBoosterClean()
    {
        Debug.Log("Clean activated");

        _lastPressedButton = BoostClean.transform;
        _playerData.BoosterClean--;
        StartCoroutine(CleanBoost());

        List<GameObject> enemies = Entity.GetAll();

        foreach (GameObject enemy in enemies)
        {
            if (enemy.CompareTag("ScreenSplat"))
            {
                enemy.GetComponent<Splat>().Remove() ;
            }
        }
    }
}
