using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] PlayerData playerData;
    public void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }

    void OnMouseOver()
    {
        Debug.Log("CLEAN: ");
        Debug.Log(playerData.BoosterClean);
        if (playerData.BoosterClean-1 > -1)
            if (Input.GetMouseButtonDown(0))
            {
                playerData.BoosterClean--;
                Destroy(gameObject);
            }
    }


}
