using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpawnableObject
{
    public float Delay;
    public GameObject SpawnObject;

    public SpawnableObject(float p_delay,  GameObject p_spawnObject)
    {
        this.Delay = p_delay;
        this.SpawnObject = p_spawnObject;
    }
}
