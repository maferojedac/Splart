// WaveManager expects to be attached to a child of Level's Master Prefab. Preferably to an object of the same name as script.
// WaveManager is one way to define objectives and goals for each level
// WaveManager currently generates waves infinitely until player loses all lives.

// Created by Javier Soto

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Initialization configuration")]
    [Tooltip("Provide level communication!")] public LevelData _levelData;
    [Tooltip("Attach spawner objects here!")] public Spawner[] _spawners;

    private int _wave;

    private int _timeScore;
    private int _complexityScore;
    private int _waveScore;
    private int _speedScore;

    private bool _allowBoss;

    private LevelLoader _loader;

    private GameObject[] SpawnablesCommon;
    private GameObject[] SpawnablesMinibosses;

    void Awake()
    {
        // Get from level settings
        SpawnablesCommon = transform.parent.GetComponent<LevelSettings>().SpawnablesCommon;
        SpawnablesMinibosses = transform.parent.GetComponent<LevelSettings>().SpawnablesMinibosses;

        _loader = transform.parent.GetComponent<LevelLoader>();
    }

    void OnEnable()
    {
        _wave = 1;

        // Initialize variables
        _complexityScore = 1;
        _timeScore = 1;
        _waveScore = 0;
        _speedScore = 0;
        _allowBoss = true;

        GenerateWave(1);
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

        Debug.Log(SpawnablesCommon);
        for(int i = 0; i < Length; i++)
        {
            if (_allowBoss && i == Length - 1)
            {
                ArrayColor bossColor = GenerateLevelPaintingColor();

                GeneratedWave.Add(new SpawnableObject(GenerateTime(_timeScore, i), SpawnablesMinibosses[Random.Range(0, SpawnablesMinibosses.Length)], bossColor));
            }
            else
            {
                GeneratedWave.Add(new SpawnableObject(GenerateTime(_timeScore, i), SpawnablesCommon[Random.Range(0, SpawnablesCommon.Length)]));
            }
        }

        int Count = 0;
        foreach (SpawnableObject spawnableObject in GeneratedWave)
        {
            _spawners[Count % _spawners.Length].SetComplexity(_complexityScore);
            _spawners[Count % _spawners.Length].AddToQueue(spawnableObject);
            Count++;
        }

        StartSpawners();
        StartCoroutine(WaitForWaveEnd());
    }

    private ArrayColor GenerateLevelPaintingColor()
    {
        RYBColor paintColor = new RYBColor(_loader._nextPaintingColor);

        paintColor = paintColor * 3f;
        Debug.Log("Boss Color > "+paintColor);
        // paintColor = paintColor.floor();

        ArrayColor generatedColor = new ArrayColor();

        for (int reds = 0; reds < paintColor.red; reds++)
            generatedColor.Add(GameColor.Red);

        for (int yellows = 0; yellows < paintColor.yellow; yellows++)
            generatedColor.Add(GameColor.Yellow);

        for (int blues = 0; blues < paintColor.blue; blues++)
            generatedColor.Add(GameColor.Blue);

        return generatedColor;
    }

    private float GenerateTime(int Score, int QueueElement)
    {
        // collapse min time at aprox 4.9 secs
        // first times at aprox 12 secs
        if (QueueElement < _spawners.Length)
            return 5f;
        float newTime;
        newTime = 20f / (Mathf.Sqrt(Score + 10f));
        newTime += Random.value * 2f;
        if (Random.value > 0.6f)
            newTime /= 2f;
        return newTime;
    }

    public void DisableSpawners()
    {
        foreach (Spawner spawner in _spawners)
        {
            spawner.Disable();
        }
    }

    public void StartSpawners()
    {
        foreach (Spawner spawner in _spawners)
        {
            spawner.StartGeneration();
        }
    }

    IEnumerator WaitForWaveEnd()
    {
        bool condition = true;
        while (condition)
        {
            if (AllDone())
            {
                _wave++;
                _complexityScore++;
                _timeScore++;
                _waveScore++;
                if (_speedScore < 10 && _waveScore % 2 == 0)
                {
                    _speedScore++;
                    _levelData.SetGlobalSpeedWaveMultiplier(_levelData.GetGlobalSpeedMultiplier() + (_speedScore / 10f));
                }
                _allowBoss = Random.value > 0.5;
                _allowBoss = true;
                _levelData.NextWave();
                GenerateWave(_wave);
            }

            yield return new WaitForSeconds(0.1f);  // Check for wave end 10 times per second (We don't need it to be *that* precise)
        }
    }
}
