using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode
{
    int id;
    public Vector3 Position;
    public bool hasWall;

    public MapNode(int p_id, Vector3 p_position, bool p_hasWall)
    {
        this.id = p_id;
        this.Position = p_position;
        this.hasWall = p_hasWall;
    }

    public MapNode()
    {
        id = 0;
        Position = Vector3.zero;
        hasWall = false;
    }

    public MapNode(Vector3 p_position)
    {
        this.id = 0;
        this.Position = p_position;
        this.hasWall = false;
    }

    public bool Is(int p_id)
    {
        return id == p_id;
    }
}
