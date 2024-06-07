using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{

    public TextMeshProUGUI _message;
    public Image _portrait;
    public Transform _continue;

    public PlayerData _playerData;

    private Vector3 _originalContinuePosition;

    private AudioSource _audioSource;
    private AudioClip _nextClip;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // gameObject.SetActive(false);
        transform.rotation = Quaternion.Euler(0, 90f, 0);

        _message.text = "";
        _portrait.sprite = null;
        _originalContinuePosition = _continue.localPosition;
    }

    void Update()
    {
        _continue.localPosition = _originalContinuePosition + (Vector3.right * 20f * Mathf.Cos(Time.realtimeSinceStartup * 8f));
    }

    public void MakeDialogue(string msg, Sprite sprite, AudioClip clip)
    {
        Time.timeScale = 0f;

        _audioSource.volume = _playerData.SoundeffectsVolume;
        _audioSource.PlayOneShot(clip);


        transform.rotation = Quaternion.Euler(0, 0, 0);

        _message.text = msg;
        _portrait.sprite = sprite;
        _continue.gameObject.SetActive(true);

        StartCoroutine(WaitForUserReaction());
    }

    IEnumerator WaitForUserReaction()
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                Time.timeScale = 1f;

                transform.rotation = Quaternion.Euler(0, 90f, 0);
                break;
            }
            yield return null;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.volume = _playerData.SoundeffectsVolume;
        _audioSource.PlayOneShot(clip);
    }

    public void BackgroundDialogue(string msg, Sprite sprite, AudioClip clip)
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);

        _audioSource.volume = _playerData.SoundeffectsVolume;
        _audioSource.PlayOneShot(clip);

        _continue.gameObject.SetActive(false);
        _message.text = msg;
        _portrait.sprite = sprite;
    }

    public void CancelDialogue()
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.Euler(0, 90f, 0);
        _message.text = "";
        _portrait.sprite = null;
    }
}
