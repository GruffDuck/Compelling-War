/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Conditions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Returns Success if the current agent is the team leader.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class ShouldLeadSearch : Conditional
    {
        [Tooltip("The existing leader")]
        [SerializeField] protected SharedGameObject m_Leader;
        [Tooltip("The probability that the agent will search without checking for a leader")]
        [SerializeField] protected SharedFloat m_ForceSearchProbability = 0.5f;

        /// <summary>
        /// Return Success if the current agent is the team leader.
        /// </summary>
        /// <returns>Success if the current agent is the team leader.</returns>
        public override TaskStatus OnUpdate()
        {
            // Don't lead if there is already a leader.
            if (m_Leader.Value != null) {
                return TaskStatus.Failure;
            }

            if (Random.value < m_ForceSearchProbability.Value) {
                return TaskStatus.Success;
            }

            // If GetLeader returns null then there is no leader and the current agent can become the leader.
            if (TeamManager.GetLeader(gameObject) == null) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}