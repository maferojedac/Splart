using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyMovement : MonoBehaviour
{
    public MovementMap NodeMap; // Mapa de nodos

    // Características que cambian el movimiento del enemigo
    [Header("Enemy traits")]
    public float RunSpeed;
    public float TurnSpeed;
    public int Focus;
    public float PathDistraction;
    public float Cowardice;
    public Vector2 StayProtected;

    private GameObject _target;    // A quien sigue el enemigo
    private List<Vector3> _nodes;    // Copia de los nodos del mapa
    private Vector3 _targetNode;     // Siguiente nodo
    private EnemyState _state;     // Estado del enemigo
    void Start()
    {
        _target = GameObject.Find("Player");

        transform.rotation = Quaternion.LookRotation((_target.transform.position - transform.position).normalized);
        _nodes = NodeMap.Path(transform.rotation, PathDistraction, transform.position);
        _targetNode = bestNode();
    }

    void Update()
    {
        // Apuntar y mover enemigo 
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((_targetNode - transform.position).normalized), Time.deltaTime * TurnSpeed);
        transform.position += transform.rotation * Vector3.forward * RunSpeed * Time.deltaTime;

        // Al alcanzar el nodo, decidir que hacer en base al estado del enemigo
        if(_state == EnemyState.Approaching)
        {
            if ((transform.position - _targetNode).magnitude < 0.5f)
            {
                _nodes.Remove(_targetNode);
                _targetNode = bestNode();
                if (_targetNode == Vector3.zero)
                {
                    _state = EnemyState.Attacking;
                    _targetNode = _target.transform.position;
                }
            }
        }else if(_state == EnemyState.Attacking)
        {
            if ((transform.position - _targetNode).magnitude < 2f)
            {
                gameObject.GetComponent<IEnemy>()?.OnReach();
            }
        }
    }

    Vector3 bestNode()
    {
        // Decidir el mejor nodo dentro de los siguientes Focus nodos
        Vector3 Chosen = Vector3.zero;
        float MaxScore = 0;
        Vector3 TargetDirection = (_target.transform.position - transform.position);
        
        Quaternion.LookRotation((_target.transform.position - transform.position).normalized);
        for (int i = 0; i < Focus && i < _nodes.Count; i++)
        {
            Vector3 NodeDirection = (_nodes[0] - transform.position); ;
            float TargetAngle = Vector3.Angle(TargetDirection, NodeDirection);
            float Score = Mathf.Abs(Mathf.Cos(TargetAngle)) * NodeDirection.magnitude;
            if(Score > MaxScore)
            {
                MaxScore = Score;
                Chosen = _nodes[0];
            }
            _nodes.RemoveAt(0);
        }

        return Chosen;
    }
}
