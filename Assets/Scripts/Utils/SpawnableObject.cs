using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpawnableObject
{
    public float Delay;
    public GameObject enemyType;
    public ArrayColor forcedColor;

    public SpawnableObject(float p_delay,  GameObject p_enemyType, ArrayColor forcedColor = null)
    {
        this.Delay = p_delay;
        this.enemyType = p_enemyType;
        this.forcedColor = forcedColor;
    }
}
