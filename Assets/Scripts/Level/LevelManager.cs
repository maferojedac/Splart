using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject[] Levels;
    public GameObject GameBase;

    public float ExitTime;

    public LevelData _levelData;

    private float _timer;
    private IEnumerator _currentCoroutine;

    private GameObject _lastLevel;

    void Start()
    {
        GameBase.SetActive(false);
    }

    public void StartGameSequence()
    {
        StartCoroutine(StartGameSequenceCoroutine());
    }

    public void EndGameSequence()
    {
        GameBase.SetActive(false);
    }

    private IEnumerator StartGameSequenceCoroutine()
    {
        _timer = 0;
        while(_timer < ExitTime)
        {
            _timer += Time.deltaTime;
            yield return null;
        }
        _timer = 0;
        Destroy(_lastLevel);
        _lastLevel = Instantiate(Levels[0]);
        _levelData.SetInstance(_lastLevel);
        GameBase.SetActive(true);
        _lastLevel.SetActive(true);
        while (_timer < ExitTime)
        {
            _timer += Time.deltaTime;
            yield return null;
        }
    }
}