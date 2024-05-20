using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraversalMenu : MonoBehaviour
{
    public SimpleMenuAnimation MainMenu;
    public SimpleMenuAnimation ConfigMenu;
    public SimpleMenuAnimation StoreMenu;

    private LevelManager _levelManager;

    public void Start()
    {
        _levelManager = GetComponent<LevelManager>();
        GoToMain();    
    }

    public void StartGame()
    {
        MainMenu.SlideOut();
        ConfigMenu.SlideOut();
        StoreMenu.SlideOut();
        _levelManager.ButtonStartGameAction();
    }

    public void GoToMain()
    {
        MainMenu.SlideIn();
        ConfigMenu.SlideOut();
        StoreMenu.SlideOut();
    }

    public void GoToConfig()
    {
        MainMenu.SlideOut();
        ConfigMenu.SlideIn();
        StoreMenu.SlideOut();
    }

    public void GoToStore()
    {
        MainMenu.SlideOut();
        ConfigMenu.SlideOut();
        StoreMenu.SlideIn();
    }
}
