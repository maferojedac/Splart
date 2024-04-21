using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public SpawnTracker _tracker;
    private int _wave;

    void Start()
    {
        _wave = 1;
        _tracker.GenerateWave(1);
    }

    void Update()
    {
        if (_tracker.AllDone())
        {
            _wave++;
            _tracker.GenerateWave(_wave);
        }
    }
}
