using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirstTimeBootChecker : MonoBehaviour
{
    public UnityEvent ThrowTutorial;
    public UnityEvent ThrowMenus;

    public PlayerData playerData;

    void Start()
    {
        ThrowMenus.Invoke();
        // StartCoroutine(DelayCheck());
        // Skip tutorial for now
    }

    IEnumerator DelayCheck()
    {
        yield return new WaitForEndOfFrame();
        if (PlayerPrefs.HasKey("FirstBoot"))
        {
            ThrowMenus.Invoke();
        }
        else
        {
            playerData.MasterVolume = 1f;
            playerData.SoundeffectsVolume = 1f;
            playerData.MusicVolume = 1f;

            playerData.SaveData();

            ThrowTutorial.Invoke();
        }
    }
}