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
    [TaskDescription("Is the target within distance?")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class IsTargetWithinDistance : Conditional
    {
        [Tooltip("The GameObject within distance")]
        [SerializeField] protected SharedGameObject m_Target;
        [Tooltip("The distance to compare against")]
        [SerializeField] protected SharedFloat m_Distance;

        /// <summary>
        /// Returns Success if the target is within distance.
        /// </summary>
        /// <returns>Success if the target is within distance.</returns>
        public override TaskStatus OnUpdate()
        {
            // If the target is null then it's not within distance.
            if (m_Target.Value == null) {
                return TaskStatus.Failure;
            }

            var direction = m_Target.Value.transform.position - transform.position;
            direction.y = 0;
            return direction.magnitude < m_Distance.Value ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}