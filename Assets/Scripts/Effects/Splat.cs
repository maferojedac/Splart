using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] PlayerData playerData;

    private float _timer;

    public void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void Remove()
    {
        Debug.Log("Remove splat thing");
        StartCoroutine(RemoveSplat());
    }

    IEnumerator RemoveSplat()
    {
        _timer = 0f;
        Debug.Log("REmove screen splat coroutine");
        while(_timer < 1f)
        {
            _timer += Time.deltaTime * 2f;

            Color mycolor = spriteRenderer.color;
            mycolor.a = 1 - _timer;
            spriteRenderer.color = mycolor;

            yield return null;
        }
        Destroy(gameObject);
    }


}
