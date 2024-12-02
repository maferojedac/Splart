using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : Effect
{
    public SpriteRenderer _sprite;

    private float _lifetime;
    
    public override void Execute()
    {
        base.Execute();

        _lifetime = 1f;

        StartCoroutine(ThunderCoroutine());
    }

    IEnumerator ThunderCoroutine()
    {
        while(_lifetime > 0)
        {
            float Stroke = Random.value / 10f;

            _sprite.enabled = Random.value > 0.5f;
            _sprite.flipX = Random.value > 0.5f;
            _sprite.flipY = Random.value > 0.5f;

            _lifetime -= Stroke;
            yield return new WaitForSeconds(Stroke);
        }

        gameObject.SetActive(false);
    }
}
