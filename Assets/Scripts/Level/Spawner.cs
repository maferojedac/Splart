using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<SpawnableObject> _spawnableQueue = new();
    public SpawnTracker _spawnTracker;

    private float _timer;

    private bool _generating;

    void Start()
    {
        _spawnTracker.Register(this);
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
                enemy.transform.parent = transform.parent;
                _spawnableQueue.RemoveAt(0);
            }
        }
        else
        {
            _generating = false;
            _timer = 0f;
        }
    }

    public bool Done()
    {
        return !_generating;
    }
}
