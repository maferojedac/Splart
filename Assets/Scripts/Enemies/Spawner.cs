using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Spawnable spawnables;

    private float _timer;
    private float _waitingTime;

    // Componentes de GameObject
    private MeshRenderer _meshRenderer;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _waitingTime = 4f;
    }

    void Update()
    {
        // Generar enemigos
        _timer += Time.deltaTime;
        if(_timer > _waitingTime)
        {
            _waitingTime = Random.Range(1f, 4f);
            _timer = 0f;
            Instantiate(spawnables.Spawnables[Random.Range(0, spawnables.Spawnables.Length)], transform.position, Quaternion.identity);
        }

        // Animacion de spawner
        transform.rotation *= Quaternion.Euler(0, 30f, 0);
        Vector3 hsvColor = Vector3.zero;
        Color.RGBToHSV(_meshRenderer.material.color, out hsvColor.x, out hsvColor.y, out hsvColor.z);
        hsvColor += new Vector3(5f, 0, 0);
        _meshRenderer.material.color = Color.HSVToRGB(hsvColor.x, hsvColor.y, hsvColor.z);
    }
}
