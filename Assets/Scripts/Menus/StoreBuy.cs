using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        playerData.BoosterSlow++;
    }

    public void BoosterThunderChanged()
    {
        playerData.BoosterThunder++;
    }

    public void BoosterCleanChanged()
    {
        playerData.BoosterClean++;
    }

    public void BoosterLifeChanged()
    {
        playerData.BoosterLife++;
    }

    public void Booster_AnyUpgradeChanged()
    {
        playerData.Booster_AnyUpgrade++;
    }

    public void Booster_ScoreUpgradeChanged()
    {
        playerData.Booster_ScoreUpgrade++;
    }
}
