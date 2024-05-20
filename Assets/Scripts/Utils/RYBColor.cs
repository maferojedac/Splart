using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public struct RYBColor
{
    public float _red;
    public float _yellow;
    public float _blue;
    public float _alpha;

    // Borrowing code and palette from https://github.com/ProfJski/ArtColors/

    public RYBColor(Color input)
    {
        /*
        // RGB corners in RYB Values
        Vector3 CG000 = new( 0.2f, 0.2f, 0.2f ); //Black
        Vector3 CG100 = new( 0.891f, 0.0f, 0.0f ); //Red
        Vector3 CG010 = new( 0.0f, 0.714f, 0.374f ); //Green = RYB Yellow + Blue
        Vector3 CG001 = new( 0.07f, 0.08f, 0.893f ); //Blue:
        Vector3 CG011 = new( 0.0f, 0.116f, 0.313f ); //Cyan = RYB Green + Blue.  Very dark to make the rest of the function work correctly
        Vector3 CG110 = new( 0.0f, 0.915f, 0.0f ); //Yellow
        Vector3 CG101 = new( 0.554f, 0.0f, 0.1f ); //Magenta =RYB Red + Blue.  Likewise dark.
        Vector3 CG111 = new( 1.0f, 1.0f, 1.0f ); //White
        */

        // Adjusted palette
        Vector3 CG000 = new Vector3(0.2f, 0.2f, 0.2f);  //Black
        Vector3 CG100 = new Vector3(1.0f, 0.0f, 0.2f);  //Red
        Vector3 CG010 = new Vector3(0.9f, 0.9f, 0.2f);  //Yellow = RGB Red+Green.
        Vector3 CG001 = new Vector3(0.2f, 0.36f, 1.0f); //Blue
        Vector3 CG011 = new Vector3(0.2f, 0.9f, 0.2f);  //Green
        Vector3 CG110 = new Vector3(1.0f, 0.6f, 0.2f);  //Orange = RGB full Red, 60% Green
        Vector3 CG101 = new Vector3(0.6f, 0.2f, 1.0f);  //Purple = 60% Red, full Blue
        Vector3 CG111 = new Vector3(1.0f, 1.0f, 1.0f);  //White

        // Test pastel palette
        /*
        Vector3 CG000 = new Vector3(0.5f, 0.5f, 0.5f);      //Black
        Vector3 CG100 = new Vector3(1.0f, 0.5f, 0.5f);      //Red
        Vector3 CG010 = new Vector3(1f, 1f, 0.5f);          //Yellow = RGB Red + Green.
        Vector3 CG001 = new Vector3(0.5f, 0.5f, 1.0f);      //Blue
        Vector3 CG011 = new Vector3(0.5f, 1f, 0.5f);        //Green
        Vector3 CG110 = new Vector3(1.0f, 0.8f, 0.5f);      //Orange = RGB full Red, 60% Green
        Vector3 CG101 = new Vector3(0.8f, 0.5f, 1.0f);      //Purple = 60% Red, full Blue
        Vector3 CG111 = new Vector3(1.0f, 1.0f, 1.0f);      //White
        */

        // Trilinear interpolation for color
        Vector3 C00, C01, C10, C11;
        C00 = ((CG000 *  (1.0f - input.r)) +  (CG100 * input.r));
        C01 = ((CG001 *  (1.0f - input.r)) +  (CG101 * input.r));
        C10 = ((CG010 *  (1.0f - input.r)) +  (CG110 * input.r));
        C11 = ((CG011 *  (1.0f - input.r)) +  (CG111 * input.r));

        Vector3 C0, C1;
        C0 = ((C00 *( 1.0f - input.g)) + (C10 * input.g));
        C1 = ((C01 *( 1.0f - input.g)) + (C11 * input.g));

        Vector3 C;
        C = ((C0 * (1.0f - input.b)) + (C1 * input.b));

        _red = C.x;
        _yellow = C.y;
        _blue = C.z;
        _alpha = input.a;
    }

    public RYBColor(float p_red, float p_yellow, float p_blue, float p_alpha)
    {
        // Key = 0;
        this._red = p_red;
        this._yellow = p_yellow;
        this._blue = p_blue;
        this._alpha = p_alpha;
    }

    public Color toRGB()
    {
        Vector3 CG000 = new Vector3(0.2f, 0.2f, 0.2f);  //Black
        Vector3 CG100 = new Vector3(1.0f, 0.0f, 0.2f);  //Red
        Vector3 CG010 = new Vector3(0.9f, 0.9f, 0.2f);  //Yellow = RGB Red+Green.
        Vector3 CG001 = new Vector3(0.2f, 0.36f, 1.0f); //Blue
        Vector3 CG011 = new Vector3(0.2f, 0.9f, 0.2f);  //Green
        Vector3 CG110 = new Vector3(1.0f, 0.6f, 0.2f);  //Orange = RGB full Red, 60% Green
        Vector3 CG101 = new Vector3(0.6f, 0.2f, 1.0f);  //Purple = 60% Red, full Blue
        Vector3 CG111 = new Vector3(1.0f, 1.0f, 1.0f);  //White

        // Trilinear interpolation for color
        Vector3 C00, C01, C10, C11;
        C00 = ((CG000 * (1.0f - _red)) + (CG100 * _red));
        C01 = ((CG001 * (1.0f - _red)) + (CG101 * _red));
        C10 = ((CG010 * (1.0f - _red)) + (CG110 * _red));
        C11 = ((CG011 * (1.0f - _red)) + (CG111 * _red));

        Vector3 C0, C1;
        C0 = ((C00 * (1.0f - _yellow)) + (C10 * _yellow));
        C1 = ((C01 * (1.0f - _yellow)) + (C11 * _yellow));
        Vector3 C;
        C = ((C0 * (1.0f - _blue)) + (C1 * _blue));

        Color output = new Color(C.x, C.y, C.z, _alpha);

        return output;
    }
}
