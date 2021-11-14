using System;
using System.Collections.Generic;
using StatSystem.Nodes;
using UnityEngine;

namespace StatSystem
{
    public class StatController : MonoBehaviour
    {
        [SerializeField] private StatDatabase m_StatDatabase;
        protected Dictionary<string, Stat> m_Stats = new Dictionary<string, Stat>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Stat> stats => m_Stats;

        private bool m_IsInitialized;
        public bool isInitialized => m_IsInitialized;
        public event Action initialized;
        public event Action willUninitialize;

        protected virtual void Awake()
        {
            if (!m_IsInitialized)
            {
                Initialize();
            }
        }

        private void OnDestroy()
        {
            willUninitialize?.Invoke();
        }

        private void Initialize()
        {
            foreach (StatDefinition definition in m_StatDatabase.stats)
            {
                m_Stats.Add(definition.name, new Stat(definition));
            }

            foreach (StatDefinition definition in m_StatDatabase.attributes)
            {
                m_Stats.Add(definition.name, new Attribute(definition));
            }

            foreach (StatDefinition definition in m_StatDatabase.primaryStats)
            {
                m_Stats.Add(definition.name, new PrimaryStat(definition));
            }
            
            InitializeStatFormulas();

            foreach (Stat stat in m_Stats.Values)
            {
                stat.Initialize();
            }
            
            m_IsInitialized = true;
            initialized?.Invoke();
        }

        protected virtual void InitializeStatFormulas()
        {
            foreach (Stat currentStat in m_Stats.Values)
            {
                if (currentStat.definition.formula != null && currentStat.definition.formula.rootNode != null)
                {
                    List<StatNode> statNodes = currentStat.definition.formula.FindNodesOfType<StatNode>();

                    foreach (StatNode statNode in statNodes)
                    {
                        if (m_Stats.TryGetValue(statNode.statName.Trim(), out Stat stat))
                        {
                            statNode.stat = stat;
                            stat.valueChanged += currentStat.CalculateValue;
                        }
                        else
                        {
                            Debug.LogWarning($"Stat {statNode.statName.Trim()} does not exist!");
                        }
                    }
                }
            }
        }
    }
}