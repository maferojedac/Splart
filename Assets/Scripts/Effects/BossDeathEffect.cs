// Effect to allow a celebration after boss is defeated

using System.Collections;
using UnityEngine;

public class BossDeathEffect : Effect
{
    [Header("Celebrations settings")]
    public GameObject FireworkPrefab;
    public float TimeBetweenFireworks;
    public int FireworksAmount;
    public Vector2 FireworkPositionOffset;

    [Header("Communications")]
    public LevelData _levelData;
    private FXPooling _fxPool;

    void Awake()
    {
        _fxPool = GameObject.Find("FX").GetComponent<FXPooling>();
        if(_levelData == null)
            _levelData = GameObject.Find("CommunicationPrefab").GetComponent<CommunicationPrefabScript>()._levelData;
    }

    public override void Execute()
    {
        base.Execute();
        StartCoroutine(Celebrate());
    }

    IEnumerator Celebrate()
    {
        for (int i = 0; i < FireworksAmount; i++)
        {
            MapNode random = _levelData.RandomNode();
            Effect firework = _fxPool.Spawn(FireworkPrefab);

            Vector3 Position = new Vector3(random.Position.x + Random.Range(-FireworkPositionOffset.x, FireworkPositionOffset.x), random.Position.y, random.Position.z + Random.Range(-FireworkPositionOffset.y, FireworkPositionOffset.y));

            firework.SetPosition(Position);
            firework.SetColor(GenerateColor());
            firework.Execute();

            yield return new WaitForSeconds(Random.Range(0, TimeBetweenFireworks));
        }
        yield return new WaitForSeconds(1f);
        _levelData.KillBoss();
    }

    private Color GenerateColor()
    {
        ArrayColor newcolor = new ArrayColor();

        newcolor.Add((GameColor)Random.Range(0, 3));
        newcolor.Add((GameColor)Random.Range(0, 3));

        return newcolor.toRGB();
    }
}
