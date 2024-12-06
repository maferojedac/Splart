using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyPen : Enemy, IRusherEnemy
{
    [Header("Enemy Setup")]
    public GameObject ScratchPrefab;

    [Header("Blot Sound Effects")]
    public AudioClip _attack;

    private Camera mainCam;

    public override void Spawn(Vector3 position)
    {
        base.Spawn(position);

        mainCam = Camera.main;

        _rigidBody.useGravity = true;
    }

    public override void OnDie()
    {
        base.OnDie();
        if(_levelData._gameRunning)
            _levelData.PaintObject();   // Boss enemies paint the background
    }

    public void OnReach(Vector3 targetPosition)
    {
        _enemyState = EnemyState.Attack;
        _isVulnerable = false;

        Vector3 Speed = (targetPosition - transform.position);
        Speed.y = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * Mathf.Abs(Speed.y));
        Speed *= 5f;

        _rigidBody.AddForce(Speed, ForceMode.Impulse);

        StartCoroutine(OnReachCoroutine(targetPosition));
    }

    IEnumerator OnReachCoroutine(Vector3 targetPosition)
    {
        while (transform.position.z > targetPosition.z) { yield return null; }
        StartCoroutine(ScratchScreen(targetPosition));
    }

    IEnumerator ScratchScreen(Vector3 targetPosition)
    {
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.useGravity = false;

        // Cast effect as ScratchDrawable
        ScratchDrawable effect = (ScratchDrawable) _fxPool.Spawn(ScratchPrefab);
        effect.SetColor(_colors.toRGB());

        _animator.SetTrigger("Attack");

        Vector2 ScreenSizing = new Vector2((Screen.width - 0.2f), (Screen.height - 0.2f));

        Vector2 BrushPosition = new Vector2(Random.value > 0.5 ? 0 : Screen.width, Random.value > 0.5 ? 0 : Screen.height); // Starting position of brush
        float BrushSpeed = (new Vector2(Screen.width / 2, Screen.height / 2) - BrushPosition).magnitude * 2f;
        Vector2 BrushDirection = (new Vector2(Screen.width / 2, Screen.height / 2) - BrushPosition).normalized;
        Vector2 BrushOscilationSpeed = new Vector2(Random.Range(0, 8) * Mathf.PI * 2f, Random.Range(0, 8) * Mathf.PI * 2f);
        float BrushSize = 0;
        float BrushPixelMax = Mathf.RoundToInt(Screen.width / 80);

        float timer = 0f;

        while (timer < 2f)
        {
            // Brush sizing
            float SizeSmoothing = timer * Mathf.PI;
            BrushSize = Mathf.Sin(SizeSmoothing);

            // Brush movement
            BrushPosition += BrushDirection * BrushSpeed * Time.deltaTime;
            BrushPosition += new Vector2(Mathf.Sin(BrushOscilationSpeed.x * timer), Mathf.Sin(BrushOscilationSpeed.y * timer)) * ScreenSizing * Time.deltaTime;

            // Paint on scratchable effect.
            effect.DrawCircle(BrushPosition, Mathf.RoundToInt(BrushSize * BrushPixelMax), false);

            // Show sprite at drawing position
            Vector3 WorldBrushPosition = mainCam.ScreenToWorldPoint(new Vector3(BrushPosition.x, BrushPosition.y, targetPosition.z + 10f));
            transform.position = WorldBrushPosition;

            timer += Time.deltaTime;
            yield return null;
        }

        effect.EnablePlayerInteraction();
        OnAttackAnimationEnd();
    }
}
