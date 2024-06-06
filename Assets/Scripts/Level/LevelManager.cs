using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IGameState
{

    public GameObject TutorialLevel;
    public GameObject[] Levels;

    public float ExitTime;

    public LevelData _levelData;

    private float _timer;
    private IEnumerator _currentCoroutine;

    private GameObject _lastLevel;

    private TraversalMenu _menu;

    void Start()
    {
        _menu = GetComponent<TraversalMenu>();
        _levelData.SetMenuInstance(gameObject);
    }

    public void ButtonStartGameAction()
    {
        StartCoroutine(StartGameSequenceCoroutine());
    }

    public void ButtonStartTutorialAction()
    {
        StartCoroutine(StartTutorialSequenceCoroutine());
    }

    private IEnumerator StartTutorialSequenceCoroutine()
    {
        if (_lastLevel != null)
            _levelData.UnloadPreviousLevel();
        _timer = 0;
        while (_timer < ExitTime)
        {
            _timer += Time.deltaTime;
            yield return null;
        }
        _timer = 0;
        Destroy(_lastLevel);
        _lastLevel = Instantiate(TutorialLevel);
        _lastLevel.SetActive(true);
        _levelData.SetInstance(_lastLevel);
        _levelData.StartGame();
        // while (_timer < ExitTime)
        // {
        //     _timer += Time.deltaTime;
        //     yield return null;
        // }
    }

    private IEnumerator StartGameSequenceCoroutine()
    {
        if(_lastLevel != null)
            _levelData.UnloadPreviousLevel();
        _timer = 0;
        while(_timer < ExitTime)
        {
            _timer += Time.deltaTime;
            yield return null;
        }
        _timer = 0;
        Destroy(_lastLevel);
        _lastLevel = Instantiate(Levels[Random.Range(0, Levels.Length)]);
        _lastLevel.SetActive(true);
        _levelData.SetInstance(_lastLevel);
        _levelData.StartGame();
        // while (_timer < ExitTime)
        // {
        //     _timer += Time.deltaTime;
        //     yield return null;
        // }
    }

    void IGameState.EndGame()
    {
        // _lastLevel.SetActive(false);
        _menu.GoToMain();
    }

    void IGameState.StartGame()
    {
        // Should not be called here as to avoid infinite loops
    }
}
