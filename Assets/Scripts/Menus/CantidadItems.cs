using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CantidadItems : MonoBehaviour
{
    public TMP_Text BoosterSlowText;
    public TMP_Text BoosterThunderText;
    public TMP_Text BoosterCleanText;
    public TMP_Text BoosterLifeText;
    public TMP_Text Booster_AnyUpgradeText;
    public TMP_Text Booster_ScoreUpgradeText;

    public PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        BoosterSlowText.text = playerData.BoosterSlow.ToString();
        BoosterThunderText.text = playerData.BoosterThunder.ToString();
        BoosterCleanText.text = playerData.BoosterClean.ToString();
        BoosterLifeText.text = playerData.BoosterLife.ToString();
        Booster_AnyUpgradeText.text = playerData.Booster_AnyUpgrade.ToString();
        Booster_ScoreUpgradeText.text = playerData.Booster_ScoreMultiplier.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        BoosterSlowText.text = playerData.BoosterSlow.ToString();
        BoosterThunderText.text = playerData.BoosterThunder.ToString();
        BoosterCleanText.text = playerData.BoosterClean.ToString();
        BoosterLifeText.text = playerData.BoosterLife.ToString();
        Booster_AnyUpgradeText.text = playerData.Booster_AnyUpgrade.ToString();
        Booster_ScoreUpgradeText.text = playerData.Booster_ScoreMultiplier.ToString();
    }
}
