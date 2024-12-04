using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPen : Enemy
{
    [Header("Enemy Setup")]
    public GameObject ScratchPrefab;

    [Header("Blot Sound Effects")]
    public AudioClip _spawn;
    public AudioClip _attack;

    public override void Spawn(Vector3 position)
    {
        base.Spawn(position);
        _isVulnerable = false;

        _soundManager.PlaySound(_spawn);

        _animator.SetTrigger("Attack");
    }

    public override void OnAttack()
    {
        CreateScratch();
    }

    void CreateScratch()
    {
        if (_levelData._gameRunning)
        {
            Effect newScratch = _fxPool.Spawn(ScratchPrefab);
            _soundManager.PlaySound(_attack);

            newScratch.SetColor(_colors.toRGB());
            newScratch.Execute();
        }
    }
}
