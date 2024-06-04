using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DineroJugador : MonoBehaviour
{
    public TMP_Text dineroJugador; 
    public PlayerData playerData;


    // Start is called before the first frame update
    void Start()
    {
        dineroJugador.text = playerData.Money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        dineroJugador.text = playerData.Money.ToString();
        if (playerData.Money == 0)
        {
            dineroJugador.text = "0";
        }
    }
}
