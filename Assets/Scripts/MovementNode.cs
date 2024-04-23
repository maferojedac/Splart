using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNode : MonoBehaviour
{
    public MovementMap movementMap;
    public bool hasWall;
    void Start()
    {
        movementMap.Register(transform.position, hasWall);
    }
}
