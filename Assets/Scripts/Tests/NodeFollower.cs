using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeFollower : MonoBehaviour
{
    string State;
    public LevelData mp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mp.AnyNodeInPath(transform.rotation, 5f, transform.position))
        {
            transform.position += transform.rotation * Vector3.forward;
        }
        else
        {
            transform.rotation *= Quaternion.Euler(0, 5f, 0);
        }
    }
}
