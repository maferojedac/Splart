using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public Spawnable spawnableEnemies;
    public Spawnable spawnableBosses;
    public LevelData _levelData;
    public Spawner[] _spawners;

    private int _wave;

    private int _timeScore;
    private int _complexityScore;
    private int _waveScore;

    private bool _allowBoss;

    void Start()
    {
        _wave = 1;

        _complexityScore = 1;
        _timeScore = 1;
        _waveScore = 0;

        GenerateWave(1);
    }

    void Update()
    {
        if (AllDone())
        {
            _wave++;
            _complexityScore++;
            _timeScore++;
            if (_timeScore % 2 == 0 && _waveScore < 5)
                _waveScore++;
            _allowBoss = Random.value > 0.5;
            _levelData.NextWave();
            GenerateWave(_wave);
        }
    }

    public bool AllDone()
    {
        foreach (Spawner spawner in _spawners)
        {
            if (spawner.IsStillGenerating())
                return false;
        }
        return true;
    }

    public void GenerateWave(int Wave)
    {
        Debug.Log("Generating new wave!");
        int Length = Random.Range(4 + _waveScore, 5 + _waveScore);
        List<SpawnableObject> GeneratedWave = new();
        for(int i = 0; i < Length; i++)
        {
            if(_allowBoss)
            {
                _allowBoss = false;
                GeneratedWave.Add(new SpawnableObject(GenerateTime(_timeScore, i), spawnableBosses.Spawnables[Random.Range(0, spawnableBosses.Spawnables.Length)]));
            }
            else
            {
                GeneratedWave.Add(new SpawnableObject(GenerateTime(_timeScore, i), spawnableEnemies.Spawnables[Random.Range(0, spawnableEnemies.Spawnables.Length)]));
            }
        }
        int Count = 0;
        foreach (SpawnableObject spawnableObject in GeneratedWave)
        {
            _spawners[Count % _spawners.Length].SetComplexity(_complexityScore);
            _spawners[Count % _spawners.Length].AddToQueue(spawnableObject);
            Count++;
        }
    }

    private float GenerateTime(int Score, int QueueElement)
    {
        // collapse min time at aprox 4.9 secs
        // first times at aprox 12 secs
        if (QueueElement < _spawners.Length)
            return 5f;
        float newTime;
        newTime = 20f / (Mathf.Sqrt(Score + 5)) + 4f;
        newTime += Random.value * 2f;
        if (Random.value > 0.8f)
            newTime /= 2f;
        return newTime;
    }
}
