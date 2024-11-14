// ArrayColor lets you build a color out of GameColors
// ArrayColor allows you to convert said color to RGB through proccess through RYB space
// ArrayColor allows you to edit the array at free

// Created by Javier Soto

using System.Collections.Generic;
using UnityEngine;

public class ArrayColor
{
    List<GameColor> _colors;

    public ArrayColor(GameColor[] p_colors)
    {
        _colors = new List<GameColor>(p_colors);
    }

    public ArrayColor(GameColor p_color)
    {
        _colors = new List<GameColor>();
        _colors.Add(p_color);
    }

    public ArrayColor()
    {
        _colors = new List<GameColor>();
    }

    public GameColor this[int i]
    {
        get { return _colors[i]; }
        set { _colors[i] = value; }
    }

    public int Count() { return _colors.Count; }
    public void Clear() { _colors.Clear(); }


    public void Add(GameColor color)
    {
        _colors.Add(color);
    }

    public void Remove(GameColor p_color)
    {
        _colors.Remove(p_color);
    }

    public void Remove(GameColor[] p_colors)
    {
        foreach (var color in p_colors)
        {
            if(_colors.Contains(color))
                _colors.Remove(color);
        }
    }

    public bool Contains(GameColor[] p_colors)
    {
        // Ver si Colors contiene completamente a p_colors
        List<GameColor> copy = new(_colors);
        foreach (var color in p_colors)
        {
            if (copy.Contains(color))
                Remove(color);
            else
                return false;
        }
        return true;
    }

    public bool Contains(GameColor p_color)
    {
        // Ver si Colors contiene completamente a p_colors
        return _colors.Contains(p_color);
    }

    public Color toRGB()
    {
        GameColor[] tempArray = _colors.ToArray();
        return CalculateRGB(tempArray);

        /*if (_colors.Count == 0) return Color.white;
        int Yellow = 0, Blue = 0, Red = 0, White = 0, Max;
        foreach (var gcolor in _colors)
        {
            switch(gcolor)
            {
                case GameColor.Yellow: Yellow++; break;
                case GameColor.Blue: Blue++; break;
                case GameColor.Red: Red++; break;
                case GameColor.White: White++; break;
                case GameColor.Black: White--; break;
                default: break;
            }
        }
        Max = Mathf.Max(Yellow, Blue, Red);
        float rybRed, rybYellow, rybBlue;

        float WhiteBalance = (White * 1f / _colors.Count);

        rybRed = Mathf.Clamp((Red * 1.0f / Max) + WhiteBalance, 0, 1f);
        rybYellow = Mathf.Clamp((Yellow * 1.0f / Max) + WhiteBalance, 0, 1f);
        rybBlue = Mathf.Clamp((Blue * 1.0f / Max) + WhiteBalance, 0, 1f);

        RYBColor color;
        if (Yellow == Red && Red == Blue)
        {
            if (Red != 0)   // if color is gray
                color = new RYBColor(0.5f, 0.5f, 0.5f, 1.0f);
            else            
                if(WhiteBalance > 0)    // if color is just white
                    color = new RYBColor(1f, 1f, 1f, 1.0f);
                else                    // if color is just black
                    color = new RYBColor(0f, 0f, 0f, 1.0f);
        }
        else    // if color is any other combination
            color = new RYBColor(rybRed, rybYellow, rybBlue, 1.0f);

        return color.toRGB();*/
    }

    public static Color makeRGB(GameColor[] p_colors)
    {
        return CalculateRGB(p_colors);
    }

    public static Color makeRGB(GameColor p_color)
    {
        /*RYBColor color;
        switch (p_color)
            {
                case GameColor.Yellow:  color = new RYBColor(0f, 1f, 0f, 1f); break;
                case GameColor.Blue:    color = new RYBColor(0f, 0f, 1f, 1f); ; break;
                case GameColor.Red:     color = new RYBColor(1f, 0f, 0f, 1f); ; break;
                case GameColor.White:   color = new RYBColor(1f, 1f, 1f, 1f); ; break;
                default:                color = new RYBColor(0f, 0f, 0f, 1f); ; break;
            }*/
        GameColor[] tempArray = { p_color };

        return CalculateRGB(tempArray);
    }

    static Color CalculateRGB(GameColor[] p_colors)
    {
        if (p_colors.Length == 0) return Color.white;   // Cancel if color is null

        // Color counters
        int Yellow = 0, Blue = 0, Red = 0, White = 0, Max;

        // Count colors
        foreach (var gcolor in p_colors)
        {
            switch (gcolor)
            {
                case GameColor.Yellow: Yellow++; break;
                case GameColor.Blue: Blue++; break;
                case GameColor.Red: Red++; break;
                case GameColor.White: White++; break;
                case GameColor.Black: White--; break;
                default: break;
            }
        }

        // Get max color
        Max = Mathf.Max(Yellow, Blue, Red);
        float rybRed, rybYellow, rybBlue;

        // Calculate white addition
        float WhiteBalance = (White * 1f / p_colors.Length);

        // Get RYB components of colors
        rybRed = Mathf.Clamp((Red * 1.0f / Max) + WhiteBalance, 0, 1f);
        rybYellow = Mathf.Clamp((Yellow * 1.0f / Max) + WhiteBalance, 0, 1f);
        rybBlue = Mathf.Clamp((Blue * 1.0f / Max) + WhiteBalance, 0, 1f);

        // Make RYB color
        RYBColor color;
        if (Yellow == Red && Red == Blue)
        {
            if (Red != 0)   // if color is gray
                color = new RYBColor(0.5f, 0.5f, 0.5f, 1.0f);
            else
                if (WhiteBalance > 0)    // if color is just white
                color = new RYBColor(1f, 1f, 1f, 1.0f);
            else                    // if color is just black
                color = new RYBColor(0f, 0f, 0f, 1.0f);
        }
        else    // if color is any other combination
            color = new RYBColor(rybRed, rybYellow, rybBlue, 1.0f);

        return color.toRGB();
    }
}
