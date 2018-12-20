using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void PrepareNewRoundEvent();
    public PrepareNewRoundEvent E_PrepareNewRoundEvent;

    public void PrepareNewRound()
    {
        E_PrepareNewRoundEvent.Invoke();
    }

    public delegate void StartNewRoundEvent();
    public StartNewRoundEvent E_StartNewRoundEvent;

    public void StartNewRound()
    {
        E_StartNewRoundEvent.Invoke();
    }

    public delegate void RollDiceEvent();
    public RollDiceEvent E_RollDiceEvent;

    public void RollDice()
    {
        E_RollDiceEvent.Invoke();
    }

    public delegate void CheckDiceEvent();
    public CheckDiceEvent E_CheckDiceEvent;

    public void CheckDice()
    {
        E_CheckDiceEvent.Invoke();
    }
}
