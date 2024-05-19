using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidersControl : MonoBehaviour
{
    public Slider MasterVolume;
    public Slider SoundeffectsVolume;
    public Slider MusicVolume;

    public PlayerData playerData;

    void Start()
    {
        // Cargar los valores guardados al iniciar el juego
        MasterVolume.value = playerData.MasterVolume;
        SoundeffectsVolume.value = playerData.SoundeffectsVolume;
        MusicVolume.value = playerData.MusicVolume;
    }

    public void MasterVolumeChanged()
    {
        playerData.MasterVolume = MasterVolume.value;
        // Aqu� puedes cambiar el volumen del sonido general del juego
    }

    public void SoundeffectsVolumeChanged()
    {
        playerData.SoundeffectsVolume = SoundeffectsVolume.value;
        // Aqu� puedes cambiar el volumen de la m�sica del juego
    }

    public void MusicVolumeChanged()
    {
        playerData.MusicVolume = MusicVolume.value;
        // Aqu� puedes cambiar el volumen de los efectos de sonido del juego
    }
}