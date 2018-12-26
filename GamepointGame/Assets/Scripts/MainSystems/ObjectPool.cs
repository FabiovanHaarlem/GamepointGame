using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class ObjectPool : MonoBehaviour
    {
        private List<Dice> m_Dice;

        public void PrepareScriptVariables()
        {
            m_Dice = new List<Dice>();
        }

        public void GetScriptRefs(GameManager gameManager)
        {
            LoadDice();
        }

        //Instantiate X amount of dice to be reused
        private void LoadDice()
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject dice = Instantiate(Resources.Load("Prefabs/Dice")) as GameObject;
                m_Dice.Add(dice.GetComponent<Dice>());
                dice.name = (m_Dice.Count).ToString();
            }
        }

        public List<Dice> GetDice(int amount)
        {
            List<Dice> dice = new List<Dice>();

            for (int i = 0; i < amount; i++)
            {
                dice.Add(m_Dice[i]); //Loads the amount of dice needed into empty array
            }
            
            //If the maximum amount of dice are reached then 1 extra dice will be created
            if (dice.Count == m_Dice.Count)
            {
                GameObject diceObject = Instantiate(Resources.Load("Prefabs/Dice")) as GameObject;
                m_Dice.Add(diceObject.GetComponent<Dice>());
                diceObject.name = (m_Dice.Count).ToString();
            }
            return dice;
        }
    }
}
