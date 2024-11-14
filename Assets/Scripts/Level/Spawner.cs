// Class is to be attached to sprites/objects that are supposed to spawn enemies
// Keep in mind it's got to have a reference to the WaveManager! (Or any other scripts that manage goals)

// Created by Javier Soto

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<SpawnableObject> _spawnableQueue = new();

    [Header("Spawner configuration")]
    [Tooltip("Attach Node to which enemies will go to!")] public MapNode _startingNode;

    private bool _generating;
    private GameObject _lastGenerated;

    private int _complexity;
    private bool _allowedKey;

    private bool _enableSpawning;
    private ArrayColor _forcedColor;

    private EnemyPooling _enemyPooler;
    private EnemySoundManager _enemySoundManager;

    void Awake()
    {
        if (_enemyPooler == null)
            _enemyPooler = GameObject.Find("Enemies").GetComponent<EnemyPooling>();
        if (_enemySoundManager == null)
            _enemySoundManager = GameObject.Find("Enemies").GetComponent<EnemySoundManager>();
    }

    void OnEnable()
    {
        _enableSpawning = true;
    }

    IEnumerator StartSpawnSequence()    // Coroutine that spawns all enemies
    {
        _generating = true;

        while (_spawnableQueue.Count > 0)
        {
            yield return new WaitForSeconds(_spawnableQueue[0].Delay);

            Enemy enemy = _enemyPooler.Spawn(_spawnableQueue[0].enemyType);
            enemy.SetSoundManager(_enemySoundManager);

            if (_forcedColor != null)
            {
                enemy.SetColor(_forcedColor);
                _forcedColor = null;
            }
            else
            {
                enemy.SetColor(GenerateColor(_complexity, _allowedKey));
            }

            enemy.Spawn(transform.position);

            EnemyMovement refe = enemy.gameObject.GetComponent<EnemyMovement>();
            if (refe != null)
            {
                refe.SetStartingNode(_startingNode);
                refe.StartRunning();
            }

            _lastGenerated = enemy.gameObject;
            _spawnableQueue.RemoveAt(0);
        }

        StartCoroutine(StartWatchSequence());
    }

    IEnumerator StartWatchSequence()    // Coroutine that waits until last generated enemy is killed
    {
        while (_lastGenerated.activeSelf)
        {
            yield return null;
        }

        _generating = false;
    }

    private ArrayColor GenerateColor(int complexity, bool allowedKey)   // Generate an array color with complexity and key
    {
        ArrayColor finalColor = new ArrayColor();

        for (int i = 0; i < Random.Range(1, complexity); i++)   // Color can only have two primary colors to avoid grays
        {
            GameColor newColor;
            if (finalColor.Count() > 2)
            {
                newColor = finalColor[Random.Range(0, 1)];
            }
            else
            {
                newColor = (GameColor)System.Enum.ToObject(typeof(GameColor), Random.Range(0, 3));
            }
            finalColor.Add(newColor);
        }

        if (allowedKey && Random.value > 0.5f)  // Add black or white at random
        {
            if(Random.value > 0.5)
            {
                finalColor.Add(GameColor.White);
            }
            else
            {
                finalColor.Add(GameColor.Black);
            }
        }

        return finalColor;
    }

    public void AddToQueue(SpawnableObject spawnable)   // Function called by goal managers to queue and generate enemies
    {
        _generating = true;
        _spawnableQueue.Add(spawnable);
    }

    public bool IsStillGenerating() // Check if spawner is active or not
    {
        return _generating;
    }

    public void StartGeneration()   // Start coroutine externally
    {
        StartCoroutine(StartSpawnSequence());
    }

    public void Disable()   // Disable spawner
    {
        _enableSpawning = false;
    }

    public void ForceColor(ArrayColor color)    // Set forced color for next enemy
    {
        _forcedColor = color;
    }

    public void SetComplexity(int complexity)   // Calculate complexity based on Wave's complexity
    {
        _allowedKey = complexity > 3;
        _complexity = (complexity / 2) + 1;
        if (_complexity > 5)
            _complexity = 5;
    }
}
