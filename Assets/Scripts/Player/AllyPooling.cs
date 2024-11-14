using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyPooling : MonoBehaviour
{
    [Tooltip("Add in order of enum")] public GameObject[] EnemyPrefabs;

    private Dictionary<GameObject, List<Ally>> allies = new Dictionary<GameObject, List<Ally>>();    // Dynamic pooling

    private AllySoundManager soundManager;

    public Ally Spawn(GameObject type)
    {
        if (!allies.ContainsKey(type))   // Initialize pool if no key
            allies[type] = new List<Ally>();

        List<Ally> currentList = allies[type];    // grab current pool

        foreach (Ally ally in currentList)
        {
            if (!ally.gameObject.activeSelf)
            {
                return ally;
            }
        }

        Debug.Log("Spawning > " + type);
        GameObject newAllyObj = Instantiate(type).gameObject;
        newAllyObj.transform.parent = transform;
        newAllyObj.SetActive(false);

        Ally newAlly = newAllyObj.GetComponent<Ally>();
        newAlly.SetSoundManager(soundManager);

        currentList.Add(newAlly);

        return newAlly;
    }

    public AllySoundManager GetSoundManager()
    {
        return soundManager;
    }

    public void PoolReset()
    {

    }

    void Start()
    {
        soundManager = GetComponent<AllySoundManager>();
    }
}