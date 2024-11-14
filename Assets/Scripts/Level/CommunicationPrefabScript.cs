// Script that holds reference to used scriptable objects.
// Add one to scene

// Created by Javier Soto

using UnityEngine;

public class CommunicationPrefabScript : MonoBehaviour
{
    public GameState _gameState;    // Game state communication
    public LevelData _levelData;    // Level communication
    public PlayerData _playerData;  // Player's progress
}
