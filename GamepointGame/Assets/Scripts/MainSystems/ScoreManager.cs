using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class ScoreManager : MonoBehaviour
    {
        private GameManager m_GameManager;

        private int m_Currency;
        private int m_Multiplier;
        private int m_CheckingDice;
        private int m_DiceTotal;
        private float m_TimeBetweenChecks;

        private bool m_Checking;

        public void PrepareScriptVariables()
        {
            m_Currency = 200;
            m_Checking = false;
            m_DiceTotal = 0;
            m_CheckingDice = 0;
            m_TimeBetweenChecks = 0.8f;
        }

        public void GetScriptRefs(GameManager gameManager)
        {
            m_GameManager = gameManager;
            m_Multiplier = m_GameManager.GetCurrentRound() * 2;
            m_GameManager.GetEventManager.E_CheckDiceEvent += StartCheckingDice;
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
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Update()
        {
            if (m_Checking)
            {
                m_TimeBetweenChecks -= Time.deltaTime;
                if (m_TimeBetweenChecks <= 0f)
                {
                    CheckDice();
                    m_TimeBetweenChecks = 0.8f;
                }
            }
        }

        private void StartCheckingDice()
        {
            m_Checking = true;
        }

        private void CheckDice()
        {
            List<Dice> dice = new List<Dice>(m_GameManager.GetAllDice());
            m_DiceTotal += dice[m_CheckingDice].GetNumber();
            m_GameManager.GetUIManager.ShowDiceTotal(dice[m_CheckingDice].transform.position, m_DiceTotal);

            if (m_CheckingDice + 1 == dice.Count)
            {
                m_CheckingDice = 0;
                m_Checking = false;
                CheckPlayerPrediction();
            }
            else
            {
                m_CheckingDice++;
            }
        }

        private void CheckPlayerPrediction()
        {
            Prediction prediction = m_GameManager.GetCurrentPrediction();
            int mainNumber = m_GameManager.GetCurrentNumber();

            switch (prediction)
            {
                case Prediction.Equal:
                    if (m_DiceTotal == mainNumber)
                    {
                        m_Currency += (50 * m_GameManager.GetCurrentRound()) + (100 * m_Multiplier);
                        m_GameManager.Won();
                    }
                    else
                    {
                        m_GameManager.Lost();
                    }
                    break;
                case Prediction.Lower:
                    if (m_DiceTotal < mainNumber)
                    {
                        m_Currency += (50 * m_GameManager.GetCurrentRound()) + (50 * m_Multiplier);
                        m_GameManager.Won();
                    }
                    else
                    {
                        m_GameManager.Lost();
                    }
                    break;
                case Prediction.Higher:
                    if (m_DiceTotal > mainNumber)
                    {
                        m_Currency += (50 * m_GameManager.GetCurrentRound()) + (50 * m_Multiplier);
                        m_GameManager.Won();
                    }
                    else
                    {
                        m_GameManager.Lost();
                    }
                    break;
            }
            m_DiceTotal = 0;
            m_Multiplier = m_GameManager.GetCurrentRound() * 2;
        }
    }
}
