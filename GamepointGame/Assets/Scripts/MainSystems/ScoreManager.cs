using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private GameManager m_GameManager;

    private int m_Currency;
    private int m_Multiplier;

    public void PrepareScriptVariables()
    {
        m_Currency = 100;
    }

    public void GetScriptRefs(GameManager gameManager)
    {
        m_GameManager = gameManager;
        m_Multiplier = m_GameManager.GetCurrentRound() * 2;
        m_GameManager.GetEventManager.E_CheckDiceEvent += CheckAllDiceAndCountThemUp;
    }

    public int GetCurrency()
    {
        return m_Currency;
    }

    public bool PayForRoll(int round)
    {
        int toPay = 50 * round;

        if (toPay <= m_Currency)
        {
            m_Currency -= toPay;
            Debug.Log("Paid: " + toPay);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckAllDiceAndCountThemUp()
    {
        List<Dice> dice = new List<Dice>(m_GameManager.GetAllDice());
        int diceTotal = 0;

        for (int i = 0; i < dice.Count; i++)
        {
            diceTotal += dice[i].GetNumber();
        }

        CheckPlayerPrediction(diceTotal);
    }

    private void CheckPlayerPrediction(int total)
    {
        Prediction prediction = m_GameManager.GetCurrentPrediction();
        int mainNumber = m_GameManager.GetCurrentNumber();

        switch(prediction)
        {
            case Prediction.Equal:
                if (total == mainNumber)
                {
                    m_Currency += (50 * m_GameManager.GetCurrentRound()) + (100 * m_Multiplier);
                    Debug.Log((50 * m_GameManager.GetCurrentRound()) + (100 * m_Multiplier));
                    m_GameManager.Won();
                }
                else
                {
                    m_GameManager.Lost();
                }
                break;
            case Prediction.Lower:
                if (total < mainNumber)
                {
                    m_Currency += (50 * m_GameManager.GetCurrentRound()) + (50 * m_Multiplier);
                    Debug.Log((50 * m_GameManager.GetCurrentRound()) + (50 * m_Multiplier));
                    m_GameManager.Won();
                }
                else
                {
                    m_GameManager.Lost();
                }
                break;
            case Prediction.Higher:
                if (total > mainNumber)
                {
                    m_Currency += (50 * m_GameManager.GetCurrentRound()) + (50 * m_Multiplier);
                    Debug.Log((50 * m_GameManager.GetCurrentRound()) + (50 * m_Multiplier));
                    m_GameManager.Won();
                }
                else
                {
                    m_GameManager.Lost();
                }
                break;
        }

        m_Multiplier = m_GameManager.GetCurrentRound() * 2;
    }
}
