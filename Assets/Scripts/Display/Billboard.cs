using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    void LateUpdate()
    {
        transform.rotation = _cam.transform.rotation;
    }
}
