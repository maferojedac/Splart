using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMage : Enemy
{
    // public GameObject Effect;

    [Header("Enemy Setup")]
    public float ColorCycleSpeed = 1;
    public float FloatingYAmount = 3;

    public GameObject BossDeathEffect;
    public GameObject InkBlot;

    [Header("Sound clips")]
    public AudioClip _charge;
    public AudioClip _attack;
    public AudioClip _disappear;
    public AudioClip _appear;

    public int life;
    private int teleportsLeft;

    private bool isCyclingColors;

    private Coroutine actionCoroutine;

    private MapNode _nextNode;

    public override void OnDamageTaken()
    {
        if (_colors.Count() == 1 && life > 0)
        {
            _colors.Add(GameColor.White);
            _isVulnerable = false;

            life--;

            _soundManager.PlaySound(_die);

            StopAllCoroutines();

            actionCoroutine = StartCoroutine(Recover());
        }
    }

    public override void OnDie()
    {
        StopCoroutine(actionCoroutine);
        if(_levelData._gameRunning)
            _fxPool.Spawn(BossDeathEffect);
    }

    public override void Spawn(Vector3 position)
    {
        base.Spawn(position);

        Entity.DisableCollision(GetComponent<Collider>());

        life = 2;
        _isVulnerable = true;

        GenerateColor();
        _spriteRenderer.color = _colors.toRGB();

        transform.position = Vector3.one * -1000;   // Disappear from view

        teleportsLeft = GenerateTeleportAmount();

        actionCoroutine = StartCoroutine(Teleport());
    }

    IEnumerator CycleColors()
    {
        float hue = 0;
        while (isCyclingColors)
        {
            hue += Time.deltaTime * ColorCycleSpeed;
            if (hue >= 1)
                hue = 0;
            _spriteRenderer.color = Color.HSVToRGB(hue, 1, 1);
            yield return null;
        }
    }

    IEnumerator ChargeAttack()
    {
        _animator.SetTrigger("StaffUp");

        GenerateColor();
        _spriteRenderer.color = _colors.toRGB();

        _rigidBody.velocity = Vector3.zero;

        isCyclingColors = false;

        _isVulnerable = true;

        _soundManager.PlaySound(_charge);

        yield return new WaitForSeconds(1f);
        _animator.SetTrigger("Reset");

        yield return new WaitForSeconds(10f);
        Attack();
        _animator.SetTrigger("StaffDown");

        teleportsLeft = GenerateTeleportAmount();

        actionCoroutine = StartCoroutine(Teleport());
    }

    IEnumerator Teleport()
    {
        _isVulnerable = false;
        if (!isCyclingColors)
        {
            isCyclingColors = true;
            StartCoroutine(CycleColors());
        }

        teleportsLeft--;

        _soundManager.PlaySound(_disappear);

        _rigidBody.velocity = Vector3.zero;

        _animator.SetTrigger("TeleportOut");
        yield return new WaitForSeconds(1);

        _nextNode = _levelData.RandomNode();
        transform.position = _nextNode.Position + (FloatingYAmount * Vector3.up);
        yield return new WaitForSeconds(0.5f);

        _soundManager.PlaySound(_appear);
        _animator.SetTrigger("TeleportIn");
        yield return new WaitForSeconds(0.5f);

        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        yield return new WaitForSeconds(1f);

        if(teleportsLeft > 0)
            actionCoroutine = StartCoroutine(Teleport());
        else
            actionCoroutine = StartCoroutine(ChargeAttack());
    }

    IEnumerator Recover()
    {
        _animator.SetTrigger("Damage");

        yield return new WaitForSeconds(2);

        teleportsLeft = GenerateTeleportAmount();
        actionCoroutine = StartCoroutine(Teleport());
    }

    private void Attack()
    {
        _soundManager.PlaySound(_attack);

        Effect newSplat = _fxPool.Spawn(InkBlot);
        newSplat.SetColor(_colors.toRGB());

        GameObject.Find("Player").GetComponent<Player>().TakeDamage();
    }

    private int GenerateTeleportAmount()
    {
        return Random.Range(2, 5);
    }

    private void GenerateColor()
    {
        _colors = new ArrayColor();
        for (int i = 0; i < Random.Range(4, 6); i++)
        {
            _colors.Add((GameColor)Random.Range(0, 3));
        }
        _originalColorCount = _colors.Count();
    }
}
