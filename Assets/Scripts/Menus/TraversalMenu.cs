using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraversalMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ConfigMenu;
    public GameObject StoreMenu;

    private LevelManager _levelManager;

    public void Start()
    {
        _levelManager = GetComponent<LevelManager>();
        GoToMain();    
    }

    public void StartGame()
    {
        MainMenu.SetActive(false);
        ConfigMenu.SetActive(false);
        StoreMenu.SetActive(false);
        _levelManager.StartGameSequence();
    }

    public void GoToMain()
    {
        MainMenu.SetActive(true);
        ConfigMenu.SetActive(false);
        StoreMenu.SetActive(false);
    }

    public void GoToConfig()
    {
        MainMenu.SetActive(false);
        ConfigMenu.SetActive(true);
        StoreMenu.SetActive(false);
    }

    public void GoToStore()
    {
        MainMenu.SetActive(false);
        ConfigMenu.SetActive(false);
        StoreMenu.SetActive(true);
    }
}
