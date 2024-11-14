using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    private AudioSource _audioSource;

    public PlayerData _playerData;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _playerData.SoundeffectsVolume;
    }

    public void PlaySound(AudioClip clip, float pitch = 1f)
    {
        _audioSource.pitch = pitch;
        _audioSource.PlayOneShot(clip);
    }
}
