// Transformation from RGB to RYB through HSV warping

using UnityEngine;

public struct RYBColor
{

    public float red;
    public float yellow;
    public float blue;
    public float alpha;

    public RYBColor(Color input)
    {
        float Hue, Saturation, Value;
        Color.RGBToHSV(input, out Hue, out Saturation, out Value);

        red = 0; yellow = 0; blue = 0; alpha = 0;

        Hue = HueRemapToRYB(Hue);

        Color newRYB = Color.HSVToRGB(Hue, Saturation, Value);

        red = newRYB.r;
        yellow = newRYB.g;
        blue = newRYB.b;
        alpha = newRYB.a;
    }

    public RYBColor(float p_red, float p_yellow, float p_blue, float p_alpha)
    {
        // Key = 0;
        this.red = p_red;
        this.yellow = p_yellow;
        this.blue = p_blue;
        this.alpha = p_alpha;
    }

    public Color toRGB()
    {
        Color RybColor = new Color(red, yellow, blue);

        Vector3 HSVColor;
        Color.RGBToHSV(RybColor, out HSVColor.x, out HSVColor.y, out HSVColor.z);

        HSVColor.x = HueRemapToRGB(HSVColor.x);

        Color newRGB = Color.HSVToRGB(HSVColor.x, HSVColor.y, HSVColor.z);

        return newRGB;
    }

    private float HueRemapToRGB(float Value)
    {
        // This function converts from your RYB space back to your RGB space.
        // It's the exact reverse of HueRemapToRYB

        // Colors that go through this function will have their values remapped from the representation in RYB to the respective representation in RGB

        // Example: Inputting a Yellow Hue (120° in RYB) should return (60° in RGB)
        // Example: Inputting a Green Hue (180° in RYB) should return (120° in RGB)
        if (Value < 240 / 360f)
        {
            if (Value < 120 / 360f)
                return Value / 2f;
            else
                if (Value < 180 / 360f)
                    return Value - (60 / 360f);
                else
                    return 2f * (Value - 120 / 360f);
                // return (Value * 1.5f) - (1f / 3f); old erroneous green remap
        }
        return Value;
    }

    private float HueRemapToRYB(float Value)
    {
        // Basically, what this function does is to grab your current Hue and remap it so that if the color were to be yellow, 
        // the yellow color should now be assigned to 120 in my custom RYB Hue. This is so that I can access the Yellow component
        // by converting my HSV back to RGB and retrieve what's given in my Green channel.

        // Colors will NOT align if this is translated back to RGB as what'd be displayed in the Green channel will be the Yellow channel's values.

        // Example: Inputting a Yellow Hue (60° in RGB) should return (120° in RYB)
        // Example: Inputting a Green Hue (120° in RGB) should return (180° RYB)
        if (Value < 240 / 360f)
        {
            if (Value < 60 / 360f)
                return Value * 2f;
            else {
                if (Value < 120 / 360f)
                    return Value + (60 / 360f);
                else
                    return Value / 2f + (120 / 360f);
            }
                //return (Value / 1.5f) + (2f / 9f); old erroneous green remap
        }
        return Value;
    }

    public static RYBColor operator +(RYBColor a, RYBColor b) => new RYBColor(a.red + b.red, a.yellow + b.yellow, a.blue + b.blue, a.alpha + b.alpha);
    public static RYBColor operator -(RYBColor a, RYBColor b) => new RYBColor(a.red - b.red, a.yellow - b.yellow, a.blue - b.blue, a.alpha - b.alpha);
    public static RYBColor operator *(RYBColor a, float b) => new RYBColor(a.red * b, a.yellow * b, a.blue * b, a.alpha);
    public static RYBColor operator /(RYBColor a, float b) => new RYBColor(a.red / b, a.yellow / b, a.blue / b, a.alpha);
    public RYBColor floor () => new RYBColor(Mathf.Floor(red), Mathf.Floor(yellow), Mathf.Floor(blue), alpha);
    public override string ToString()
    {
        return "(" + red + "," + yellow + "," + blue + "," + alpha + ")";
    }
}
