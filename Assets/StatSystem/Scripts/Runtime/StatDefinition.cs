using UnityEngine;

namespace StatSystem
{
    [CreateAssetMenu(fileName = "StatDefinition", menuName = "StatSystem/StatDefinition", order = 0)]
    public class StatDefinition : ScriptableObject
    {
        [SerializeField] private int m_BaseValue;
        [SerializeField] private int m_Cap = -1;
        public int baseValue => m_BaseValue;
        public int cap => m_Cap;

    }
}