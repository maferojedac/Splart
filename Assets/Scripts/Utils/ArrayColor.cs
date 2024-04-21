using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayColor
{
    List<GameColor> _colors;

    public ArrayColor(GameColor[] p_colors)
    {
        _colors = new List<GameColor>(p_colors);
    }

    public ArrayColor()
    {
        _colors = new List<GameColor>();
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

    public Color toRGB()
    {
        if (_colors.Count == 0) return Color.white;
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
        float WhiteBalance = White * (1f / _colors.Count);
        if(Yellow == Red && Red == Blue)
            if (Red != 0)
                WhiteBalance = -0.5f;
            else
                Max = 1;
        RYBColor color = new RYBColor((Red * 1.0f / Max) + WhiteBalance, (Yellow * 1.0f / Max) + WhiteBalance, (Blue * 1.0f / Max) + WhiteBalance, 1.0f);

        return color.toRGB();
    }

    public static Color makeRGB(GameColor[] p_colors)
    {
        List<GameColor> Colors = new List<GameColor>(p_colors);

        if (Colors.Count == 0) return Color.white;
        int Yellow = 0, Blue = 0, Red = 0, White = 0, Max;
        foreach (var gcolor in Colors)
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
        Max = Mathf.Max(Yellow, Blue, Red);
        float WhiteBalance = White * (1f / p_colors.Length);
        if (Yellow == Red && Red == Blue && Red != 0)
            WhiteBalance = -0.5f;
        RYBColor color = new RYBColor((Red * 1.0f / Max) + WhiteBalance, (Yellow * 1.0f / Max) + WhiteBalance, (Blue * 1.0f / Max) + WhiteBalance, 1.0f);

        return color.toRGB();
    }

    public static Color makeRGB(GameColor p_color)
    {
        RYBColor color;
        switch (p_color)
            {
                case GameColor.Yellow:  color = new RYBColor(0f, 1f, 0f, 1f); break;
                case GameColor.Blue:    color = new RYBColor(0f, 0f, 1f, 1f); ; break;
                case GameColor.Red:     color = new RYBColor(1f, 0f, 0f, 1f); ; break;
                case GameColor.White:   color = new RYBColor(1f, 1f, 1f, 1f); ; break;
                default:                color = new RYBColor(0f, 0f, 0f, 1f); ; break;
            }

        return color.toRGB();
    }
}
