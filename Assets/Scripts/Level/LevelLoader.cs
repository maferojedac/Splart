using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour, IGameState
{
    public List<LevelObject> AllLevelSprites;

    private List<LevelObject> ColorSpritesQueue;
    private List<LevelObject> SlideInSpritesQueue;
    private List<LevelObject> SlideOutSpritesQueue;

    private float _timer;
    private IEnumerator _lastCoroutine;

    public PlayerData _playerData;
    private AudioSource _audioSource;

    private EnemyPooling _enemyPooling;
    private FXPooling _fxPooling;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _playerData.MusicVolume;

        if (AllLevelSprites.Count == 0)
        {
            foreach (Transform levelsprite in transform.Find("Map").Find("TerrainSprites"))
            {
                AllLevelSprites.Add(levelsprite.gameObject.GetComponent<LevelObject>());
            }
        }
        ColorSpritesQueue = new List<LevelObject>(AllLevelSprites);
        StartGame();
    }

    void IGameState.GameOver()
    {
        GameObject.Find("WaveManager").GetComponent<TutorialManager>()?.EndTutorial();
        StartCoroutine(MusicFadeOut());
    }

    void IGameState.EndGame()
    {
        GameObject.Find("WaveManager").GetComponent<WaveManager>().DisableSpawners();

        _enemyPooling.KillAllEnemies();
        _fxPooling.CancelAllEffects();

        foreach (LevelObject current in ColorSpritesQueue)
        {
            current.Paint();
        }
    }

    void IGameState.UnloadLevel()
    {
        _enemyPooling.KillAllEnemies();
        _fxPooling.CancelAllEffects();

        if (_lastCoroutine == null)
        {
            SlideOutSpritesQueue = new List<LevelObject>(AllLevelSprites);
            StartCoroutine(SlideAllObjectsOut());
        }
    }

    public void StartGame()
    {
        // Debug.Log("start calling lol");
        if(_lastCoroutine == null)
        {
            SlideInSpritesQueue = new List<LevelObject>(AllLevelSprites);
            StartCoroutine(SlideAllObjectsIn());
        }
    }

    void IGameState.NextWave() {
        if(ColorSpritesQueue.Count > 0)
        {
            while(ColorSpritesQueue[0].gameObject.CompareTag("Non Paintable"))
            {
                ColorSpritesQueue.RemoveAt(0);
            }
            ColorSpritesQueue[0].Paint();
            ColorSpritesQueue.RemoveAt(0);
        }
    }

    IEnumerator MusicFadeOut()
    {
        _timer = 1f;
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            _audioSource.volume = _timer;
            yield return null;
        }
        _audioSource.volume = 0f;
    }

    IEnumerator SlideAllObjectsIn()
    {
        _timer = 0;
        while(SlideInSpritesQueue.Count > 0)
        {
            _timer += Time.deltaTime;
            if (_timer > 0.05f)
            {
                SlideInSpritesQueue[0].SlideIn();
                SlideInSpritesQueue.RemoveAt(0);
                _timer = 0;
            }
            yield return null;
        }
    }

    IEnumerator SlideAllObjectsOut()
    {
        _timer = 0;
        while (SlideOutSpritesQueue.Count > 0)
        {
            _timer += Time.deltaTime;
            if (_timer > 0.005f)
            {
                SlideOutSpritesQueue[0].SlideOut();
                SlideOutSpritesQueue.RemoveAt(0);
                _timer = 0;
            }
            yield return null;
        }
    }
}
