using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNode : MonoBehaviour
{
    public LevelData movementMap;
    public bool hasWall;
    void Start()
    {
        movementMap.RegisterNode(transform.position, hasWall);
    }
}
