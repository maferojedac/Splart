using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraversalMenu : MonoBehaviour
{
    public SimpleMenuAnimation MainMenu;
    public SimpleMenuAnimation ConfigMenu;
    public SimpleMenuAnimation StoreMenu;

    public AudioClip onClickSound;

    public PlayerData _playerData;
    private LevelManager _levelManager;

    private AudioSource _audioSource;

    private float _timer;
    private bool _wasMenuDisabled;

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _levelManager = GetComponent<LevelManager>();

        VanishAllMenu();
    }

    void Update()
    {
        _audioSource.volume = _playerData.MusicVolume; // ram eater
    }

    public void VanishAllMenu()
    {
        _audioSource.volume = 0f;

        _wasMenuDisabled = true;

        MainMenu.Vanish();
        ConfigMenu.Vanish();
        StoreMenu.Vanish();
    }

    public void HideAllMenu()
    {
        _wasMenuDisabled = true;

        StopAllCoroutines();

        StartCoroutine(MusicFadeOut());

        _audioSource.PlayOneShot(onClickSound);

        MainMenu.SlideOut();
        ConfigMenu.SlideOut();
        StoreMenu.SlideOut();
    }

    public void StartGame()
    {
        _wasMenuDisabled = true;

        StopAllCoroutines();

        StartCoroutine(MusicFadeOut());

        _audioSource.PlayOneShot(onClickSound);

        MainMenu.SlideOut();
        ConfigMenu.SlideOut();
        StoreMenu.SlideOut();

        _levelManager.ButtonStartGameAction();
    }

    public void GoToMain()
    {
        if (_wasMenuDisabled)
        {
            StopAllCoroutines();

            _audioSource.Play();
            StartCoroutine(MusicFadeIn());
        }

        _wasMenuDisabled = false;

        _audioSource.PlayOneShot(onClickSound);

        MainMenu.SlideIn();
        ConfigMenu.SlideOut();
        StoreMenu.SlideOut();
    }

    public void GoToConfig()
    {
        _audioSource.PlayOneShot(onClickSound);

        MainMenu.SlideOut();
        ConfigMenu.SlideIn();
        StoreMenu.SlideOut();
    }

    public void GoToStore()
    {
        _audioSource.PlayOneShot(onClickSound);

        MainMenu.SlideOut();
        ConfigMenu.SlideOut();
        StoreMenu.SlideIn();
    }

    IEnumerator MusicFadeOut()
    {
        _timer = 1f;
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            _audioSource.volume = _timer * _playerData.MusicVolume;
            yield return null;
        }
        _audioSource.volume = 0f;
        _audioSource.Stop();
    }

    IEnumerator MusicFadeIn()
    {
        _timer = 0;
        while (_timer < 1f)
        {
            _timer += Time.deltaTime;
            _audioSource.volume = _timer * _playerData.MusicVolume;
            yield return null;
        }
        _audioSource.volume = _playerData.MusicVolume;
    }
}
