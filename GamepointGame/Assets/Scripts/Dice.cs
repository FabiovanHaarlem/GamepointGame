using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> m_DiceSprites;
    private SpriteRenderer m_SpriteRenderer;
    private int m_Number;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        Roll();
        SetSprite(m_Number);
        GameManager.m_Instance.GetEventManager.E_RollDiceEvent += Roll;
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    private void Roll()
    {
        m_Number = GetRandomNumber();
        SetSprite(m_Number);
    }

    public int GetNumber()
    {
        return m_Number;
    }

    private int GetRandomNumber()
    {
        return Random.Range(1, 7);
    }

    private void SetSprite(int number)
    {
        m_SpriteRenderer.sprite = m_DiceSprites[number - 1];
    }
}
