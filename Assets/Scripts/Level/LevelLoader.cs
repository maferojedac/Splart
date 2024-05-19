using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour, IGameState
{
    public List<LevelObject> ColorSpritesQueue;

    void IGameState.EndGame()
    {
        Debug.Log("ey");
    }

    void IGameState.StartGame()
    {
        //
    }

    void IGameState.NextWave() {
        ColorSpritesQueue[0].Paint();
        ColorSpritesQueue.RemoveAt(0);
    }

    void Update()
    {
         
    }
}
