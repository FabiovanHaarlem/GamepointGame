using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class UIManager : MonoBehaviour
    {
        private GameManager m_GameManager;

        [SerializeField] private List<Image> m_PredictionButtons;

        private Text m_RandomNumber;
        private Text m_NumberTip;
        private Text m_Currency;
        private Text m_PayAmountForRoll;
        private Text m_LastNumber;
        private Text m_RolledNumber;
        private Text m_DiceOnNextRoll;

        private ShowDiceTotal m_ShowDiceTotal;

        private GameObject m_RollDiceButton;

        public void GetScriptRefs(GameManager gameManager)
        {
            m_GameManager = gameManager;
            EventManager eventManager = m_GameManager.GetEventManager();
            eventManager.E_RefreshUIEvent += RefreshInformationUI;
            eventManager.E_RefreshUIEvent += ResetButtonColors;
            FindAllUIObjects();
            m_ShowDiceTotal.gameObject.SetActive(false);
            DisableRollDiceButton();
            RefreshRolledNumber(0);
        }

        //Finds all needed UI elements at the start of the game only
        private void FindAllUIObjects()
        {
            m_RandomNumber = GameObject.Find("RandomNumber").GetComponent<Text>();
            m_NumberTip = GameObject.Find("NumberTip").GetComponent<Text>();
            m_Currency = GameObject.Find("Currency").GetComponent<Text>();
            m_PayAmountForRoll = GameObject.Find("ToPayForRoll").GetComponent<Text>();
            m_RollDiceButton = GameObject.Find("RollDiceButton");
            m_LastNumber = GameObject.Find("PreviousNumber").GetComponent<Text>();
            m_ShowDiceTotal = GameObject.Find("TextBorderShowDiceTotal").GetComponent<ShowDiceTotal>();
            m_RolledNumber = GameObject.Find("RolledNumber").GetComponent<Text>();
            m_DiceOnNextRoll = GameObject.Find("NextRollDice").GetComponent<Text>();
        }

        //Refreshes all UI that gives the player more information about the game
        public void RefreshInformationUI()
        {
            int amountOfDice = m_GameManager.GetCurrentRound() + 1;
            int maxAmountRoll = 6 * amountOfDice;
            m_Currency.text = "Money" + Environment.NewLine + m_GameManager.GetScoreManager().GetMoney();
            m_NumberTip.text = "Min = " + amountOfDice + Environment.NewLine + " Max = " + maxAmountRoll;
            m_RandomNumber.text = "Number" + Environment.NewLine + m_GameManager.GetCurrentNumber();
            m_LastNumber.text = "Previous" + Environment.NewLine + m_GameManager.GetCurrentPreviousNumber();
            m_PayAmountForRoll.text = m_GameManager.GetGlobalGameValue().m_RollPrice * m_GameManager.GetCurrentRound() + " to roll";
        }

        public void RefreshRolledNumber(int rolledNumber)
        {
            m_RolledNumber.text = "Rolled" + Environment.NewLine + rolledNumber;
        }

        public void RefreshDiceOnNextRoll(int activeDice)
        {
            m_DiceOnNextRoll.text = "Dice on next roll" + Environment.NewLine + activeDice;
        }

        public void DisableRollDiceButton()
        {
            m_RollDiceButton.SetActive(false);
        }

        public void EnableRollButton()
        {
            m_RollDiceButton.SetActive(true);
        }

        //Activates floating number above the dice
        public void ShowDiceTotal(Vector2 startPos, int value)
        {
            RefreshRolledNumber(value);
            m_ShowDiceTotal.gameObject.SetActive(true);
            m_ShowDiceTotal.Activate(startPos, value);
        }

        private void ResetButtonColors()
        {
            for (int i = 0; i < m_PredictionButtons.Count; i++)
            {
                m_PredictionButtons[i].color = Color.white;
            }
        }

        //Enables roll button and changes color of the pressed button
        public void SelectCurrentPrediction(Prediction prediction)
        {
            EnableRollButton();
            for (int i = 0; i < m_PredictionButtons.Count; i++)
            {
                m_PredictionButtons[i].color = Color.white;
            }
            m_PredictionButtons[(int)prediction].color = Color.gray;
        }
    }
}
