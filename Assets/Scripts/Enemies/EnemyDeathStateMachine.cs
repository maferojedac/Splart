using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enemy enemyScript = animator.GetComponent<Enemy>();

        enemyScript.OnDeathAnimationEnd();
    }
}
