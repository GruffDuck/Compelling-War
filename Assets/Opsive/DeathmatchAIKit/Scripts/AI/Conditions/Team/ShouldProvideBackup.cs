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
    [TaskDescription("Returns Success if the agent should provide backup.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class ShouldProvideBackup : Conditional
    {
        [Tooltip("The agent requesting backup")]
        [SerializeField] protected SharedGameObject m_Requstor;
        [Tooltip("The leader of the formation that the agent is in (can be null)")]
        [SerializeField] protected SharedGameObject m_Leader;
        [Tooltip("The maximim distance that the requestor can be away from the agent")]
        [SerializeField] protected SharedFloat m_MaxDistance;

        /// <summary>
        /// Return Success if the agent should provide backup.
        /// </summary>
        /// <returns>Success if the current agent is the team leader.</returns>
        public override TaskStatus OnUpdate()
        {
            // Always provide backup if the agent requesting backup is the leader.
            if (m_Requstor.Value.Equals(m_Leader.Value)) {
                return TaskStatus.Success;
            }

            // Don't provide backup if too far away.
            if ((m_Requstor.Value.transform.position - transform.position).magnitude > m_MaxDistance.Value) {
                return TaskStatus.Failure;
            }

            // Provide backup.
            return TaskStatus.Success;
        }
    }
}