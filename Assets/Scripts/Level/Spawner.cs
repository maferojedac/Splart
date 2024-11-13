using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<SpawnableObject> _spawnableQueue = new();

    private float _timer;

    public EnemyPooling _enemyPooler;

    public MapNode _startingNode;

    private bool _generating;
    private GameObject _lastGenerated;

    private int _complexity;
    private bool _allowedKey;

    private bool _enableSpawning;
    private ArrayColor _forcedColor;

    void Start()
    {
        _enableSpawning = true;

        if (_enemyPooler == null)
            _enemyPooler = GameObject.Find("Enemies").GetComponent<EnemyPooling>();
    }

    void Update()
    {
        // Generar enemigos
        if (_enableSpawning)
        {
            _timer += Time.deltaTime;
            if (_spawnableQueue.Count > 0)
            {
                _generating = true;
                if (_timer > _spawnableQueue[0].Delay)
                {
                    _timer = 0f;
                    Enemy enemy = _enemyPooler.Spawn(_spawnableQueue[0].enemyType);
                    enemy.SetSoundManager(_enemyPooler.GetComponent<EnemySoundManager>());

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
            }
            else
            {
                if (!_lastGenerated.activeSelf)  // dont end sequence until enemy is dead
                {
                    _generating = false;
                    _timer = 0f;
                }
                else
                {
                    _generating = true;
                }
            }
        }
    }

    private ArrayColor GenerateColor(int complexity, bool allowedKey)
    {
        ArrayColor finalColor = new ArrayColor();
        for (int i = 0; i < Random.Range(1, complexity); i++)
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
        if (allowedKey && Random.value > 0.5f)
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

    public void AddToQueue(SpawnableObject spawnable)
    {
        _generating = true;
        _spawnableQueue.Add(spawnable);
    }

    public bool IsStillGenerating()
    {
        return _generating;
    }

    public void Disable()
    {
        _enableSpawning = false;
    }

    public void ForceColor(ArrayColor color)
    {
        _forcedColor = color;
    }

    public void SetComplexity(int complexity)
    {
        _allowedKey = complexity > 3;
        _complexity = (complexity / 2) + 1;
        if (_complexity > 5)
            _complexity = 5;
    }
}
