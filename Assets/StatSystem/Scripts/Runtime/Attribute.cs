using System;
using UnityEngine;

namespace StatSystem
{
    public class Attribute : Stat
    {
        protected int m_CurrentValue;
        public int currentValue => m_CurrentValue;
        public event Action currentValueChanged;
        public event Action<StatModifier> appliedModifier;
        
        public Attribute(StatDefinition definition) : base(definition)
        {
        }
        
        public override void Initialize()
        {
            base.Initialize();
            m_CurrentValue = value;
        }

        public virtual void ApplyModifier(StatModifier modifier)
        {
            int newValue = m_CurrentValue;
            switch (modifier.type)
            {
                case ModifierOperationType.Override:
                    newValue = modifier.magnitude;
                    break;
                case ModifierOperationType.Additive:
                    newValue += modifier.magnitude;
                    break;
                case ModifierOperationType.Multiplicative:
                    newValue *= modifier.magnitude;
                    break;
            }

            newValue = Mathf.Clamp(newValue, 0, m_Value);

            if (currentValue != newValue)
            {
                m_CurrentValue = newValue;
                currentValueChanged?.Invoke();
                appliedModifier?.Invoke(modifier);
            }
        }
    }
}