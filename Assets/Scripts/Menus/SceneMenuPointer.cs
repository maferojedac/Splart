// Debugging class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMenuPointer : MonoBehaviour, IGameState
{
    // Duplicate of LevelManager that returns to main menu scene instead of calling dynamic menu.

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
        _levelData.SetMenuInstance(this);

        StartCoroutine(StartGameSequenceCoroutine());
    }

    public void ButtonStartGameAction()
    {
        // unused since scene is loaded when level is requested
    }

    private IEnumerator StartGameSequenceCoroutine()
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
        _lastLevel = Instantiate(Levels[0]);
        _lastLevel.SetActive(true);
        _levelData.SetLevelInstance(_lastLevel.GetComponent<IGameState>());
        _levelData.StartGame();
    }

    void IGameState.EndGame()
    {
        SceneManager.LoadScene("Resolutions"); //Para regresar al menu principal
    }

    void IGameState.StartGame()
    {
        // Should not be called here as to avoid infinite loops
    }
}
