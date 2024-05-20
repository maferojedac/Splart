using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameColor _color;
    public GameObject _target;

    public float _speed;

    // componentes de gameobject
    public SpriteRenderer _spriteRenderer;
    public GameObject _explosion;
    private Rigidbody _rigidBody;
    private bool _released;
    private Animator _animator;
    private float _deathTimer;
    private float _timer;

    private Color _originalColor;
    private float _alpha;

    void Start()
    {
        _alpha = 1f;
        _originalColor = ArrayColor.makeRGB(_color);

        Color newColor = _originalColor;
        newColor.a = _alpha;
        _spriteRenderer.color = newColor;


        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
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
            newColor.a = Mathf.Lerp(_alpha, 1f, _timer);
            _spriteRenderer.color = newColor;

            if (_target == null)
                Kill();
            else
                transform.position += (_target.transform.position - transform.position).normalized * _speed * Time.deltaTime / 2f;
        }
        if (_deathTimer > -10 && _deathTimer < 0)
        {
            GameObject exp = Instantiate(_explosion, transform.position, Quaternion.identity);
            ParticleSystem.MainModule retard = exp.GetComponent<ParticleSystem>().main;
            retard.startColor = _originalColor;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _target)
        {
            other.GetComponent<IEnemy>()?.TakeDamage(_color);
            Kill();
        }
    }

    public void Release()
    {
        _timer = 0f;
        _released = true;
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
