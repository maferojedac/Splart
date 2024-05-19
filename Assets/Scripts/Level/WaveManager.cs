using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public Spawnable spawnableEnemies;
    public LevelData _levelData;
    public Spawner[] _spawners;

    private int _wave;
    private int _timeScore;
    private int _complexityScore;
    private bool _isNextPointTowardsComplexity;

    void Start()
    {
        _wave = 1;
        _complexityScore = 1;
        _timeScore = 1;
        GenerateWave(1);
    }

    void Update()
    {
        if (AllDone())
        {
            _wave++;
            _complexityScore++;
            _timeScore++;
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
        int Length = Random.Range(4, 5);
        List<SpawnableObject> GeneratedWave = new();
        for(int i = 0; i < Length; i++)
        {
            GeneratedWave.Add(new SpawnableObject(GenerateTime(_timeScore), spawnableEnemies.Spawnables[Random.Range(0, spawnableEnemies.Spawnables.Length)]));
        }
        int Count = 0;
        foreach (SpawnableObject spawnableObject in GeneratedWave)
        {
            _spawners[Count % _spawners.Length].SetComplexity(_complexityScore);
            _spawners[Count % _spawners.Length].AddToQueue(spawnableObject);
            Count++;
        }
    }

    private float GenerateTime(int Score)
    {
        // collapse min time at aprox 4.9 secs
        // first times at aprox 12 secs
        float newTime = 0;
        newTime = 20f / (Mathf.Sqrt(Score + 5)) + 4f;
        newTime += Random.value * 2f;
        return newTime;
    }
}
