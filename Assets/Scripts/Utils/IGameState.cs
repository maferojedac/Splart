// Interface for objects that must be called when something happens between game states.
// Events that are exclusive to GameState

public interface IGameState
{
    void EndGame();
    void StartGame();
    void UnloadLevel() { }
    void NextWave() { }
    void GameOver() { }

}
