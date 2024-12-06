// Script to be attached to parent level PreFab.
// Stores the settings such as spawnables for each level.

// Created by Javier Soto

using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    public string LevelName;

    [Header("Spawnables")]
    public GameObject[] SpawnablesCommon;
    public GameObject[] SpawnablesMinibosses;
    public GameObject SpawnablesStageBoss;
    public GameObject[] SpawnablesBonus;

    [Header("Stage painting")]
    public Color[] LevelPalette;
}
