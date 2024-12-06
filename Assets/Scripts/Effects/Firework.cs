// Behavior script for a firework rocket

using System.Collections;
using UnityEngine;

public class Firework : Effect
{
    private ParticleSystem _particleSystem;
    private SpriteRenderer _spriteRenderer;

    public AudioClip FireworkExplosion;

    public float FireworkHeight;

    private Rigidbody2D _rigidbody;

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void SetColor(Color color)
    {
        ParticleSystem.MainModule main = _particleSystem.main;
        main.startColor = color;
        _spriteRenderer.color = color;
    }

    public override void Execute()
    {
        base.Execute();

        // Hide blast
        _particleSystem.Clear();
        _particleSystem.Stop();

        // Show rocket
        _spriteRenderer.enabled = true;

        StartCoroutine(FireworkRocket());
    }

    IEnumerator FireworkRocket()
    {
        float initialVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * FireworkHeight);
        _rigidbody.velocity = initialVelocity * Vector3.up;

        while (_rigidbody.velocity.y > 0.01f)
        {
            yield return null;
        }

        FireworkBlast();
    }

    void FireworkBlast()
    {
        _soundManager.PlaySound(FireworkExplosion, Random.Range(0.8f, 1.3f));

        // Hide rocket
        _spriteRenderer.enabled = false;

        // Show blast
        _particleSystem.Play();
    }
}
