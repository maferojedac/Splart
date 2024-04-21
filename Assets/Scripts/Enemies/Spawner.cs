using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<SpawnableObject> _spawnableQueue = new();
    public SpawnTracker _spawnTracker;

    private float _timer;

    private bool _generating;

    // Componentes de GameObject
    private MeshRenderer _meshRenderer;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _spawnTracker.Register(this);
    }

    void Update()
    {
        // Generar enemigos
        _timer += Time.deltaTime;
        if(_spawnableQueue.Count > 0)
        {
            _generating = true;
            if (_timer > _spawnableQueue[0].Delay)
            {
                _timer = 0f;
                Instantiate(_spawnableQueue[0].SpawnObject, transform.position, Quaternion.identity);
                _spawnableQueue.RemoveAt(0);
            }
        }
        else
        {
            _generating = false;
            _timer = 0f;
        }

        // Animacion de spawner
        transform.rotation *= Quaternion.Euler(0, 360f * Time.deltaTime, 0);
        Vector3 hsvColor = Vector3.zero;
        Color.RGBToHSV(_meshRenderer.material.color, out hsvColor.x, out hsvColor.y, out hsvColor.z);
        hsvColor += new Vector3(1f * Time.deltaTime, 0, 0);
        _meshRenderer.material.color = Color.HSVToRGB(hsvColor.x, hsvColor.y, hsvColor.z);
    }

    public bool Done()
    {
        return !_generating;
    }
}
