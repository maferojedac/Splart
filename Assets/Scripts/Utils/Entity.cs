using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entity
{
    public static List<GameObject> GetAll()
    {
        LayerMask entityLayer = LayerMask.NameToLayer("Entity");

        Transform EnemyTransform = GameObject.Find("Enemies").transform;
        List<GameObject> Entities = new();

        foreach (Transform obj in EnemyTransform) { 
            if(entityLayer.value == obj.gameObject.layer)
            {
                Entities.Add(obj.gameObject);
            }
        }
        return Entities;
    }

    public static void DisableCollision(Collider col)
    {
        LayerMask entityLayer = LayerMask.NameToLayer("Entity");

        Transform EnemyTransform = GameObject.Find("Enemies").transform;

        foreach (Transform obj in EnemyTransform)
        {
            if (entityLayer.value == obj.gameObject.layer)
            {
                Collider otherCol = obj.gameObject.GetComponent<Collider>();
                if(otherCol != null)
                    Physics.IgnoreCollision(col, otherCol);
            }
        }
    }
}
