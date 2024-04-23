using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameColor _color;

    // componentes de gameobject
    public SpriteRenderer _spriteRenderer;
    private Rigidbody _rigidBody;

    void Start()
    {
        _spriteRenderer.color = ArrayColor.makeRGB(_color);
        _rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rigidBody.AddForce(new Vector3(0, -1f, 0) * Time.fixedDeltaTime, ForceMode.VelocityChange);
        if (transform.position.y < -50f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IEnemy>()?.TakeDamage(_color);
        Destroy(gameObject);
    }
}
