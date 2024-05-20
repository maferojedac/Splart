using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<SpawnableObject> _spawnableQueue = new();

    private float _timer;

    public Transform _enemyParent;

    private bool _generating;
    private GameObject _lastGenerated;

    private int _complexity;
    private bool _allowedKey;

    void Start()
    {
        if(_enemyParent == null)
            _enemyParent = GameObject.Find("Enemies").transform;
    }

    void Update()
    {
        // Generar enemigos
        _timer += Time.deltaTime;
        if(_spawnableQueue.Count > 0)
        {
            _generating = true;
            if (_timer > _spawnableQueue[0].Delay)
            {
                _timer = 0f;
                GameObject enemy = Instantiate(_spawnableQueue[0].SpawnObject, transform.position, Quaternion.identity);
                enemy.GetComponent<IEnemy>().SetColor(GenerateColor(_complexity, _allowedKey));
                enemy.transform.parent = _enemyParent;
                _lastGenerated = enemy;
                _spawnableQueue.RemoveAt(0);
            }
        }
        else
        {
            if(_lastGenerated == null)  // dont end sequence until enemy is dead
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

    private ArrayColor GenerateColor(int complexity, bool addKey)
    {
        ArrayColor finalColor = new ArrayColor();
        for (int i = 0; i < complexity; i++)
        {
            GameColor newColor;
            if (finalColor.Count() > 1)
            {
                newColor = finalColor[Random.Range(0, 1)];
            }
            else
            {
                newColor = (GameColor)System.Enum.ToObject(typeof(GameColor), Random.Range(0, 3));
            }
            finalColor.Add(newColor);
        }
        if (addKey)
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

    public void SetComplexity(int complexity)
    {
        _allowedKey = (complexity + 2) % 2 == 0;
        _complexity = (complexity / 2) + 1;
        if (_complexity > 3)
            _complexity = 3;
    }
}
