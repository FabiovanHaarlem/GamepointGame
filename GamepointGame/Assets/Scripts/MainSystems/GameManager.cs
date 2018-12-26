using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainGame
{
    [RequireComponent(typeof(EventManager))]
    [RequireComponent(typeof(ObjectPool))]
    [RequireComponent(typeof(GameStateManager))]
    [RequireComponent(typeof(ScoreManager))]
    [RequireComponent(typeof(UIManager))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GlobalGameValues m_GlobalGameValues;
        [SerializeField] private Transform m_DiceStartPosition;

        public static GameManager m_Instance;
        private EventManager m_EventManager;
        private ObjectPool m_ObjectPool;
        private GameStateManager m_GameStateManager;
        private ScoreManager m_ScoreManager;
        private UIManager m_UIManager;
        private List<Dice> m_Dice;

        private Prediction m_Prediction;

        private int m_MainNumber;
        private int m_LastNumber;
        private int m_Round;

        #region PrivateVariablesGetMethods
        public GlobalGameValues GetGlobalGameValue()
        { return m_GlobalGameValues; }

        public EventManager GetEventManager()
        { return m_EventManager; }

        public ObjectPool GetObjectPool()
        { return m_ObjectPool; }

        public ScoreManager GetScoreManager()
        { return m_ScoreManager; }

        public UIManager GetUIManager()
        { return m_UIManager; }

        public Prediction GetCurrentPrediction()
        { return m_Prediction; }

        public List<Dice> GetActiveDice()
        { return m_Dice; }

        public int GetCurrentNumber()
        { return m_MainNumber; }

        public int GetCurrentPreviousNumber()
        { return m_LastNumber; }

        public int GetCurrentRound()
        { return m_Round; }
        #endregion

        private void Awake()
        {
            m_Dice = new List<Dice>();
            m_Instance = this;
            m_Round = 1;
            m_LastNumber = 0;

            m_EventManager = GetComponent<EventManager>();

            m_ObjectPool = GetComponent<ObjectPool>();
            m_ObjectPool.PrepareScriptVariables();

            m_GameStateManager = GetComponent<GameStateManager>();

            m_ScoreManager = GetComponent<ScoreManager>();
            m_ScoreManager.PrepareScriptVariables();

            m_UIManager = GetComponent<UIManager>();
        }

        private void Start()
        {
            ChangeGameState(GameState.Preparing);
            m_ScoreManager.GetScriptRefs(this);
            m_UIManager.GetScriptRefs(this);
            m_ObjectPool.GetScriptRefs(this);
            LoadNewDice();
            PrepareRound();
        }

        public void ChangeGameState(GameState newGameState)
        {
            m_GameStateManager.ChangeGameState(newGameState);
        }

        public bool CheckGameState(GameState checkedGameState)
        {
            if (m_GameStateManager.GetGameState() == checkedGameState)
                return true;
            else
                return false;
        }

        private void LoadNewDice()
        {
            m_Dice = new List<Dice>(m_ObjectPool.GetDice(m_Round + 1));
        }

        private void PrepareRound()
        {
            m_MainNumber = GenerateMainNumber(m_Round + 1);
            m_EventManager.RefreshUI();
            m_UIManager.RefreshDiceOnNextRoll(m_Dice.Count);
            ChangeGameState(GameState.ReadyToRoll);
        }
        
        public int GenerateMainNumber(int amountOfDice)
        {
            int maxValue = (6 * amountOfDice) + 1;// max value if all dice roll 6
            return Random.Range(amountOfDice, maxValue); ;
        }

        //Sets the prediction of the player
        public void SetPrediction(int prediction)
        {
            if (CheckGameState(GameState.ReadyToRoll))
            {
                m_Prediction = (Prediction)prediction;
                m_UIManager.SelectCurrentPrediction(m_Prediction);
            }
        }

        //Rolls all active dice
        public void RollDice()
        {
            if (CheckGameState(GameState.ReadyToRoll))
            {
                if (m_ScoreManager.PayForRoll(m_Round)) //Checks if player has enough money
                {
                    ChangeGameState(GameState.Rollin);
                    ResetDice();
                    RefreshUI();
                    m_LastNumber = m_MainNumber;
                    for (int i = 0; i < m_Dice.Count; i++)
                    {
                        m_Dice[i].Roll(m_DiceStartPosition.position);
                    }
                    GetUIManager().DisableRollDiceButton();
                    CheckDiceValue();
                }
                else //If player does not have enough money the rounds will be reset
                {
                    m_Round = 1;
                }
            }
        }

        public void CheckDiceValue()
        {
            m_ScoreManager.StartCheckingDice();
        }

        private void RefreshUI()
        {
            m_EventManager.RefreshUI();
        }

        /*Disables all dice currently active and unloads them form the list
        Then it reloads the correct amount of dice needed for the next round*/
        private void ResetDice()
        {
            for (int i = 0; i < m_Dice.Count; i++)
            {
                m_Dice[i].Disable();
            }
            LoadNewDice();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToMainMenu();
            }
        }

        public void Won()
        {
            ChangeGameState(GameState.Rolled);
            m_Round++;
            RefreshUI();
            PrepareRound();
        }

        public void Lost()
        {
            if (m_ScoreManager.GetMoney() <= 0f)
            {
                ToMainMenu();
            }

            ChangeGameState(GameState.Rolled);
            m_Round = 1;
            RefreshUI();
            PrepareRound();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public enum Prediction
    {
        Lower = 0,
        Equal,
        Higher
    }
}
