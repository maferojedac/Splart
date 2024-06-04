using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreBuy : MonoBehaviour
{
    public Button BoosterSlow;
    public Button BoosterThunder;
    public Button BoosterClean;
    public Button BoosterLife;
    public Button Booster_AnyUpgrade;
    public Button Booster_ScoreUpgrade;

    public PlayerData playerData;

    void Start()
    {
        BoosterSlow.onClick.AddListener(BoosterSlowChanged);
        BoosterThunder.onClick.AddListener(BoosterThunderChanged);
        BoosterClean.onClick.AddListener(BoosterCleanChanged);
        BoosterLife.onClick.AddListener(BoosterLifeChanged);
        Booster_AnyUpgrade.onClick.AddListener(Booster_AnyUpgradeChanged);
        Booster_ScoreUpgrade.onClick.AddListener(Booster_ScoreUpgradeChanged);
    }

    public void BoosterSlowChanged()
    {
        if (playerData.Money > 0)
        {
            playerData.BoosterSlow++;
            playerData.Money -= 100;
        }
    }

    public void BoosterThunderChanged()
    {
        if (playerData.Money > 0)
        {
            playerData.BoosterThunder++;
            playerData.Money -= 100;
        }
    }

    public void BoosterCleanChanged()
    {
        if (playerData.Money > 0)
        {
            playerData.BoosterClean++;
            playerData.Money -= 100;
        }
    }

    public void BoosterLifeChanged()
    {
        if (playerData.Money > 0)
        {
            playerData.BoosterLife++;
            playerData.Money -= 100;
        }
    }

    public void Booster_AnyUpgradeChanged()
    {
        if (playerData.Money > 0)
        {
            playerData.Booster_AnyUpgrade++;
            playerData.Money -= 100;
        }
    }

    public void Booster_ScoreUpgradeChanged()
    {
        if (playerData.Money > 0)
        {
            playerData.Booster_ScoreUpgrade++;
            playerData.Money -= 100;
        }
    }
}
