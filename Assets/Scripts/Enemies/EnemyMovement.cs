// Script that moves an enemy through a set path of nodes and calls OnReach enemy towards player upon reach

// Expected is a class that implements IRusherEnemy on its OnReach method
// Expected is a Rigidbody2D

// Movement should not be modified upon attaching this class, as this script manages it.

// Created by Javier Soto


using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public LevelData NodeMap; // Mapa de nodos

    // Caracteristicas que cambian el movimiento del enemigo
    [Header("Enemy traits")]
    public /*static*/ float RunSpeed;   // Upon discussion, it was decided that all enemies should share the same speed
    public /*static*/ float NodeReachRadius = 0.5f;   // At what distance from node should it be counted as reached
    public /*static*/ float PlayerReachRadius = 10f;   // At what distance from player should it be counted as reached
    public float TurnSpeed;

    private GameObject _target;    // A quien sigue el enemigo
    private MapNode _targetNode;     // Siguiente nodo del mapa
    private Enemy _enemy;          // Who uses the enemy movement script

    private Rigidbody _rigidBody;

    void Awake()
    {
        _target = GameObject.Find("Player");
        _rigidBody = GetComponent<Rigidbody>();
        _enemy = GetComponent<Enemy>();

        Entity.DisableCollision(GetComponent<BoxCollider>());
    }

    void OnEnable()
    {
        if (_targetNode == null) // If no node was assigned at spawner, emergency script to avoid null references
        {
            _targetNode = NodeMap.RandomNode();
            Debug.Log("Node at spawner not assigned!");
        }
    }

    IEnumerator FollowNode()
    {
        while((transform.position - _targetNode.Position).magnitude > NodeReachRadius && _enemy._enemyState == EnemyState.Rush)
        {
            Vector3 newSpeed = _rigidBody.velocity;
            float SpeedY = _rigidBody.velocity.y;
            // _targetNode.Position.y = transform.position.y;   // I don't feel comfortable leaving this commented, but also it would change the Y position of the nodes. Make radius more generous?

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((_targetNode.Position - transform.position).normalized), Time.fixedDeltaTime * TurnSpeed);

            newSpeed = transform.rotation * Vector3.forward * RunSpeed * NodeMap.GetGlobalSpeedMultiplier() * NodeMap.GetGlobalSpeedWaveMultiplier();
            newSpeed.y = SpeedY;

            _rigidBody.velocity = newSpeed;

            yield return null;
        }

        if(_enemy._enemyState == EnemyState.Rush)
        {
            if (_targetNode.IsEnd())
            {
                StartCoroutine(FollowPlayer());
            }
            else
            {
                _targetNode = _targetNode.Next();
                StartCoroutine(FollowNode());
            }
        }
    }

    IEnumerator FollowPlayer()
    {
        while ((transform.position - _target.transform.position).magnitude > PlayerReachRadius && _enemy._enemyState == EnemyState.Rush)
        {
            Vector3 newSpeed = _rigidBody.velocity;
            float SpeedY = _rigidBody.velocity.y;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((_target.transform.position - transform.position).normalized), Time.fixedDeltaTime * TurnSpeed);

            newSpeed = transform.rotation * Vector3.forward * RunSpeed * NodeMap.GetGlobalSpeedMultiplier() * NodeMap.GetGlobalSpeedWaveMultiplier();
            newSpeed.y = SpeedY;

            _rigidBody.velocity = newSpeed;

            yield return null;
        }

        if (_enemy._enemyState == EnemyState.Rush)
            gameObject.GetComponent<IRusherEnemy>()?.OnReach(_target.transform.position);
    }

    public void SetStartingNode(MapNode node)
    {
        _targetNode = node;
    }

    public void StartRunning()
    {
        StartCoroutine(FollowNode());
    }
}
