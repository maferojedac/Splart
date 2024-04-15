using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrabbyPathy : MonoBehaviour
{
    public Transform flayer;
    public MovementMap mp;
    public float Degrees;
    public Material cholorer;
    public List<Vector3> noves;
    // Start is called before the first frame update
    void Update()
    {
        noves = mp.Path(flayer.rotation, Degrees, flayer.position);
        if (mp.Path(flayer.rotation, Degrees, flayer.position).Count > 0)
            cholorer.color = Color.white;
        else
            cholorer.color = Color.red;
    }
}
