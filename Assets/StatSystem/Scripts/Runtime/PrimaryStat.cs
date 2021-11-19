using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("StatSystem.Tests")]
namespace StatSystem
{
    public class PrimaryStat : Stat
    {
        private int m_BaseValue;
        public override int baseValue => m_BaseValue;
        
        public PrimaryStat(StatDefinition definition) : base(definition)
        {
        }
        
        public override void Initialize()
        {
            m_BaseValue = definition.baseValue;
            base.Initialize();
        }

        internal void Add(int amount)
        {
            m_BaseValue += amount;
            CalculateValue();
        }

        internal void Subtract(int amount)
        {
            m_BaseValue -= amount;
            CalculateValue();
        }
    }
}