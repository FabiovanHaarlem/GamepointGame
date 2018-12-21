using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class UIManager : MonoBehaviour
    {
        private GameManager m_GameManager;
        private Text m_RandomNumber;
        private Text m_NumberTip;
        private Text m_Currency;
        private Text m_PayAmountForRoll;
        private Text m_LastNumber;
        private ShowDiceTotal m_ShowDiceTotal;

        private GameObject m_RollDiceButton;

        [SerializeField]
        private List<Image> m_PredictionButtons;

        public void PrepareScriptVariables()
        {

        }

        public void GetScriptRefs(GameManager gameManager)
        {
            m_GameManager = gameManager;
            m_GameManager.GetEventManager.E_PrepareNewRoundEvent += EnableRollButton;
            m_GameManager.GetEventManager.E_StartNewRoundEvent += RefreshRandomNumber;
            m_GameManager.GetEventManager.E_RollDiceEvent += DisableRollDiceButton;
            m_RandomNumber = GameObject.Find("RandomNumber").GetComponent<Text>();
            m_NumberTip = GameObject.Find("NumberTip").GetComponent<Text>();
            m_Currency = GameObject.Find("Currency").GetComponent<Text>();
            m_PayAmountForRoll = GameObject.Find("ToPayForRoll").GetComponent<Text>();
            m_RollDiceButton = GameObject.Find("RollDiceButton");
            m_LastNumber = GameObject.Find("PreviousNumber").GetComponent<Text>();
            m_ShowDiceTotal = GameObject.Find("TextBorderShowDiceTotal").GetComponent<ShowDiceTotal>();
            m_ShowDiceTotal.gameObject.SetActive(false);
        }

        public void RefreshRandomNumber()
        {
            int amountOfDice = m_GameManager.GetCurrentRound() + 1;
            int maxAmountRoll = 6 * amountOfDice;
            m_RandomNumber.text = "Number" + m_GameManager.GetCurrentNumber();
            m_NumberTip.text = "Min = " + amountOfDice + " Max = " + maxAmountRoll;
            m_Currency.text = "Currency: " + m_GameManager.GetScoreManager.GetCurrency();
            m_PayAmountForRoll.text = 50 * m_GameManager.GetCurrentRound() + " to roll";
            m_LastNumber.text = "Previous" + m_GameManager.GetCurrentPreviousNumber();
            ResetButtonColors();
            DisableRollDiceButton();
        }

        private void DisableRollDiceButton()
        {
            m_RollDiceButton.SetActive(false);
        }

        public void EnableRollButton()
        {
            m_RollDiceButton.SetActive(true);
        }

        public void ShowDiceTotal(Vector2 startPos, int value)
        {
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
