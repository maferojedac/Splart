using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{

    [Tooltip("Add in order of enum")] public GameObject[] EnemyPrefabs;

    private Dictionary<GameObject, List<Enemy>> enemies = new Dictionary<GameObject, List<Enemy>>();    // Dynamic pooling

    private EnemySoundManager soundManager;

    public Enemy Spawn(GameObject type)
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

        Debug.Log("Spawning > "+ type);
        GameObject newEnemyObj = Instantiate(type);
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