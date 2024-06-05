using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMenuAnimation : MonoBehaviour
{

    public Vector3 _exitOffset;

    private bool _isMenuIn;

    private bool _slidingIn;
    private bool _slidingOut;

    private float _timer;

    private Vector3 _exitPosition;
    private Vector3 _displayPosition;

    private Quaternion _exitRotation;
    private Quaternion _displayRotation;


    void Start()
    {
        _exitPosition = _exitOffset;
        _displayPosition = Vector3.zero;

        _exitRotation = Quaternion.Euler(0, 90f, 0);
        _displayRotation = Quaternion.identity;

        transform.localPosition = _exitPosition;
        transform.rotation = _exitRotation;

        _slidingIn = false;
        _slidingOut = false;
    }

    public void Vanish()
    {
        transform.localPosition = _exitPosition;
        transform.rotation = _exitRotation;

        _slidingIn = false;
        _slidingOut = false;

        _isMenuIn = false;

    }

    public void SlideIn()
    {
        if (!_isMenuIn)
        {
            _timer = Time.realtimeSinceStartup;

            transform.localPosition = _exitPosition;
            transform.rotation = _exitRotation;

            _slidingIn = true;
            _slidingOut = false;

            _isMenuIn = true;
        }
    }

    public void SlideOut()
    {
        if (_isMenuIn) {
            _timer = Time.realtimeSinceStartup;

            transform.localPosition = _displayPosition;
            transform.rotation = _displayRotation;

            _slidingIn = false;
            _slidingOut = true;

            _isMenuIn = false;
        }
    }

    void Update()
    {
        float vTime = (Time.realtimeSinceStartup - _timer) * 5f;
        if (_slidingIn)
        {
            transform.localPosition = Vector3.Slerp(_exitPosition, _displayPosition, vTime);
            transform.rotation = Quaternion.Slerp(_exitRotation, _displayRotation, vTime);
        }
        else
        {
            if (_slidingOut)
            {
                transform.localPosition = Vector3.Slerp(_displayPosition, _exitPosition, vTime);
                transform.rotation = Quaternion.Slerp(_displayRotation, _exitRotation, vTime);
            }
        }
    }
}
