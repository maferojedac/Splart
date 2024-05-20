using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "custom/playerdata")]
public class PlayerData : ScriptableObject
{
    public int Money;

    // Boosters que se pueden comprar
    public int BoosterSlow;     // reloj ralentizador
    public int BoosterThunder;      // rayo
    public int BoosterClean;     // jabon
    public int BoosterLife;     // vidas extra

    // Mejoras permanentes
    public int Booster_AnyUpgrade;      // autocromatic
    public int Booster_ScoreUpgrade;    // multiplicador puntos

    // Configuracion del juego
    public float MasterVolume;
    public float SoundeffectsVolume;
    public float MusicVolume;

    private void OnEnable()
    {
        LoadData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Money"                  , Money                 );

        PlayerPrefs.SetInt("BoosterSlow"            , BoosterSlow           );
        PlayerPrefs.SetInt("BoosterThunder"         , BoosterThunder        );
        PlayerPrefs.SetInt("BoosterClean"           , BoosterClean          );
        PlayerPrefs.SetInt("BoosterLife"            , BoosterLife           );

        PlayerPrefs.SetInt("Booster_AnyUpgrade"     , Booster_AnyUpgrade    );
        PlayerPrefs.SetInt("Booster_ScoreUpgrade"   , Booster_ScoreUpgrade  );

        PlayerPrefs.SetFloat("MasterVolume"         , MasterVolume          );
        PlayerPrefs.SetFloat("SoundeffectsVolume"   , SoundeffectsVolume    );
        PlayerPrefs.SetFloat("MusicVolume"          , MusicVolume           );
    }

    public void LoadData()
    {
        Money                 = PlayerPrefs.GetInt("Money");

        BoosterSlow           = PlayerPrefs.GetInt("BoosterSlow");
        BoosterThunder        = PlayerPrefs.GetInt("BoosterThunder");
        BoosterClean          = PlayerPrefs.GetInt("BoosterClean");
        BoosterLife           = PlayerPrefs.GetInt("BoosterLife");

        Booster_AnyUpgrade    = PlayerPrefs.GetInt("Booster_AnyUpgrade");
        Booster_ScoreUpgrade  = PlayerPrefs.GetInt("Booster_ScoreUpgrade");

        MasterVolume          = PlayerPrefs.GetFloat("MasterVolume");
        SoundeffectsVolume    = PlayerPrefs.GetFloat("SoundeffectsVolume");
        MusicVolume           = PlayerPrefs.GetFloat("MusicVolume");
    }

}
