/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Conditions
{
    using UnityEngine;
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using Opsive.UltimateCharacterController.Objects;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
    using Opsive.Shared.Game;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Is the agent near a grenade?")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class IsNearGrenade : Conditional
    {
        [Tooltip("The agent may not react to the grenade if a random probability is greater than the react likelihood.")]
        [SerializeField] protected SharedFloat m_ReactLikelihood = 1;
        [Tooltip("A reference to the grenades to react to.")]
        [SerializeField] protected SharedGameObjectList m_Grenades;

        private bool m_CanReact;

        /// <summary>
        /// Prepare for another task execution.
        /// </summary>
        public override void OnStart()
        {
            m_CanReact = true;
        }

        /// <summary>
        /// Returns Success if the agent should react to the nearby grenade.
        /// </summary>
        /// <returns>Success if the agent should react to the nearby grenade.</returns>
        public override TaskStatus OnUpdate()
        {
            // Allow the AI to make a mistake by not reacting to the grenade. If the agent should not react to the grenade then remember the
            // result until the next time the task starts. This will prevent the agent from quickly switching between a reaction or not every tick.
            if (m_CanReact && Random.value > m_ReactLikelihood.Value) {
                m_CanReact = false;
            }

            if (!m_CanReact) {
                return TaskStatus.Failure;
            }

            // Remove the grenade reference as soon as it explodes.
            for (int i = m_Grenades.Value.Count - 1; i > -1; --i) {
                if (!m_Grenades.Value[i].activeSelf) {
                    m_Grenades.Value.RemoveAt(i);
                }
            }

            // The agent should react if there are any grenades within the list.
            return m_Grenades.Value.Count > 0 ? TaskStatus.Success : TaskStatus.Failure;
        }

        /// <summary>
        /// The grenade will have a trigger that is significantly larger than the size of the grenade. As soon as the agent enters that trigger they will be notified
        /// that they are near a grenade.
        /// </summary>
        /// <param name="other">A possible grenade.</param>
        public override void OnTriggerEnter(Collider other)
        {
            // Only be afraid of a grenade thrown by somebody else.
            var grenade = other.gameObject.GetCachedComponent<Grenade>();
            if (grenade != null) {
                if (grenade.Originator != gameObject && !m_Grenades.Value.Contains(other.gameObject)) {
                    m_Grenades.Value.Add(other.gameObject);
                }
            }
        }

        /// <summary>
        /// Remove the grenade once it can no longer harm the character.
        /// </summary>
        /// <param name="other">A possible grenade.</param>
        public override void OnTriggerExit(Collider other)
        {
            // Only be afraid of a grenade thrown by somebody else.
            var grenade = other.gameObject.GetCachedComponent<Grenade>();
            if (grenade != null) {
                if (grenade.Originator != gameObject && m_Grenades.Value.Contains(other.gameObject)) {
                    m_Grenades.Value.Remove(other.gameObject);
                }
            }
        }
    }
}