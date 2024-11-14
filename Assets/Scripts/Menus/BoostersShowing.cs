// Script that manages booster buttons
// This should not be attached to anything as it's managed by GameBase prefab, which is already set-up
// Anidation level to GameBase prefab must be 2 to reference Scriptable Objects properly.

// Created by Javier Soto

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

    private PlayerData _playerData;
    private LevelData _levelData;

    [Header("Visuals and effects")]
    public Image _visualTimer;
    public GameObject _pressedSparkles;
    public GameObject _darkenEffect;
    public GameObject _thunderObject;

    private float _counterTimer;
    private float _counterGoal;

    private bool _activeBooster;

    private Transform _lastPressedButton;

    void Awake()
    {
        _levelData = transform.parent.parent.GetComponent<PlayerManager>()._levelData;  // Get Scriptable object references from parent to 2
        _playerData = transform.parent.parent.GetComponent<PlayerManager>()._playerData;
    }

    void OnEnable()
    {
        DeactivateButtons();    // Reset buttons for current boosters
        ActivateButtons();

        _visualTimer.fillAmount = 0f;
        _pressedSparkles.SetActive(false);
    }

    public void UsedBoosterSlow()
    {
        _playerData.BoosterSlow--;
        _lastPressedButton = BoostSlow.transform;

        StartCoroutine(SlowBoost());
    }

    public void UsedBoosterThunder()
    {
        _playerData.BoosterThunder--;
        _lastPressedButton = BoostThunder.transform;

        StartCoroutine(ThunderBoost());

        // Booster effect
        List<GameObject> enemies = Entity.GetAll();
        // Instantiate(_darkenEffect);  // VFX POOLING!!

        foreach (GameObject enemy in enemies)
        {
            GameObject newThunder = Instantiate(_thunderObject);    // FUCKKKKK
            newThunder.GetComponent<Thunder>()._target = enemy;
        }
    }

    public void UsedBoosterClean()
    {
        _playerData.BoosterClean--;
        _lastPressedButton = BoostClean.transform;

        StartCoroutine(CleanBoost());

        // Booster effect
        List<GameObject> enemies = Entity.GetAll();

        foreach (GameObject enemy in enemies)   // Fix splat ples
        {
            if (enemy.CompareTag("ScreenSplat"))
            {
                enemy.GetComponent<Splat>().Remove();
            }
        }
    }

    IEnumerator SlowBoost()
    {
        HoldButton();

        _counterTimer = 0;
        _counterGoal = 10f;

        _visualTimer.fillAmount = 1f;

        List<GameObject> enemies = Entity.GetAll();

        _levelData.SetGlobalSpeedMultiplier(0.2f);  // Change enemy speed

        while( _counterTimer < _counterGoal)    // Small timer for visual Timer
        {
            _counterTimer += Time.deltaTime;

            _visualTimer.fillAmount = 1 - (_counterTimer / _counterGoal);

            yield return null;
        }

        _levelData.SetGlobalSpeedMultiplier(1f);    // Change enemy speed

        ReleaseButton();
    }

    IEnumerator ThunderBoost()
    {
        HoldButton();

        _counterTimer = 0;
        _counterGoal = 1.5f;

        _visualTimer.fillAmount = 1f;

        while (_counterTimer < _counterGoal)    // Small timer for visual Timer
        {
            _counterTimer += Time.deltaTime;

            _visualTimer.fillAmount = 1 - (_counterTimer / _counterGoal);

            yield return null;
        }

        ReleaseButton();
    }

    IEnumerator CleanBoost()
    {
        HoldButton();

        _counterTimer = 0;
        _counterGoal = 0.5f;

        _visualTimer.fillAmount = 1f;

        while (_counterTimer < _counterGoal)    // Small timer for visual Timer
        {
            _counterTimer += Time.deltaTime;

            _visualTimer.fillAmount = 1 - (_counterTimer / _counterGoal);

            yield return null;
        }

        ReleaseButton();
    }

    void DeactivateButtons()
    {
        BoostSlow.interactable = false;
        BoostThunder.interactable = false;
        BoostClean.interactable = false;
        BoostLifeShield.interactable = false;
        BoostScore.interactable = false;
    }

    void ActivateButtons()
    {
        if (_playerData.BoosterSlow > 0) BoostSlow.interactable = true;
        if (_playerData.BoosterThunder > 0) BoostThunder.interactable = true;
        if (_playerData.BoosterClean > 0) BoostClean.interactable = true;
        if (_playerData.BoosterLife > 0) BoostLifeShield.interactable = true;
        if (_playerData.Booster_ScoreUpgrade > 0) BoostScore.interactable = true;
    }

    void HoldButton()   // Effects of a held button
    {
        _activeBooster = true;

        _lastPressedButton.localScale = Vector3.one * 0.5f; // Show button at half size

        // Show sparkles where button was pressed
        _pressedSparkles.transform.position = _lastPressedButton.transform.position;
        _pressedSparkles.SetActive(true);

        DeactivateButtons(); // Deactivate other buttons
    }

    void ReleaseButton()    // Effects of releasing a button
    {
        _activeBooster = false;

        _visualTimer.fillAmount = 0f;

        if (_lastPressedButton != null)
            _lastPressedButton.localScale = Vector3.one;    // Return button to normal size

        _pressedSparkles.SetActive(false);  // Hide sparkles

        ActivateButtons(); // Activate other buttons
    }
}
