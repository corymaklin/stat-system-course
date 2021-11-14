using System;
using System.Collections.Generic;
using Core;
using LevelSystem.Nodes;
using UnityEngine;

namespace LevelSystem
{
    public class LevelController : MonoBehaviour, ILevelable
    {
        [SerializeField] private int m_Level = 1;
        [SerializeField] private int m_CurrentExperience;
        [SerializeField] private NodeGraph m_RequiredExperienceFormula;

        private bool m_IsInitialized;

        public int level => m_Level;
        public event Action levelChanged;
        public event Action currentExperienceChanged;

        public int currentExperience
        {
            get => m_CurrentExperience;
            set
            {
                if (value >= requiredExperience)
                {
                    m_CurrentExperience = value - requiredExperience;
                    currentExperienceChanged?.Invoke();
                    m_Level++;
                    levelChanged?.Invoke();
                }
                else if (value < requiredExperience)
                {
                    m_CurrentExperience = value;
                    currentExperienceChanged?.Invoke();
                }
            }
        }

        public int requiredExperience => Mathf.RoundToInt(m_RequiredExperienceFormula.rootNode.value);
        public bool isInitialized => m_IsInitialized;
        public event Action initialized;
        public event Action willUninitialize;

        private void Awake()
        {
            if (!m_IsInitialized)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            List<LevelNode> levelNodes = m_RequiredExperienceFormula.FindNodesOfType<LevelNode>();
            foreach (LevelNode levelNode in levelNodes)
            {
                levelNode.levelable = this;
            }

            m_IsInitialized = true;
            initialized?.Invoke();
        }

        private void OnDestroy()
        {
            willUninitialize?.Invoke();
        }
    }
}