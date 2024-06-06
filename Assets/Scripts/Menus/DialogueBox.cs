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

    private Vector3 _originalContinuePosition;

    void Start()
    {
        gameObject.SetActive(false);
        _message.text = "";
        _portrait.sprite = null;
        _originalContinuePosition = _continue.localPosition;
    }

    void Update()
    {
        _continue.localPosition = _originalContinuePosition + (Vector3.right * 20f * Mathf.Cos(Time.realtimeSinceStartup * 8f));
    }

    public void MakeDialogue(string msg, Sprite sprite)
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
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
                gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
    }

    public void BackgroundDialogue(string msg, Sprite sprite)
    {
        gameObject.SetActive(true);
        _continue.gameObject.SetActive(false);
        _message.text = msg;
        _portrait.sprite = sprite;
    }
}
