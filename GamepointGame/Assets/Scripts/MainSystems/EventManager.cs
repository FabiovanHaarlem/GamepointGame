using UnityEngine;
namespace MainGame
{
    public class EventManager : MonoBehaviour
    {
        public delegate void RefreshUIEvent();
        public RefreshUIEvent E_RefreshUIEvent;

        //Is called when the game is loaded and when the player has won or lost a round
        public void RefreshUI()
        {
            E_RefreshUIEvent.Invoke();
        }
    }
}
