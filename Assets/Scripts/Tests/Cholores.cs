using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cholores : MonoBehaviour
{

    ArrayColor clr = new ArrayColor();
    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddRed()
    {
        clr.Add(GameColor.Red);
    }

    public void AddYellow()
    {
        clr.Add(GameColor.Yellow);
    }

    public void AddBlue()
    {
        clr.Add(GameColor.Blue);
    }

    public void ClearColors()
    {
        clr.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        img.color = clr.toRGB();
    }
}
