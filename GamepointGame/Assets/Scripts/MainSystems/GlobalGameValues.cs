using UnityEngine;
namespace MainGame
{
    [CreateAssetMenu(fileName = "Game Value Container", menuName = "Game Value Containers")]
    public class GlobalGameValues : ScriptableObject
    {
        public int m_StartingCurrency;
        public int m_RollPrice;
        public int m_EqualWinFlatAmount;
        public int m_LowerWinFlatAmount;
        public int m_HigherWinFlatAmount;
        public float m_RoundMultiplierBonus;
    }
}
