using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Effect
{
    private ParticleSystem _particleSystem;

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public override void SetColor(Color color)
    {
        ParticleSystem.MainModule main = _particleSystem.main;
        main.startColor = color;
    }

    public override void Execute()
    {
        base.Execute();
        _particleSystem.Clear();
        _particleSystem.Play();
    }
}
