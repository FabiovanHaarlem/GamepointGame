using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    [RequireComponent(typeof(EventManager))]
    [RequireComponent(typeof(ScoreManager))]
    [RequireComponent(typeof(UIManager))]
    public class GameManager : MonoBehaviour
    {
        public static GameManager m_Instance;

        public EventManager GetEventManager { get; private set; }
        public ScoreManager GetScoreManager { get; private set; }
        public UIManager GetUIManager { get; private set; }
        public ObjectPool GetObjectPool { get; private set; }

        private List<Dice> m_Dice;
        [SerializeField]
        private Transform m_DiceStartPosition;

        private Prediction m_Prediction;
        private int m_MainNumber;
        private int m_LastNumber;
        private int m_Round;
        private bool m_LostPreviousRound;

        private void Awake()
        {
            m_Instance = this;
            m_LostPreviousRound = false;
            m_Round = 1;
            m_LastNumber = 0;
            GetEventManager = GetComponent<EventManager>();

            GetScoreManager = GetComponent<ScoreManager>();
            GetScoreManager.PrepareScriptVariables();

            GetUIManager = GetComponent<UIManager>();
            GetUIManager.PrepareScriptVariables();

            GetObjectPool = GetComponent<ObjectPool>();
            GetObjectPool.PrepareScriptVariables();

            m_Dice = new List<Dice>();
        }

        private void Start()
        {
            GetEventManager.E_PrepareNewRoundEvent += PrepareRandomNumber;
            GetScoreManager.GetScriptRefs(this);
            GetUIManager.GetScriptRefs(this);
            GetObjectPool.GetScriptRefs(this);
            //for (int i = 0; i < 2; i++)
            //{
            //    CreateDice();
            //}
            GetDice(m_Round + 1);
            PrepareRound();
        }

        private void GetDice(int amount)
        {
            m_Dice = new List<Dice>(GetObjectPool.GetDice(amount));
        }

        //private void CreateDice()
        //{
        //    GameObject dice = Instantiate(Resources.Load("Prefabs/Dice")) as GameObject;
        //    m_Dice.Add(dice.GetComponent<Dice>());
        //}

        private void PrepareRound()
        {
            GetEventManager.PrepareNewRound();
            StartRound();
        }

        private void StartRound()
        {
            GetEventManager.StartNewRound();
        }

        public int GetCurrentNumber()
        {
            return m_MainNumber;
        }

        public int GetCurrentPreviousNumber()
        {
            return m_LastNumber;
        }

        public Prediction GetCurrentPrediction()
        {
            return m_Prediction;
        }

        public List<Dice> GetAllDice()
        {
            return m_Dice;
        }

        public int GetCurrentRound()
        {
            return m_Round;
        }

        public void PrepareRandomNumber()
        {
            //Switch to amount of dice
            m_MainNumber = GetRandomNumber(m_Dice.Count);
        }

        public int GetRandomNumber(int amountOfDice)
        {
            int maxValue = (6 * amountOfDice) + 1;
            return Random.Range(amountOfDice, maxValue); ;
        }

        public void SetPrediction(int prediction)
        {
            m_Prediction = (Prediction)prediction;
            GetUIManager.SelectCurrentPrediction(m_Prediction);
        }

        public void RollDice()
        {
            if (GetScoreManager.PayForRoll(m_Round))
            {
                ResetDice();
                m_LastNumber = m_MainNumber;
                for (int i = 0; i < m_Dice.Count; i++)
                {
                    m_Dice[i].SetPosition(m_DiceStartPosition.position);
                    m_Dice[i].Roll();
                }
                //GetEventManager.RollDice();
                CheckDiceValue();
            }
            else
            {
                m_Round = 1;
            }
        }

        public void CheckDiceValue()
        {
            GetEventManager.CheckDice();
        }

        private void ResetDice()
        {
            for (int i = 0; i < m_Dice.Count; i++)
            {
                m_Dice[i].gameObject.SetActive(false);
            }

            m_Dice = new List<Dice>(GetObjectPool.GetDice(m_Round + 1));
        }

        public void Won()
        {
            m_Round++;
            GetUIManager.RefreshRandomNumber();
            PrepareRound();
        }

        public void Lost()
        {
            m_Round = 1;
            GetUIManager.RefreshRandomNumber();
            PrepareRound();
        }
    }

    public enum Prediction
    {
        Lower = 0,
        Equal,
        Higher
    }
}
