using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class Dice : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> m_DiceSprites;
        private SpriteRenderer m_SpriteRenderer;
        private Rigidbody2D m_Rigidbody2D;
        private int m_Number;

        private void Start()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Number = GetRandomNumber(1, 7);
            SetSprite(m_Number);
            //GameManager.m_Instance.GetEventManager.E_RollDiceEvent += Roll;
            gameObject.SetActive(false);
        }

        public void SetPosition(Vector2 pos)
        {
            transform.position = pos;
        }

        public void Roll()
        {
            gameObject.SetActive(true);
            ThrowDice();
            m_Number = GetRandomNumber(1, 7);
            SetSprite(m_Number);
        }

        private void ThrowDice()
        {
            Vector2 throwToPosition = new Vector2(Random.Range(2f, 5f), Random.Range(-2f, 3f));
            Vector2 direction = (throwToPosition - new Vector2(transform.position.x, transform.position.y).normalized);
            m_Rigidbody2D.AddForce(direction * GetRandomNumber(3, 8), ForceMode2D.Impulse);
        }

        public int GetNumber()
        {
            return m_Number;
        }

        private int GetRandomNumber(int min, int max)
        {
            return Random.Range(min, max);
        }

        private void SetSprite(int number)
        {
            m_SpriteRenderer.sprite = m_DiceSprites[number - 1];
        }
    }
}
