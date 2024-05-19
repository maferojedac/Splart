using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] LevelData _levelData;

    private bool _slidingIn;
    private bool _slidingOut;

    private float _timer;

    private Vector3 _exitPosition;
    private Vector3 _displayPosition;

    private Quaternion _exitRotation;
    private Quaternion _displayRotation;

    void Start()
    {
        _exitPosition = new Vector3(0, 1500f, 0);
        _displayPosition = Vector3.zero;

        _exitRotation = Quaternion.Euler(0, 180f, 0);
        _displayRotation = Quaternion.identity;

        transform.localPosition = _exitPosition;
        transform.rotation = _exitRotation;
        Time.timeScale = 1f;
        _slidingIn = false;
        _slidingOut = false;
    }

    public void Vanish()
    {
        transform.localPosition = _exitPosition;
        transform.rotation = _exitRotation;

        _slidingIn = false;
        _slidingOut = false;
    }

    public void PauseGame()
    {
        if( !_slidingIn )
        {
            _timer = Time.realtimeSinceStartup;
            transform.localPosition = _exitPosition;
            transform.rotation = _exitRotation;
            Time.timeScale = 0f;
            _slidingIn = true;
            _slidingOut = false;
        }
    }

    public void ResumeGame()
    {
        if ( _slidingIn && !_slidingOut)
        {
            _timer = Time.realtimeSinceStartup;
            transform.localPosition = _displayPosition;
            transform.rotation = _displayRotation;
            Time.timeScale = 1f;
            _slidingIn = false;
            _slidingOut = true;
        }
    }

    public void SlideOut()
    {
        _timer = Time.realtimeSinceStartup;
        transform.localPosition = _displayPosition;
        transform.rotation = _displayRotation;
        Time.timeScale = 1f;
        _slidingIn = false;
        _slidingOut = true;
    }

    void Update()
    {
        float vTime = (Time.realtimeSinceStartup - _timer) * 5f;
        if( _slidingIn )
        {
            transform.localPosition = Vector3.Slerp(_exitPosition, _displayPosition, vTime);
            transform.rotation = Quaternion.Slerp(_exitRotation, _displayRotation, vTime);
        }
        else
        {
            if( _slidingOut )
            {
                transform.localPosition = Vector3.Slerp(_displayPosition, _exitPosition, vTime);
                transform.rotation = Quaternion.Slerp(_displayRotation, _exitRotation, vTime);
            }
        }
    }
}
