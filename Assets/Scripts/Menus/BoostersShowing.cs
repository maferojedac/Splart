using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostersShowing : MonoBehaviour
{
    public GameObject BoostSlow;
    public GameObject BoostThunder;
    public GameObject BoostClean;
    public GameObject BoostLifeShield;
    public GameObject BoostScore;
    public PlayerData _playerData;

    private bool _isSlowBooted = false;

    void Start()
    {
        if (_playerData.BoosterSlow <= 0) BoostSlow.SetActive(false);
        if (_playerData.BoosterThunder <= 0) BoostThunder.SetActive(false);
        if (_playerData.BoosterClean <= 0) BoostClean.SetActive(false);
        if (_playerData.BoosterLife <= 0) BoostLifeShield.SetActive(false);
        if (_playerData.Booster_ScoreUpgrade <= 0) BoostScore.SetActive(false);
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
            StartCoroutine(SlowBoost());
            _isSlowBooted = false;
            
        }
    }

    public void UsedBoosterSlow()
    {
        _isSlowBooted = true;
        
        _playerData.BoosterSlow--;
    }

    IEnumerator SlowBoost()
    {
        Debug.Log("SlowBoost");
        GameObject father = GameObject.Find("TestLevel(Clone)");
        Transform son = father.transform.Find("Enemies");

        foreach (Transform enemy in son)
        {
            enemy.GetComponent<EnemyMovement>().SetSpeedMultiplier(0.2f);
        }

        yield return new WaitForSeconds(10.0f);

        foreach (Transform enemy in son)
        {
            enemy.GetComponent<EnemyMovement>().SetSpeedMultiplier(1.0f);
        }
    }

    public void UsedBoosterThunder()
    {
        GameObject father = GameObject.Find("TestLevel(Clone)");
        Transform son = father.transform.Find("Enemies");

        foreach (Transform enemy in son)
        {
            if ((enemy.name == "Mage_Flashbang(Clone)") || (enemy.name == "Mage_Multicolor(Clone)"))
            {
                Destroy(enemy.gameObject);
                _playerData.BoosterThunder--;
            }
                
        }
    }
}
