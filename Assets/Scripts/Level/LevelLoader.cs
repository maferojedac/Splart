// Script that manages sprites in game as well as other utilities

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour, IGameState, ILevelEvent
{
    public List<LevelObject> AllLevelSprites = new();

    private Dictionary<Color, List<LevelObject>> ColorSpritesQueue = new();
    public List<Color> ColorPickQueue = new();

    private float _timer;
    private int DictionaryIndex;
    private IEnumerator _lastCoroutine;

    private AudioSource _audioSource;

    private LevelSettings _levelSettings;

    private EnemyPooling _enemyPooling;
    private FXPooling _fxPooling;

    [Header("Communication")]
    public LevelData _levelData;
    public PlayerData _playerData;

    [Header("Level Music")]
    public AudioClip _bossMusic;
    public AudioClip _levelMusic;

    public Color _nextPaintingColor;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _levelSettings = GetComponent<LevelSettings>();

        CommunicationPrefabScript communicator = GameObject.Find("CommunicationPrefab").GetComponent<CommunicationPrefabScript>();
        _levelData = communicator._levelData;
        _playerData = communicator._playerData;

        _levelData.SubscribeToEvents(this);

        _enemyPooling = GameObject.Find("Enemies").GetComponent<EnemyPooling>();
        _fxPooling = GameObject.Find("FX").GetComponent<FXPooling>();

        foreach (Transform levelsprite in transform.Find("Map").Find("TerrainSprites"))
        {
            // Get data relevant for adding and clasification
            LevelObject currentLevelObject = levelsprite.GetComponent<LevelObject>();
            SpriteRenderer currentSpriteRenderer = levelsprite.GetComponent<SpriteRenderer>();
            Color closestPaletteColor = ClosestPaletteColor(currentSpriteRenderer.color);

            if (!ColorSpritesQueue.ContainsKey(closestPaletteColor))  // Initialize dictionary for color key if missing
                ColorSpritesQueue[closestPaletteColor] = new List<LevelObject>();

            // Save object
            AllLevelSprites.Add(currentLevelObject);
            ColorSpritesQueue[closestPaletteColor].Add(currentLevelObject);
        }

        StartGame();
    }

    void OnEnable()
    {
        _audioSource.Stop();

        _audioSource.clip = _levelMusic;
        _audioSource.volume = _playerData.MusicVolume;
        _audioSource.Play();

        ColorPickQueue = new List<Color>(_levelSettings.LevelPalette);

        _levelData.SetMaxPaintableColors(ColorPickQueue.Count);
        _levelData.SetLevelName(_levelSettings.LevelName);

        _nextPaintingColor = ColorPickQueue[Random.Range(0, ColorPickQueue.Count)];
    }

    void IGameState.GameOver(bool Victory)
    {
        GameObject.Find("WaveManager").GetComponent<TutorialManager>()?.EndTutorial();
        StartCoroutine(MusicFadeOut());
    }

    void IGameState.EndGame()
    {
        GameObject.Find("WaveManager").GetComponent<WaveManager>().DisableSpawners();

        _enemyPooling.KillAllEnemies();
        _fxPooling.CancelAllEffects();
    }

    void IGameState.UnloadLevel()
    {
        _enemyPooling.KillAllEnemies();
        _fxPooling.CancelAllEffects();

        if (_lastCoroutine == null)
        {
            StartCoroutine(SlideAllObjectsOut());
        }
    }

    public void StartGame()
    {
        // Debug.Log("start calling lol");
        if(_lastCoroutine == null)
        {
            StartCoroutine(SlideAllObjectsIn());
        }
    }

    public void PaintObject() {
        if(ColorPickQueue.Count > 0 && gameObject.activeInHierarchy)
        {
            foreach(LevelObject levelobj in ColorSpritesQueue[_nextPaintingColor])
            {
                levelobj.Paint();
            }

            ColorPickQueue.Remove(_nextPaintingColor);

            if (ColorPickQueue.Count > 0)
                _nextPaintingColor = ColorPickQueue[Random.Range(0, ColorPickQueue.Count)];
            else
            {
                _levelData.SpawnBoss();
                StartCoroutine(PlayBossMusic());
            }
        }
    }

    IEnumerator PlayBossMusic()
    {
        yield return StartCoroutine(MusicFadeOut());
        _audioSource.Stop();

        _audioSource.clip = _bossMusic;
        _audioSource.volume = _playerData.MusicVolume;
        _audioSource.Play();
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
        int Index = 0;
        while(Index < AllLevelSprites.Count)
        {
            AllLevelSprites[Index].SlideIn();
            Index++;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator SlideAllObjectsOut()
    {
        _timer = 0;
        int Index = 0;
        while (Index < AllLevelSprites.Count)
        {
            AllLevelSprites[Index].SlideOut();
            Index++;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private Color ClosestPaletteColor(Color inputColor)
    {
        float InputHue;
        Color.RGBToHSV(inputColor, out InputHue, out _, out _);

        Color closestColor = Color.white;
        float closestDistance = 1f;

        foreach(Color color in _levelSettings.LevelPalette)
        {
            float PaletteItemHue;
            Color.RGBToHSV(color, out PaletteItemHue, out _, out _);

            float currentDistance = CompareHues(InputHue, PaletteItemHue);

            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestColor = color;
            }
        }

        return closestColor;
    }

    private float CompareHues(float hue1, float hue2)
    {
        // Calculate the absolute difference
        float difference = Mathf.Abs(hue1 - hue2);

        // Wrap if there is shorter distance around
        if (difference > 0.5f)
        {
            difference = 1f - difference;
        }

        return difference;
    }
}
