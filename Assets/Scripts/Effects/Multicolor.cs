using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multicolor : MonoBehaviour
{
    public Material _multicolorMaterial;
    public Material _defaultMaterial;

    private LevelObject[] _furthestSprites;

    void Start()
    {
        Debug.Log("Se supone esta corriendo el shader");
        _furthestSprites = new LevelObject[5];
        GameObject terrainSprites = GameObject.Find("TerrainSprites");
        if(terrainSprites != null)
        {
            for (int i = 0; i < 5; i++)
            {
                _furthestSprites[i] = terrainSprites.transform.GetChild(i).gameObject.GetComponent<LevelObject>();
                _furthestSprites[i].SetMaterial(_multicolorMaterial);
            }
            StartCoroutine(DoEffect());
        }
    }

    IEnumerator DoEffect()
    {
        yield return new WaitForSeconds(5f);
        foreach(var f in _furthestSprites)
        {
            f.SetMaterial(_defaultMaterial);
        }
        Destroy(gameObject);
    }
}
