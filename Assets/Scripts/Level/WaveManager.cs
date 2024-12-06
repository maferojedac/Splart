// WaveManager expects to be attached to a child of Level's Master Prefab. Preferably to an object of the same name as script.
// WaveManager is one way to define objectives and goals for each level
// WaveManager currently generates waves infinitely until player loses all lives.

// Created by Javier Soto

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour, ILevelEvent
{
    [Header("Initialization configuration")]
    [Tooltip("Provide level communication!")] public LevelData _levelData;
    [Tooltip("Attach spawner objects here!")] public Spawner[] _spawners;
    [Range(0, 1)][Tooltip("Chance of bonus roudn per round!")] public float BonusChance;

    [Header("Level Wave Starting Difficulty")]
    public int ColorComplexity = 1;
    public int TimeComplexity = 1;
    public int WaveLength = 0;
    public int EnemiesSpeed = 0;

    private int _wave;

    private int _timeScore;
    private int _complexityScore;
    private int _waveScore;
    private int _speedScore;

    private bool _wasBossGenerated;

    private LevelLoader _loader;

    private GameObject[] SpawnablesCommon;
    private GameObject[] SpawnablesMinibosses;
    private GameObject SpawnableBoss;
    private GameObject[] SpawnablesBonus;

    void Awake()
    {
        // Get from level settings
        SpawnablesCommon = transform.parent.GetComponent<LevelSettings>().SpawnablesCommon;
        SpawnablesMinibosses = transform.parent.GetComponent<LevelSettings>().SpawnablesMinibosses;
        SpawnableBoss = transform.parent.GetComponent<LevelSettings>().SpawnablesStageBoss;
        SpawnablesBonus = transform.parent.GetComponent<LevelSettings>().SpawnablesBonus;

        _loader = transform.parent.GetComponent<LevelLoader>();

        _levelData.SubscribeToEvents(this);
    }

    void OnEnable()
    {
        _wave = 1;

        // Initialize variables
        _complexityScore = ColorComplexity;
        _timeScore = TimeComplexity;
        _waveScore = WaveLength;
        _speedScore = EnemiesSpeed;
        _wasBossGenerated = false;

        _levelData.SetGlobalSpeedWaveMultiplier(1 + (_speedScore / 10f));

        GenerateWave();
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

    public void GenerateWave()
    {
        int Length = Random.Range(4 + _waveScore, 5 + _waveScore);
        List<SpawnableObject> GeneratedWave = new();

        if (Random.value < BonusChance)
            GeneratedWave.Add(new SpawnableObject(0, SpawnablesBonus[Random.Range(0, SpawnablesBonus.Length)]));

        for (int i = 0; i < Length; i++)
        {
            if (!_wasBossGenerated && Random.value < 0.5f)
            {
                _wasBossGenerated = true;
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

    public void SpawnBoss()
    {
        DisableSpawners();
        _spawners[0].AddToQueue(new SpawnableObject(0.1f, SpawnableBoss));
        StartCoroutine(WaitBeforeBossSpawn());
    }

    IEnumerator WaitBeforeBossSpawn()
    {
        yield return new WaitForSeconds(2f);
        _spawners[0].StartGeneration();
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

    // If an object was painted, it's time to increase difficulty
    public void PaintObject()
    {
        if (!gameObject.activeInHierarchy) return;
        _wave++;
        _complexityScore++;
        _timeScore++;
        _waveScore++;
        _speedScore++;
        _levelData.SetGlobalSpeedWaveMultiplier(1 + (_speedScore / 10f));
    }

    IEnumerator WaitForWaveEnd()
    {
        bool condition = true;
        while (condition)
        {
            if (AllDone())
            {
                _wasBossGenerated = false;
                _levelData.NextWave();
                GenerateWave();
            }

            yield return new WaitForSeconds(0.1f);  // Check for wave end 10 times per second (We don't need it to be *that* precise)
        }
    }
}
