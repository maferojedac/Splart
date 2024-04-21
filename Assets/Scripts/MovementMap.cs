using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/MovementMap")]
public class MovementMap : ScriptableObject
{
    public List<MapNode> _nodes = new List<MapNode>();

    private void OnEnable()
    {
        _nodes.Clear();
    }

    public void Register(Vector3 p_position, bool p_hasWall)
    {
        _nodes.Add(new MapNode(_nodes.Count, p_position, p_hasWall));
    }

    public List<MapNode> Path(Quaternion p_atDirection, float p_FOVdegrees, Vector3 p_fromPosition)
    {
        List<MapNode> pathNodes = new List<MapNode>();
        foreach (MapNode node in _nodes)
        {
            if (nodeInArea(p_atDirection, p_FOVdegrees, p_fromPosition, node))
                pathNodes.Add(node);
        }

        if(pathNodes.Count > 1)
            for (int i = 0; i < pathNodes.Count - 1; i++)
            {
                for (int j = 0; j < pathNodes.Count - i - 1; j++)
                {
                    float DistanceCurrent = (pathNodes[j].Position - p_fromPosition).magnitude;
                    float DistanceNext = (pathNodes[j + 1].Position - p_fromPosition).magnitude;
                    if (DistanceCurrent > DistanceNext)
                    {
                        MapNode temp = pathNodes[j];
                        pathNodes[j] = pathNodes[j + 1];
                        pathNodes[j + 1] = temp;
                    }
                }
            }

        return pathNodes;
    }

    public MapNode ClosestInPath(Quaternion p_atDirection, float p_FOVdegrees, Vector3 p_fromPosition)
    {
        MapNode closestNode = new MapNode(0, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), false);
        foreach (MapNode node in _nodes)
        {
            if (nodeInArea(p_atDirection, p_FOVdegrees, p_fromPosition, node))
                if ((node.Position - p_fromPosition).magnitude < (closestNode.Position - p_fromPosition).magnitude)
                    closestNode = (node);
        }

        return closestNode;
    }

    public bool Any(Quaternion p_atDirection, float p_FOVdegrees, Vector3 p_fromPosition)
    {
        foreach (MapNode node in _nodes)
        {
            if (nodeInArea(p_atDirection, p_FOVdegrees, p_fromPosition, node))
                return true;
        }
        return false;
    }

    private bool nodeInArea(Quaternion p_atDirection, float p_FOVdegrees, Vector3 p_fromPosition, MapNode p_node)
    {
        // Ignore vertical offset in nodes
        p_fromPosition.y = p_node.Position.y;
        Quaternion transformAngle = Quaternion.FromToRotation(p_atDirection * Vector3.forward, (p_node.Position - p_fromPosition).normalized);
        float diffAngle = Quaternion.Angle(Quaternion.Euler(0, 0, 0), transformAngle);
        if (diffAngle < p_FOVdegrees)
            return true;
        return false;
    }

}
