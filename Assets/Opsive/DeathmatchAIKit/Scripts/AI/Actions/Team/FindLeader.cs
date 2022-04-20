/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Finds the leader of the team that the agent is on.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class FindLeader : Action
    {
        [Tooltip("The found leader")]
        [SharedRequired] [SerializeField] protected SharedGameObject m_Leader;

        /// <summary>
        /// Communicates with the TeamManager to determine the leader.
        /// </summary>
        /// <returns>Success if a leader was found.</returns>
        public override TaskStatus OnUpdate()
        {
            // Don't change leaders if a leader already exists.
            if (m_Leader.Value != null) {
                return TaskStatus.Success;
            }

            m_Leader.Value = TeamManager.GetLeader(gameObject);
            
            if (m_Leader.Value != null) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}