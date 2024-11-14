using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Ally
{
    public SpriteRenderer _spriteRenderer;
    public GameObject _explosion;   // FX pooling

    public AudioClip _release;

    private float _deathTimer;
    private float _timer;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        _spriteRenderer.color = _originalColor;

        _released = false;
        _deathTimer = -10f;
    }

    void FixedUpdate()
    {
        _deathTimer -= Time.deltaTime;
        _timer += Time.deltaTime;
        if (_released)
        {
            Color newColor = _originalColor;
            _spriteRenderer.color = newColor;

            if (_target == null)
                Kill();
            else
                transform.position += (_target.transform.position - transform.position).normalized * _speed * Time.deltaTime / 2f;
        }
        if (_deathTimer > -10 && _deathTimer < 0)
        {
            // GameObject exp = Instantiate(_explosion, transform.position, Quaternion.identity);
            // ParticleSystem.MainModule colorAdjuster = exp.GetComponent<ParticleSystem>().main;
            // colorAdjuster.startColor = _originalColor;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _target)
        {
            other.GetComponent<Enemy>()?.TakeDamage(_color);
            Kill();
        }
    }

    public override void Release()
    {
        base.Release();
        _timer = 0f;

        _soundManager.PlaySound(_release);

        if (_target == null)
        {
            Kill();
        }
    }

    private void Kill()
    {
        _released = false;
        _animator.SetTrigger("die");
        _deathTimer = 0.15f;
    }
}
