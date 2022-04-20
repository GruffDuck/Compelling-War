/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Conditions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Traits;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Should the agent flee?")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class ShouldFlee : Conditional
    {
        [Tooltip("The character that caused damage to the agent.")]
        [SerializeField] protected SharedGameObject m_Attacker;
        [Tooltip("The agent should flee if the health is less than the specified value.")]
        [SerializeField] protected SharedFloat m_HealthAmount;
        [Tooltip("The agent should flee if the distance is greater than the specified value.")]
        [SerializeField] protected SharedFloat m_MinDistance;

        private Health m_Health;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            m_Health = gameObject.GetCachedComponent<Health>();
        }

        /// <summary>
        /// Returns Success if the agent's health is low and the agent is far away from the attacker.
        /// </summary>
        /// <returns>Success if the agent's health is low and the agent is far away from the attacker.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_Attacker.Value == null) {
                return TaskStatus.Failure;
            }

            // The agent should not flee if they have adequit health or is far away from the target.
            if (m_Health.Value > m_HealthAmount.Value || (transform.position - m_Attacker.Value.transform.position).magnitude > m_MinDistance.Value) {
                return TaskStatus.Failure;
            }
            return TaskStatus.Success;
        }

        /// <summary>
        /// Reset the SharedVariable values.
        /// </summary>
        public override void OnReset()
        {
            m_HealthAmount = 0;
            m_MinDistance = 0;
        }
    }
}