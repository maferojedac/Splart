using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    public void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
