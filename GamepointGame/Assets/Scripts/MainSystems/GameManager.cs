using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventManager))]
[RequireComponent(typeof(ScoreManager))]
[RequireComponent(typeof(UIManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance;

    public EventManager GetEventManager { get; private set; }
    public ScoreManager GetScoreManager { get; private set; }
    public UIManager GetUIManager { get; private set; }

    private List<Dice> m_Dice;

    private Prediction m_Prediction;
    private int m_RandomNumber;
    private int m_Round;

    private void Awake()
    {
        m_Instance = this;
        m_Round = 1;
        GetEventManager = GetComponent<EventManager>();

        GetScoreManager = GetComponent<ScoreManager>();
        GetScoreManager.PrepareScriptVariables();

        GetUIManager = GetComponent<UIManager>();
        GetUIManager.PrepareScriptVariables();

        m_Dice = new List<Dice>();
    }

    private void Start()
    {
        GetEventManager.E_PrepareNewRoundEvent += PrepareRandomNumber;
        GetScoreManager.GetScriptRefs(this);
        GetUIManager.GetScriptRefs(this);
        for (int i = 0; i < 2; i++)
        {
            CreateDice();
        }
        PrepareRound();
    }

    private void CreateDice()
    {
        GameObject dice = Instantiate(Resources.Load("Prefabs/Dice")) as GameObject;
        m_Dice.Add(dice.GetComponent<Dice>());
    }

    private void PrepareRound()
    {
        GetEventManager.PrepareNewRound();
        //Plaats de dice op een rij en draai random getallen.
        //Tel daarna de getallen bij elkaar op.
        int height = -2;
        for (int i = 0; i < m_Dice.Count; i++)
        {
            m_Dice[i].SetPosition(new Vector2(((-0.5f * (m_Dice.Count / 2))) + (1 * (i + 1)), height));
        }
        StartRound();
    }

    private void StartRound()
    {
        GetEventManager.StartNewRound();
    }

    public int GetCurrentNumber()
    {
        return m_RandomNumber;
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
        m_RandomNumber = GetRandomNumber(m_Dice.Count);
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
            GetEventManager.RollDice();
            CheckDiceValue();
        }
        else
        {
            m_Round = 1;
            for (int i = m_Dice.Count - 1; i > 1; i--)
            {
                GameObject toDestroyObject = m_Dice[i].gameObject;
                Destroy(toDestroyObject);
                m_Dice.RemoveAt(i);
            }
            //m_Dice.RemoveRange(m_Dice.Count - 2, );
        }
    }

    public void CheckDiceValue()
    {
        GetEventManager.CheckDice();
    }

    public void Won()
    {
        m_Round++;
        GetUIManager.RefreshRandomNumber();
        CreateDice();
        PrepareRound();
    }

    public void Lost()
    {
        m_Round = 1;
        for (int i = m_Dice.Count - 1; i > 1; i--)
        {
            GameObject toDestroyObject = m_Dice[i].gameObject;
            Destroy(toDestroyObject);
            m_Dice.RemoveAt(i);
        }
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
