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

    public AudioClip demoClip;

    private AudioSource _audioSource;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // Cargar los valores guardados al iniciar el juego
        MasterVolume.value = playerData.MasterVolume;
        SoundeffectsVolume.value = playerData.SoundeffectsVolume;
        MusicVolume.value = playerData.MusicVolume;

        AudioListener.volume = playerData.MasterVolume;
    }

    public void MasterVolumeChanged()
    {
        playerData.MasterVolume = MasterVolume.value;
        AudioListener.volume = MasterVolume.value;
        // Aquí puedes cambiar el volumen del sonido general del juego
    }

    public void SoundeffectsVolumeChanged()
    {
        playerData.SoundeffectsVolume = SoundeffectsVolume.value;
        _audioSource.volume = SoundeffectsVolume.value;
        _audioSource.PlayOneShot(demoClip);
        // Aquí puedes cambiar el volumen de la música del juego
    }

    public void MusicVolumeChanged()
    {
        playerData.MusicVolume = MusicVolume.value;
        // Aquí puedes cambiar el volumen de los efectos de sonido del juego
    }
}
