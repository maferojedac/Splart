using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CantidadItems : MonoBehaviour
{
    public TMP_Text Producto1;
    public TMP_Text Producto2;
    public TMP_Text Producto3;
    public TMP_Text Producto4;
    public TMP_Text Producto5;
    public TMP_Text Producto6;

    public PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        Producto1.text = playerData.BoosterSlow.ToString();
        Producto2.text = playerData.BoosterThunder.ToString();
        Producto3.text = playerData.BoosterClean.ToString();
        Producto4.text = playerData.BoosterLife.ToString();
        Producto5.text = playerData.Booster_AnyUpgrade.ToString();
        Producto6.text = playerData.Booster_ScoreUpgrade.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Producto1.text = playerData.BoosterSlow.ToString();
        Producto2.text = playerData.BoosterThunder.ToString();
        Producto3.text = playerData.BoosterClean.ToString();
        Producto4.text = playerData.BoosterLife.ToString();
        Producto5.text = playerData.Booster_AnyUpgrade.ToString();
        Producto6.text = playerData.Booster_ScoreUpgrade.ToString();
    }
}
