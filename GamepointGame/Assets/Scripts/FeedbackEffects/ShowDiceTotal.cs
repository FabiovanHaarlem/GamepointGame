using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class ShowDiceTotal : MonoBehaviour
    {
        private RectTransform m_RectTransform;
        private Text m_Text;

        private float m_ActiveTimer;

        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
            m_Text = transform.GetChild(0).GetComponent<Text>();
        }

        public void Activate(Vector2 startPosition, int value)
        {
            //Converts the dice world position to sceen position
            m_RectTransform.position = Camera.main.WorldToScreenPoint(new Vector2(startPosition.x, startPosition.y + 0.5f));
            gameObject.SetActive(true);
            m_Text.text = value.ToString();
            m_ActiveTimer = 1.2f;
        }

        //Moves the object upwards
        private void Update()
        {
            Vector2 currentPos = m_RectTransform.position;
            m_RectTransform.position = new Vector3(currentPos.x, currentPos.y + 50f * Time.deltaTime);

            m_ActiveTimer -= Time.deltaTime;

            if (m_ActiveTimer <= 0f)
            {
                Disable();
            }
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
