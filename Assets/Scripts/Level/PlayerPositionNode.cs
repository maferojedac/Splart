using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionNode : MonoBehaviour
{

    public LevelData movementMap;

    void Start()
    {
        movementMap.SetPlayerPosition(transform.position);
    }
}
 