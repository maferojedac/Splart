using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/MovementMap")]
public class MovementMap : ScriptableObject
{
    public List<Transform> _nodes = new List<Transform>();

    private void OnEnable()
    {
        _nodes.Clear();
    }

    public void Register(Transform transform)
    {
        _nodes.Add(transform);
    }

    public List<Vector3> Path(Quaternion p_atDirection, float p_FOVdegrees, Vector3 p_fromPosition)
    {
        List<Vector3> pathNodes = new List<Vector3>();
        foreach (Transform node in _nodes)
        {
            if (nodeInArea(p_atDirection, p_FOVdegrees, p_fromPosition, node))
                pathNodes.Add(node.position);
        }

        if(pathNodes.Count > 1)
            for (int i = 0; i < pathNodes.Count - 1; i++)
            {
                for (int j = 0; j < pathNodes.Count - i - 1; j++)
                {
                    float DistanceCurrent = (pathNodes[j] - p_fromPosition).magnitude;
                    float DistanceNext = (pathNodes[j + 1] - p_fromPosition).magnitude;
                    if (DistanceCurrent > DistanceNext)
                    {
                        Vector3 temp = pathNodes[j];
                        pathNodes[j] = pathNodes[j + 1];
                        pathNodes[j + 1] = temp;
                    }
                }
            }

        return pathNodes;
    }

    public Vector3 ClosestInPath(Quaternion p_atDirection, float p_FOVdegrees, Vector3 p_fromPosition)
    {
        Vector3 closestNode = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        foreach (Transform node in _nodes)
        {
            if (nodeInArea(p_atDirection, p_FOVdegrees, p_fromPosition, node))
                if ((node.position - p_fromPosition).magnitude < (closestNode - p_fromPosition).magnitude)
                    closestNode = (node.position);
        }

        return closestNode;
    }

    public bool Any(Quaternion p_atDirection, float p_FOVdegrees, Vector3 p_fromPosition)
    {
        foreach (Transform node in _nodes)
        {
            if (nodeInArea(p_atDirection, p_FOVdegrees, p_fromPosition, node))
                return true;
        }
        return false;
    }

    private bool nodeInArea(Quaternion p_atDirection, float p_FOVdegrees, Vector3 p_fromPosition, Transform p_node)
    {
        // Ignore vertical offset in nodes
        p_fromPosition.y = p_node.position.y;
        Quaternion transformAngle = Quaternion.FromToRotation(p_atDirection * Vector3.forward, (p_node.position - p_fromPosition).normalized);
        float diffAngle = Quaternion.Angle(Quaternion.Euler(0, 0, 0), transformAngle);
        if (diffAngle < p_FOVdegrees)
            return true;
        return false;
    }

}
