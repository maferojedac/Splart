// Map nodes utilized to create levels
// Created by Javier Soto

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public LevelData movementMap;   // Level communication class

    [SerializeField] private MapNode[] nextNode = new MapNode[1];    // Reference to the next node in path
    public bool isSource;       // Can the node be a spawn point at random?

    [DoNotSerialize] public Vector3 Position; // Publicly accesible position for classes that need MapNode

    void OnEnable()
    {
        Debug.Log("Registed nodes!");
        Position = transform.position;  // set position

        StartCoroutine(RegisterNodes());
    }

    IEnumerator RegisterNodes() // Unfortunately, we have to wait one frame
    {
        yield return null;

        movementMap.RegisterNode(this); // Generic map nodes
        if (isSource)
            movementMap.RegisterSourceNode(this); // If publicly accesible mapnodes, register here. Like for enemies who fall from sky, for example.
    }

    public bool IsEnd() {
        return nextNode.Length == 0;
    }

    public MapNode Next()   // Return random node from array
    {
        return nextNode[Random.Range(0, nextNode.Length)];
    }
}
