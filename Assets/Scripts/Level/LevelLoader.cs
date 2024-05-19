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

    void Start()
    {
        if(AllLevelSprites.Count == 0)
        {
            foreach (Transform levelsprite in transform.Find("Map").Find("TerrainSprites"))
            {
                AllLevelSprites.Add(levelsprite.gameObject.GetComponent<LevelObject>());
            }
        }
        ColorSpritesQueue = new List<LevelObject>(AllLevelSprites);
        StartGame();
    }

    void IGameState.EndGame()
    {

    }

    void IGameState.UnloadLevel()
    {
        foreach (LevelObject obj in AllLevelSprites)
        {
            obj.SlideOut();
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
            ColorSpritesQueue[0].Paint();
            ColorSpritesQueue.RemoveAt(0);
        }
    }

    void Update()
    {
         
    }

    IEnumerator SlideAllObjectsIn()
    {
        _timer = 0;
        while(SlideInSpritesQueue.Count > 0)
        {
            _timer += Time.deltaTime;
            if (_timer > 0.1f)
            {
                SlideInSpritesQueue[0].SlideIn();
                SlideInSpritesQueue.RemoveAt(0);
                _timer = 0;
            }
            yield return null;
        }
    }
}
