using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNode : MonoBehaviour
{
    public MovementMap movementMap;
    void Start()
    {
        movementMap.Register(transform);
    }
}
