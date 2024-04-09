using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class RYBColor
{
    public float Red;
    public float Yellow;
    public float Blue;
    public float Alpha;

    // Borrowing code and palette from https://github.com/ProfJski/ArtColors/

    public RYBColor(Color input)
    {

        // RGB corners in RYB Values
        Vector3 CG000 = new( 0.0f, 0.0f, 0.0f ); //Black
        Vector3 CG100 = new( 0.891f, 0.0f, 0.0f ); //Red
        Vector3 CG010 = new( 0.0f, 0.714f, 0.374f ); //Green = RYB Yellow + Blue
        Vector3 CG001 = new( 0.07f, 0.08f, 0.893f ); //Blue:
        Vector3 CG011 = new( 0.0f, 0.116f, 0.313f ); //Cyan = RYB Green + Blue.  Very dark to make the rest of the function work correctly
        Vector3 CG110 = new( 0.0f, 0.915f, 0.0f ); //Yellow
        Vector3 CG101 = new( 0.554f, 0.0f, 0.1f ); //Magenta =RYB Red + Blue.  Likewise dark.
        Vector3 CG111 = new( 1.0f, 1.0f, 1.0f ); //White

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

        // C = Saturate(C, 0.5f);
        Red = C.x;
        Yellow = C.y;
        Blue = C.z;
        Alpha = input.a;
    }

    public RYBColor(float Red, float Yellow, float Blue, float Alpha)
    {
        // Key = 0;
        this.Red = Red;
        this.Yellow = Yellow;
        this.Blue = Blue;
        this.Alpha = Alpha;
    }

    public Color toRGB()
    {
        Vector3 CG000 = new Vector3( 0.0f, 0.0f, 0.0f ); //Black
        Vector3 CG100 = new Vector3( 1.0f, 0.0f, 0.0f ); //Red
        Vector3 CG010 = new Vector3( 0.9f, 0.9f, 0.0f ); //Yellow = RGB Red+Green.  Still a bit high, but helps Yellow compete against Green.  Lower gives murky yellows.
        Vector3 CG001 = new Vector3( 0.0f, 0.36f, 1.0f ); //Blue: Green boost of 0.36 helps eliminate flatness of spectrum around pure Blue
        Vector3 CG011 = new Vector3( 0.0f, 0.9f, 0.2f ); //Green: A less intense green than {0,1,0}, which tends to dominate
        Vector3 CG110 = new Vector3( 1.0f, 0.6f, 0.0f ); //Orange = RGB full Red, 60% Green
        Vector3 CG101 = new Vector3( 0.6f, 0.0f, 1.0f ); //Purple = 60% Red, full Blue
        Vector3 CG111 = new Vector3( 1.0f, 1.0f, 1.0f ); //White

        // Trilinear interpolation for color
        Vector3 C00, C01, C10, C11;
        C00 = ((CG000 * (1.0f - Red)) + (CG100 * Red));
        C01 = ((CG001 * (1.0f - Red)) + (CG101 * Red));
        C10 = ((CG010 * (1.0f - Red)) + (CG110 * Red));
        C11 = ((CG011 * (1.0f - Red)) + (CG111 * Red));

        Vector3 C0, C1;
        C0 = ((C00 * (1.0f - Yellow)) + (C10 * Yellow));
        C1 = ((C01 * (1.0f - Yellow)) + (C11 * Yellow));
        Vector3 C;
        C = ((C0 * (1.0f - Blue)) + (C1 * Blue));

        Color output = new Color(C.x, C.y, C.z, Alpha);

        return output;
    }

    private Vector3 Saturate(Vector3 color, float sat)
    {
        if (Mathf.Abs(sat) < 0.004) { return color; }  //Immediately return when sat is zero or so small no difference will result (less than 1/255)
        if ((color.x == 0)&& (color.y == 0)&& (color.z == 0)) { return color; }  //Prevents division by zero trying to saturate black

        Vector3 clerp = color;
        Vector3 output = clerp;

        if (sat > 0.0)
        {
            Vector3 maxsat;
            float mx = Mathf.Max(color.x, color.y, color.z);
            maxsat = clerp * (1.0f / mx);
            output += maxsat * sat;
        }
        if (sat < 0.0)
        {
            Vector3 grayc;
            float avg = (color.x + color.y + color.z);
            avg /= (3.0f * 255.0f);
            grayc =new Vector3 (avg,avg,avg);
            output += grayc * (-1.0f * sat);
        }
        
        return output;
    }
}
