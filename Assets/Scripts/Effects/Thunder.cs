using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{

    public GameObject _target;

    public SpriteRenderer _sprite;

    private float _timer;
    private float _lifetime;

    void Start()
    {
        _lifetime = 1f;
        if( _target != null)
        {
            transform.position = _target.transform.position;
            _target.GetComponent<IEnemy>().OnDie();
        }
    }

    void Update()
    {
        if( _timer < 0)
        {
            _timer = Random.value / 10f;
            _sprite.enabled = Random.value > 0.5f;
            _sprite.flipX = Random.value > 0.5f;
            _sprite.flipY = Random.value > 0.5f;
        }
        else
        {
            _timer -= Time.deltaTime;
        }
        _lifetime -= Time.deltaTime;
        if(_lifetime < 0)
        {
            Destroy(gameObject);
        }
    }
}
