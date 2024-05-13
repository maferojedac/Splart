using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public Spawnable spawnableEnemies;
    public Spawner[] _spawners;
    private int _wave;

    void Start()
    {
        _wave = 1;
        GenerateWave(1);
    }

    void Update()
    {
        if (AllDone())
        {
            _wave++;
            GenerateWave(_wave);
        }
    }

    public bool AllDone()
    {
        foreach (Spawner spawner in _spawners)
        {
            if (!spawner.Done())
                return false;
        }
        return true;
    }

    public void GenerateWave(int Wave)
    {
        int Score = Wave * 2;
        float MasterDelay = 0f;
        List<SpawnableObject> GeneratedWave = new();
        while (Score > 0)
        {
            GeneratedWave.Add(new SpawnableObject(MasterDelay, spawnableEnemies.Spawnables[Random.Range(0, spawnableEnemies.Spawnables.Length)]));
            if (GeneratedWave.Count > 0 && GeneratedWave.Count % _spawners.Length == 0)
                MasterDelay += Random.Range(5f, 6f);
            else
                MasterDelay += Random.Range(0f, 1f);
            Score--;
        }

        int Count = 0;
        foreach (SpawnableObject spawnableObject in GeneratedWave)
        {
            _spawners[Count % _spawners.Length]._spawnableQueue.Add(spawnableObject);
            Count++;
        }
    }
}
