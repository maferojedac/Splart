using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeControl : MonoBehaviour
{

    public float Speed;
    private 
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += transform.rotation * Vector3.forward * Speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.rotation * Vector3.forward * Speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation *= Quaternion.Euler(0, Speed, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation *= Quaternion.Euler(0, -Speed, 0f);
        }
    }
}
