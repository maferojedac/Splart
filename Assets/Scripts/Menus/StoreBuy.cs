using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreBuy : MonoBehaviour
{
    [Header("Precios")]
    public int BoosterSlowPrice;
    public int BoosterThunderPrice;
    public int BoosterCleanPrice;
    public int BoosterLifePrice;
    public int Booster_AnyUpgradePrice;
    public int Booster_ScoreUpgradePrice;

    [Header("Botones de compra")]
    public TextMeshProUGUI BoosterSlow;
    public TextMeshProUGUI BoosterThunder;
    public TextMeshProUGUI BoosterClean;
    public TextMeshProUGUI BoosterLife;
    public TextMeshProUGUI Booster_AnyUpgrade;
    public TextMeshProUGUI Booster_ScoreUpgrade;

    public PlayerData playerData;

    void Start()
    {
        BoosterSlow.text = BoosterSlowPrice.ToString();
        BoosterThunder.text = BoosterThunderPrice.ToString();
        BoosterClean.text = BoosterCleanPrice.ToString();
        BoosterLife.text = BoosterLifePrice.ToString();
        Booster_AnyUpgrade.text = Booster_AnyUpgradePrice.ToString();
        Booster_ScoreUpgrade.text = Booster_ScoreUpgradePrice.ToString();
    }

    public void BoosterSlowBuy()
    {
        if (playerData.Money >= BoosterSlowPrice)
        {
            playerData.BoosterSlow++;
            playerData.Money -= BoosterSlowPrice;
        }
    }

    public void BoosterThunderBuy()
    {
        if (playerData.Money >= BoosterThunderPrice)
        {
            playerData.BoosterThunder++;
            playerData.Money -= BoosterThunderPrice;
        }
    }

    public void BoosterCleanBuy()
    {
        if (playerData.Money >= BoosterCleanPrice)
        {
            playerData.BoosterClean++;
            playerData.Money -= BoosterCleanPrice;
        }
    }

    public void BoosterLifeBuy()
    {
        if (playerData.Money >= BoosterLifePrice)
        {
            playerData.BoosterLife++;
            playerData.Money -= BoosterLifePrice;
        }
    }
    
    public void Booster_AnyUpgradeBuy()
    {
        if (playerData.Money >= Booster_AnyUpgradePrice)
        {
            playerData.Booster_AnyUpgrade++;
            playerData.Money -= Booster_AnyUpgradePrice;
        }
    }

    public void Booster_ScoreUpgradeBuy()
    {
        if (playerData.Money >= Booster_ScoreUpgradePrice)
        {
            playerData.Booster_ScoreUpgrade++;
            playerData.Money -= Booster_ScoreUpgradePrice;
        }
    }
}
