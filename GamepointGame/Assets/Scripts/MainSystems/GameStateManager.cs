using UnityEngine;

namespace MainGame
{
    public class GameStateManager : MonoBehaviour
    {
        private GameState m_CurrentGameState;

        public void ChangeGameState(GameState newGameState)
        {
            m_CurrentGameState = newGameState;
        }

        public GameState GetGameState()
        {
            return m_CurrentGameState;
        }
    }

    public enum GameState
    {
        Preparing = 0,
        ReadyToRoll,
        Rollin,
        Rolled
    }
}