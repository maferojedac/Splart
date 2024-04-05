using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayColor
{
    List<GameColor> Colors;

    public ArrayColor(GameColor[] p_colors)
    {
        Colors = new List<GameColor>(p_colors);
    }

    public ArrayColor()
    {
        Colors = new List<GameColor>();
    }

    public void Add(GameColor color)
    {
        Colors.Add(color);
    }

    public void Clear() { Colors.Clear(); }

    public void Remove(GameColor p_color)
    {
        Colors.Remove(p_color);
    }

    public void Remove(GameColor[] p_colors)
    {
        foreach (var color in p_colors)
        {
            if(Colors.Contains(color))
                Colors.Remove(color);
        }
    }

    public bool Contains(GameColor[] p_colors)
    {
        // Ver si Colors contiene completamente a p_colors
        List<GameColor> copy = new(Colors);
        foreach (var color in p_colors)
        {
            if (copy.Contains(color))
                Remove(color);
            else
                return false;
        }
        return true;
    }

    public Color toRGB()
    {
        if (Colors.Count == 0) return Color.white;
        int Yellow = 0, Blue = 0, Red = 0, Max;
        foreach (var gcolor in Colors)
        {
            switch(gcolor)
            {
                case GameColor.Yellow: Yellow++; break;
                case GameColor.Blue: Blue++; break;
                case GameColor.Red: Red++; break;
                default: break;
            }
        }
        Max = Mathf.Max(Yellow, Blue, Red);
        //Max = Colors.Count;
        RYBColor color = new RYBColor(Red * 1.0f / Max, Yellow * 1.0f / Max, Blue * 1.0f / Max, 1.0f);
        // color = new RYBColor(0f, 1f, 1f, 1.0f);

        return color.toRGB();
    }
}
