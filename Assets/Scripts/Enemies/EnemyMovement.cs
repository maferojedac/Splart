using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyMovement : MonoBehaviour
{
    public LevelData NodeMap; // Mapa de nodos

    // Características que cambian el movimiento del enemigo
    [Header("Enemy traits")]
    public float RunSpeed;
    public float TurnSpeed;
    public int Focus;
    public float PathDistraction;

    private GameObject _target;    // A quien sigue el enemigo
    private List<MapNode> _nodes;    // Copia de los nodos del mapa
    private MapNode _targetNode;     // Siguiente nodo
    private EnemyState _state;     // Estado del enemigo

    private Rigidbody _rigidBody;
    void Start()
    {
        _target = GameObject.Find("Player");
        _rigidBody = GetComponent<Rigidbody>();

        transform.rotation = Quaternion.LookRotation((_target.transform.position - transform.position).normalized);
        _nodes = NodeMap.MakePathFromNodes(transform.rotation, PathDistraction, transform.position);
        _targetNode = bestNode();
    }

    private void FixedUpdate()
    {
        float SpeedY = _rigidBody.velocity.y;
        _targetNode.Position.y = transform.position.y;

        // Apuntar y mover enemigo 
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((_targetNode.Position - transform.position).normalized), Time.fixedDeltaTime * TurnSpeed);
        _rigidBody.velocity = transform.rotation * Vector3.forward * RunSpeed;
        _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, SpeedY, _rigidBody.velocity.z);

        // Al alcanzar el nodo, decidir que hacer en base al estado del enemigo
        if (_state == EnemyState.Approaching)
        {
            if ((transform.position - _targetNode.Position).magnitude < 0.5f)
            {
                _nodes.Remove(_targetNode);
                _targetNode = bestNode();
                if (_targetNode.Position == Vector3.zero)
                {
                    _state = EnemyState.Attacking;
                    _targetNode = new MapNode(_target.transform.position);
                }
            }
        }
        else if (_state == EnemyState.Attacking)
        {
            if ((transform.position - _targetNode.Position).magnitude < 2f)
            {
                gameObject.GetComponent<IEnemy>()?.OnReach();
            }
        }
    }

    MapNode bestNode()
    {
        // Decidir el mejor nodo dentro de los siguientes Focus nodos
        MapNode Chosen = new MapNode(0, Vector3.zero, false);
        float MaxScore = 0;
        Vector3 TargetDirection = (_target.transform.position - transform.position);
        
        Quaternion.LookRotation((_target.transform.position - transform.position).normalized);
        for (int i = 0; i < Focus && i < _nodes.Count; i++)
        {
            Vector3 NodeDirection = (_nodes[0].Position - transform.position); ;
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
