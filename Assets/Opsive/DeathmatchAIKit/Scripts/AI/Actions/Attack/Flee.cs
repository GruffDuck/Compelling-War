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
    [TaskDescription("Flees in the opposite direction of the attacker.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class Flee : SpeedChangeMovement
    {
        [Tooltip("The character that hte agent should flee from.")]
        [SerializeField] protected SharedGameObject m_Attacker;
        [Tooltip("The agent has fleed when they are greater than the specified flee distance away from the attacker.")]
        [SerializeField] protected SharedFloat m_FleeDistance = 10;

        /// <summary>
        /// Flees in the opposite direction of the attacker.
        /// </summary>
        /// <returns>Success after the agent has fled.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_Attacker.Value == null) {
                return TaskStatus.Failure;
            }

            var direction = transform.InverseTransformDirection(transform.position - m_Attacker.Value.transform.position);
            direction.y = 0;
            if (direction.magnitude > m_FleeDistance.Value) {
                return TaskStatus.Success;
            }

            // The attacker is still close. Keep fleeing.
            SetDestination(transform.position + transform.TransformDirection(direction.normalized) * m_FleeDistance.Value);

            return TaskStatus.Running;
        }
    }
}