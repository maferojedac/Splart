using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{

    [Tooltip("Add in order of enum")] public GameObject[] EnemyPrefabs;

    private Dictionary<EnemyType, List<Enemy>> enemies = new Dictionary<EnemyType, List<Enemy>>();    // Dynamic pooling

    private EnemySoundManager soundManager;

    public Enemy Spawn(EnemyType type)
    {
        if(!enemies.ContainsKey(type))   // Initialize pool if no key
            enemies[type] = new List<Enemy>();

        List<Enemy> currentList = enemies[type];    // grab current pool

        foreach (Enemy enemy in currentList)
        {
            if (!enemy.gameObject.activeSelf)
            {
                return enemy;
            }
        }

        GameObject newEnemyObj = Instantiate(EnemyPrefabs[(int) type]);
        newEnemyObj.transform.parent = transform;
        newEnemyObj.SetActive(false);

        Enemy newEnemy = newEnemyObj.GetComponent<Enemy>();

        currentList.Add(newEnemy);

        return newEnemy;
    }

    public EnemySoundManager GetSoundManager()
    {
        return soundManager;
    }


    public void PoolReset()
    {
        
    }

    void Start()
    {
        soundManager = GetComponent<EnemySoundManager>();
    }
}

public enum EnemyType
{
    Blot,
    Sumo,
    Mage,
    Pen,
    Coin,
    Piggy,
}