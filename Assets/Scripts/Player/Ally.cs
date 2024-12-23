using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    protected GameColor _color;
    protected GameObject _target;

    protected Color _originalColor;

    public float _speed;
    protected bool _released;

    protected PlayerData _playerData;
    protected SoundManager _soundManager;
    protected FXPooling _fxPool;

    protected Rigidbody _rigidBody;
    protected Animator _animator;

    public virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _fxPool = GameObject.Find("FX").GetComponent<FXPooling>();
    }

    public virtual void OnEnable()
    {
        _originalColor = ArrayColor.makeRGB(_color);
    }

    public virtual void Release()
    {
        _released = true;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public void SetColor(GameColor color)
    {
        _color = color;
    }

    public void SetSoundManager(SoundManager manager)
    {
        _soundManager = manager;
    }
}
