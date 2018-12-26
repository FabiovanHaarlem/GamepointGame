using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class ScoreManager : MonoBehaviour
    {
        private GameManager m_GameManager;

        private float m_Money;
        private float m_TempCurrency;
        private float m_AmountToAddEachSecond;
        private float m_Multiplier;
        private float m_TimeBetweenChecks;

        private int m_CheckingDice;
        private int m_DiceTotal;

        private bool m_Checking;

        public void PrepareScriptVariables()
        {
            m_Checking = false;
            m_DiceTotal = 0;
            m_CheckingDice = 0;
            m_TimeBetweenChecks = 0.8f;
        }

        public void GetScriptRefs(GameManager gameManager)
        {
            m_GameManager = gameManager;
            m_Money = m_GameManager.GetGlobalGameValue().m_StartingCurrency;
            m_Multiplier = m_GameManager.GetCurrentRound() * m_GameManager.GetGlobalGameValue().m_RoundMultiplierBonus;
        }

        public int GetMoney()
        {
            return Mathf.RoundToInt(m_Money);
        }

        //Checks if player can pay to roll the dice
        public bool PayForRoll(int round)
        {
            int toPay = m_GameManager.GetGlobalGameValue().m_RollPrice * round;

            if (toPay <= m_Money)
            {
                m_Money -= toPay;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Update()
        {
            if (m_Checking) // Checks 1 dice every X seconds this is for the count up effect
            {
                m_TimeBetweenChecks -= Time.deltaTime;
                if (m_TimeBetweenChecks <= 0f)
                {
                    CheckDice();
                    m_TimeBetweenChecks = 0.8f;
                }
            }

            if (m_TempCurrency > 0f)
            {
                float delta = Time.deltaTime;
                if (m_TempCurrency >= delta)
                {
                    float amount = m_AmountToAddEachSecond * delta;
                    m_TempCurrency -= amount;
                    m_Money += amount;

                    if (m_TempCurrency <= amount)
                    {
                        m_Money += m_TempCurrency;
                        m_TempCurrency = 0f;
                    }
                }
                m_GameManager.GetEventManager().RefreshUI();
            }
        }

        public void StartCheckingDice()
        {
            m_Checking = true;
        }

        /*Checks all dice numbers and counts them up
        Then checks what the player predicted*/
        private void CheckDice()
        {
            List<Dice> dice = new List<Dice>(m_GameManager.GetActiveDice());
            m_DiceTotal += dice[m_CheckingDice].GetNumber();
            m_GameManager.GetUIManager().ShowDiceTotal(dice[m_CheckingDice].transform.position, m_DiceTotal); //Activated the floating number above the dice

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

        //Checks if playe was right and gives points
        private void CheckPlayerPrediction()
        {
            Prediction prediction = m_GameManager.GetCurrentPrediction();
            int mainNumber = m_GameManager.GetCurrentNumber();
            int amountOfDice = m_GameManager.GetActiveDice().Count;

            #region Multiplier Calculations
            int max = amountOfDice * 6;
            //Calculates the % diffrence from the: main number - amount; and uses that as a multiplier for the higher roll 
            float higherMultiplier = ((100f * (mainNumber - amountOfDice)) / max) / 12f;

            //Calculates the % diffrence from the: max dice roll - main number; and uses that as a multiplier for the lower roll 
            float lowerMultiplier = ((100f * (max - mainNumber)) / max) / 12f;

            //Calculates the % diffrence from the: max dice roll - amount of dice - 1; and uses that as a multiplier for the equal roll
            float equalMultiplier = ((100f * ((max - amountOfDice) - 1f) / max) / 10f);
            #endregion

            switch (prediction)
            {
                case Prediction.Equal: //If the player prediction = Equal
                    if (m_DiceTotal == mainNumber)
                    {
                        //price = roll price:(100f * amount of rounds) + normale multiplier:(50f * amount of rounds * 1.2f) * equal % calculation(calculation is above)
                        float price = (m_GameManager.GetGlobalGameValue().m_RollPrice * m_GameManager.GetCurrentRound()) + 
                            ((m_GameManager.GetGlobalGameValue().m_EqualWinFlatAmount * m_Multiplier) * equalMultiplier);
                        m_TempCurrency += price;
                        m_GameManager.Won();
                    }
                    else
                    {
                        m_GameManager.Lost();
                    }
                    break;
                case Prediction.Lower: //If the player prediction = Lower
                    if (m_DiceTotal < mainNumber)
                    {
                        //price = roll price:(100f * amount of rounds) + normale multiplier:(25f * amount of rounds * 1.2f) * lower % calculation(calculation is above)
                        float price = (m_GameManager.GetGlobalGameValue().m_RollPrice * m_GameManager.GetCurrentRound()) + 
                            ((m_GameManager.GetGlobalGameValue().m_LowerWinFlatAmount * m_Multiplier) * lowerMultiplier);
                        m_TempCurrency += price;
                        m_GameManager.Won();
                    }
                    else
                    {
                        m_GameManager.Lost();
                    }
                    break;
                case Prediction.Higher: //If the player prediction = Higher
                    if (m_DiceTotal > mainNumber)
                    {
                        //price = roll price:(100f * amount of rounds) + normale multiplier:(25f * amount of rounds * 1.2f) * higher % calculation(calculation is above)
                        float price = (m_GameManager.GetGlobalGameValue().m_RollPrice * m_GameManager.GetCurrentRound()) + 
                            ((m_GameManager.GetGlobalGameValue().m_HigherWinFlatAmount * m_Multiplier) * higherMultiplier);
                        m_TempCurrency += price;
                        m_GameManager.Won();
                    }
                    else
                    {
                        m_GameManager.Lost();
                    }
                    break;
            }
            m_AmountToAddEachSecond = m_TempCurrency / 2f;
            m_DiceTotal = 0;
            m_Multiplier = m_GameManager.GetCurrentRound() * m_GameManager.GetGlobalGameValue().m_RoundMultiplierBonus;
        }
    }
}
