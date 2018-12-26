using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class Dice : MonoBehaviour
    {
        [SerializeField] private List<Sprite> m_DiceSprites;
        private SpriteRenderer m_SpriteRenderer;
        private Rigidbody2D m_Rigidbody2D;

        private int m_Number;

        private void Start()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            Disable();
        }

        public int GetNumber()
        {
            return m_Number;
        }

        //Sets sprite to the spite of the rolled number
        private void SetSprite()
        {
            m_SpriteRenderer.sprite = m_DiceSprites[m_Number - 1];
        }

        public void Roll(Vector2 startPosition)
        {
            transform.position = startPosition;
            gameObject.SetActive(true);
            ThrowDice();
            m_Number = Random.Range(1, 7);
            SetSprite();
        }

        //Calculates the direction and adds a force in that direction
        private void ThrowDice()
        {
            Vector2 throwToPosition = new Vector2(Random.Range(1f, 5f), Random.Range(-2f, 3f)); //Position to throw to
            Vector2 direction = (throwToPosition - new Vector2(transform.position.x, transform.position.y).normalized); //Calculates the direction
            m_Rigidbody2D.AddForce(direction * Random.Range(3, 8), ForceMode2D.Impulse); //Adds force
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
