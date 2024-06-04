using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostersShowing : MonoBehaviour
{
    public GameObject BoostSlow;
    public GameObject BoostThunder;
    public GameObject BoostClean;
    public GameObject BoostLifeShield;
    public GameObject BoostScore;

    public PlayerData _playerData;

    public Image _visualTimer;

    private bool _isSlowBooted = false;

    private float _counterTimer;
    private float _counterGoal;

    private bool _activeBooster;



    void Start()
    {
        if (_playerData.BoosterSlow <= 0) BoostSlow.SetActive(false);
        if (_playerData.BoosterThunder <= 0) BoostThunder.SetActive(false);
        if (_playerData.BoosterClean <= 0) BoostClean.SetActive(false);
        if (_playerData.BoosterLife <= 0) BoostLifeShield.SetActive(false);
        if (_playerData.Booster_ScoreUpgrade <= 0) BoostScore.SetActive(false);

        _visualTimer.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerData.BoosterSlow > 0) BoostSlow.SetActive(true); else BoostSlow.SetActive(false);
        if (_playerData.BoosterThunder > 0) BoostThunder.SetActive(true); else BoostThunder.SetActive(false);
        if (_playerData.BoosterClean > 0) BoostClean.SetActive(true); else BoostClean.SetActive(false);
        if (_playerData.BoosterLife > 0) BoostLifeShield.SetActive(true); else BoostLifeShield.SetActive(false);
        if (_playerData.Booster_ScoreUpgrade > 0) BoostScore.SetActive(true); else BoostScore.SetActive(false);

        if (_isSlowBooted)
        {
            
            _isSlowBooted = false;
            
        }
    }

    public void UsedBoosterSlow()
    {
        StartCoroutine(SlowBoost());

        _playerData.BoosterSlow--;
    }

    IEnumerator SlowBoost()
    {
        Debug.Log("Slow activated");

        _counterTimer = 0;
        _counterGoal = 10f;

        _visualTimer.fillAmount = 1f;

        List<GameObject> enemies = Entity.GetAll();

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                enemy.GetComponent<EnemyMovement>()?.SetSpeedMultiplier(0.2f);
        }

        while( _counterTimer < _counterGoal)
        {
            _counterTimer += Time.deltaTime;

            _visualTimer.fillAmount = 1 - (_counterTimer / _counterGoal);

            yield return null;
        }

        // yield return new WaitForSeconds(10.0f);

        foreach (GameObject enemy in enemies)
        {
            if(enemy != null)
                enemy.GetComponent<EnemyMovement>()?.SetSpeedMultiplier(1.0f);
        }

        _visualTimer.fillAmount = 0f;
    }

    public void UsedBoosterThunder()
    {
        Debug.Log("Thunder activated");

        List<GameObject> enemies = Entity.GetAll();

        foreach (GameObject enemy in enemies)
        {
            if (enemy.CompareTag("MageEnemy"))
            {
                enemy.GetComponent<IEnemy>().OnDie();
                _playerData.BoosterThunder--;
            }
                
        }
    }
}
