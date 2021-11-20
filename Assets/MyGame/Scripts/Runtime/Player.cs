using LevelSystem;
using StatSystem;
using UnityEngine;
using UnityEngine.AI;

namespace MyGame.Scripts
{
    [RequireComponent(typeof(PlayerStatController))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Player : MonoBehaviour
    {
        private PlayerStatController m_PlayerStatController;
        private ILevelable m_Levelable;
        [SerializeField] private Transform m_Target;
        private NavMeshAgent m_NavMeshAgent;

        private void Awake()
        {
            m_PlayerStatController = GetComponent<PlayerStatController>();
            m_Levelable = m_PlayerStatController.GetComponent<ILevelable>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            if (m_Target != null)
                m_NavMeshAgent.SetDestination(m_Target.position);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                (m_PlayerStatController.stats["Health"] as Attribute).ApplyModifier(new StatModifier
                {
                    magnitude = -10,
                    type = ModifierOperationType.Additive
                });
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                (m_PlayerStatController.stats["Mana"] as Attribute).ApplyModifier(new StatModifier
                {
                    magnitude = -10,
                    type = ModifierOperationType.Additive
                });
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_Levelable.currentExperience += 50;
            }
        }
    }
}