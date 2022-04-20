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
    [TaskDescription("Evades the specified grenade.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class EvadeGrenade : SpeedChangeMovement
    {
        [Tooltip("The agent has evaded the grenade when they have moved the specified distance away from the grenade.")]
        [SerializeField] protected SharedFloat m_EvadeDistance = 10;
        [Tooltip("A reference to the grenade(s) to evade.")]
        [SerializeField] protected SharedGameObjectList m_Grenades;

        /// <summary>
        /// Move in the opposite direction of the grenade.
        /// </summary>
        /// <returns>Success when the agent has evaded the grenade.</returns>
        public override TaskStatus OnUpdate()
        {
            // Multiple grenades can be specified. If multiple grenades are specified then evade from the center point of all of the grenades.
            var centerPoint = Vector3.zero;
            for (int i = 0; i < m_Grenades.Value.Count; ++i) {
                centerPoint += m_Grenades.Value[i].transform.position;
            }
            centerPoint /= m_Grenades.Value.Count;
            
            var direction = transform.InverseTransformDirection(transform.position - centerPoint);
            direction.y = 0;
            if (direction.magnitude > m_EvadeDistance.Value) {
                return TaskStatus.Success;
            }

            // Head in the opposite direction of the center point.
            SetDestination(transform.position + transform.TransformDirection(direction.normalized) * m_EvadeDistance.Value);

            return TaskStatus.Running;
        }

        /// <summary>
        /// The task has ended. Clear the grenade list.
        /// </summary>
        public override void OnEnd()
        {
            base.OnEnd();

            m_Grenades.Value.Clear();
        }
    }
}