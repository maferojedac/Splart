using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour, IGameState
{
    void IGameState.EndGame()
    {
        Debug.Log("ey");
    }

    void IGameState.StartGame()
    {
        //
    }

    void Update()
    {
         
    }
}
