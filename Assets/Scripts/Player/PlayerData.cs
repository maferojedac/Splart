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

    public void SaveData()
    {

    }

    public void LoadData()
    {

    }

}
