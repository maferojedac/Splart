using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "custom/playerdata")]
public class PlayerData : ScriptableObject
{
    public int Money;
    public int MaxScore;

    public int LastMoneyBatch;

    // Boosters que se pueden comprar
    public int BoosterSlow;     // reloj ralentizador
    public int BoosterThunder;      // rayo
    public int BoosterClean;     // jabon
    public int BoosterLife;     // vidas extra

    // Mejoras permanentes
    public int Booster_AnyUpgrade;      // autocromatic
    public int Booster_ScoreMultiplier;    // multiplicador puntos

    // Configuracion del juego
    public float MasterVolume;
    public float SoundeffectsVolume;
    public float MusicVolume;

    private List<IPlayerDataEvent> _listenerObjects = new(); // Listeners to player events

    private void OnEnable()
    {
        LoadData();
    }

    public float GetMoneyMultiplier()
    {
        if (Booster_ScoreMultiplier == 0)
            return 1f;
        else if (Booster_ScoreMultiplier == 1)
            return 1.25f;
        else if (Booster_ScoreMultiplier == 2)
            return 1.40f;
        else
            return 1.55f;
    }

    public void SubscribeToEvents(IPlayerDataEvent listener)
    {
        _listenerObjects.Add(listener);
    }

    public void SumMoney(int money)
    {
        this.Money += money;
        foreach (ILevelEvent listener in _listenerObjects)
        {
            listener.UpdateMoney();
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Money"                  , Money                 );
        PlayerPrefs.SetInt("MaxScore"               , MaxScore              );

        PlayerPrefs.SetInt("BoosterSlow"            , BoosterSlow           );
        PlayerPrefs.SetInt("BoosterThunder"         , BoosterThunder        );
        PlayerPrefs.SetInt("BoosterClean"           , BoosterClean          );
        PlayerPrefs.SetInt("BoosterLife"            , BoosterLife           );

        PlayerPrefs.SetInt("Booster_AnyUpgrade"     , Booster_AnyUpgrade    );
        PlayerPrefs.SetInt("Booster_ScoreUpgrade"   , Booster_ScoreMultiplier);

        PlayerPrefs.SetFloat("MasterVolume"         , MasterVolume          );
        PlayerPrefs.SetFloat("SoundeffectsVolume"   , SoundeffectsVolume    );
        PlayerPrefs.SetFloat("MusicVolume"          , MusicVolume           );
    }

    public void LoadData()
    {
        Money                 = PlayerPrefs.GetInt("Money");
        MaxScore              = PlayerPrefs.GetInt("MaxScore");

        BoosterSlow           = PlayerPrefs.GetInt("BoosterSlow");
        BoosterThunder        = PlayerPrefs.GetInt("BoosterThunder");
        BoosterClean          = PlayerPrefs.GetInt("BoosterClean");
        BoosterLife           = PlayerPrefs.GetInt("BoosterLife");

        Booster_AnyUpgrade    = PlayerPrefs.GetInt("Booster_AnyUpgrade");
        Booster_ScoreMultiplier = PlayerPrefs.GetInt("Booster_ScoreUpgrade");

        MasterVolume          = PlayerPrefs.GetFloat("MasterVolume");
        SoundeffectsVolume    = PlayerPrefs.GetFloat("SoundeffectsVolume");
        MusicVolume           = PlayerPrefs.GetFloat("MusicVolume");
    }

}
