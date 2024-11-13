using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpawnableObject
{
    public float Delay;
    public EnemyType enemyType;

    public SpawnableObject(float p_delay,  EnemyType p_enemyType)
    {
        this.Delay = p_delay;
        this.enemyType = p_enemyType;
    }
}
