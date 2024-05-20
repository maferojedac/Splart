using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{

    [SerializeField] private float ColorDelay;
    [SerializeField] private Direction ExitDirection;

    private SpriteRenderer _spriteRenderer;
    private Vector3 _originalHSV;
    private Vector3 _grayscaleHSV;

    private Vector3 _exitPosition;
    private Vector3 _atLevelPosition;

    private float _time;
    private bool _painting;
    private LevelObjectState _state;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _painting = false;
        _time = 0f;

        Vector3 hsv = Vector3.zero;
        Color.RGBToHSV(_spriteRenderer.color, out hsv.x, out hsv.y, out hsv.z) ;
        _originalHSV = hsv;
        hsv.y = 0;
        hsv.z = 1f;
        _grayscaleHSV = hsv;
        _spriteRenderer.color = Color.HSVToRGB(hsv.x, hsv.y, hsv.z);

        _state = LevelObjectState.None;

        _atLevelPosition = transform.position;
        switch (ExitDirection)
        {
            case Direction.Left: _exitPosition = _atLevelPosition - new Vector3(200f, 0, 0); break;
            case Direction.Right: _exitPosition = _atLevelPosition + new Vector3(200f, 0, 0); break;
            case Direction.Up: _exitPosition = _atLevelPosition + new Vector3(0, 250f, 0); break;
            case Direction.Down: _exitPosition = _atLevelPosition - new Vector3(0, 250f, 0); break;
            default: _exitPosition = transform.position; break;
        }
        transform.position = _exitPosition;
    }

    public void SlideIn()
    {
        _painting = false;
        _time = 0;
        transform.position = _exitPosition;
        _state = LevelObjectState.SlidingIn;
    }

    public void SlideOut()
    {
        _painting = false;
        _time = 0;
        transform.position = _atLevelPosition;
        _state = LevelObjectState.SlidingOut;
    }

    public void Paint()
    {
        _time = 0;
        _painting = true;
    }

    public void SetMaterial(Material material)
    {
        _spriteRenderer.material = material;
    }

    void Update() 
    {
        if(_painting)
        {
            _time += Time.deltaTime;
            Vector3 hsv = Vector3.zero;
            hsv = Vector3.Lerp(_grayscaleHSV, _originalHSV, _time);
            _spriteRenderer.color = Color.HSVToRGB(hsv.x, hsv.y, hsv.z);
        }
        else
        {
            if(_state != LevelObjectState.None)
            {
                _time += Time.deltaTime;
                if (_state == LevelObjectState.SlidingIn)
                {
                    transform.position = Vector3.Slerp(_exitPosition, _atLevelPosition, _time);
                }
                if(_state == LevelObjectState.SlidingOut)
                {
                    transform.position = Vector3.Slerp(_atLevelPosition, _exitPosition, _time);
                }
            }
        }
    }
}
