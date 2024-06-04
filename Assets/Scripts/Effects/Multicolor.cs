using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multicolor : MonoBehaviour
{
    public Material _multicolorMaterial;
    public Material _defaultMaterial;

    private List<LevelObject> _furthestSprites;

    private float _timer;
    private float _strength;

    void Start()
    {
        Debug.Log("Se supone esta corriendo el shader");
        _furthestSprites = new();
        Transform terrainSprites = GameObject.Find("TerrainSprites").transform;

        _strength = 1f;
        _multicolorMaterial.SetFloat("_Strength", _strength);

        if (terrainSprites != null)
        {
            foreach (Transform item in terrainSprites)
            {
                if(item.gameObject.CompareTag("Far Background"))
                {
                    _furthestSprites.Add(item.GetComponent<LevelObject>());
                    item.GetComponent<LevelObject>().SetMaterial(_multicolorMaterial);
                }
            }
            StartCoroutine(DoEffect());
        }
    }

    IEnumerator DoEffect()
    {
        yield return new WaitForSeconds(4f);

        while (_strength > 0f)
        {
            _strength -= Time.deltaTime;
            _multicolorMaterial.SetFloat("_Strength", _strength);
            yield return null;
        }

        foreach(var item in _furthestSprites)
        {
            item.SetMaterial(_defaultMaterial);
        }
        Destroy(gameObject);
    }
}
