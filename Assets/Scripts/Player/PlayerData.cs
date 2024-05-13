using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SaveData()
    {

    }

    public void LoadData()
    {

    }

}
