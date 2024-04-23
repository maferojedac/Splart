using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/SpawnTracker")]
public class SpawnTracker : ScriptableObject
{

    public Spawnable spawnableEnemies;

    List<Spawner> _spawners = new List<Spawner>();

    private void OnEnable()
    {
        _spawners.Clear();
    }

    public void Register(Spawner spawner)
    {
        _spawners.Add(spawner);
    }

    public bool AllDone()
    {
        foreach(Spawner spawner in _spawners)
        {
            if (!spawner.Done())
                return false;
        }
        return true;
    }

    public void GenerateWave(int Wave)
    {
        int Score = Wave * 2;
        float MasterDelay = 0f;
        List<SpawnableObject> GeneratedWave = new();
        while(Score > 0)
        {
            GeneratedWave.Add(new SpawnableObject(MasterDelay, spawnableEnemies.Spawnables[Random.Range(0, spawnableEnemies.Spawnables.Length)]));
            if (GeneratedWave.Count > 0 && GeneratedWave.Count % _spawners.Count == 0)
                MasterDelay += Random.Range(5f, 6f);
            else
                MasterDelay += Random.Range(0f, 1f);
            Score--;
        }

        int Count = 0;
        foreach(SpawnableObject spawnableObject in GeneratedWave)
        {
            _spawners[Count % _spawners.Count]._spawnableQueue.Add(spawnableObject);
            Count++;
        }
    }
}
