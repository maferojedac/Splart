using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splatter : MonoBehaviour, IEnemy
{

    private ArrayColor _colors = new();
    public SpriteRenderer spriteRenderer;

    [SerializeField] Splat splat;

    public void OnDie()
    {
        Debug.Log("Player beat me!");
        Destroy(gameObject);
    }

    void IEnemy.OnReach()
    {
        Color splat_color = spriteRenderer.color;
        splat_color.a = 0.5f;
        splat.ChangeColor(splat_color);
        Instantiate(splat);
        Debug.Log("Reached player! I should deal damage here!");
        Destroy(gameObject);
    }

    void IEnemy.TakeDamage(GameColor color)
    {
        _colors.Remove(color);
        spriteRenderer.color = _colors.toRGB();

        if (_colors.Count() == 0)
            OnDie();
    }

    void Start()
    {
        if (spriteRenderer == null)
        {
            Debug.Log("Sprite not found!");
            Destroy(gameObject);
        }

        for(int i = 0; i < Random.Range(1, 4);  i++)
        {
            _colors.Add((GameColor)System.Enum.ToObject(typeof(GameColor), Random.Range(0, 5)));
        }

        spriteRenderer.color = _colors.toRGB();
    }

    void CreateSplat()
    {

    }
}
