/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Traits;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Determines if the agent should switch targets after attacked.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class CheckForTargetSwitch : Action
    {
        [Tooltip("A set of targets that the agent should ignore. This set will be added to when the agent switches to a new target to prevent the agent from switching back to the old.")]
        [SerializeField] protected SharedGameObjectSet m_IgnoreTargets;
        [Tooltip("Switch targets if the target's health is less than this amount.")]
        [SerializeField] protected SharedFloat m_SwitchHealth;
        [Tooltip("Switch targets if a random value is less than the switch probability.")]
        [SerializeField] protected SharedFloat m_ForceSwitchProbability;
        [Tooltip("The target that the agent should attack.")]
        [SerializeField] protected SharedGameObject m_Target;
        [Tooltip("The target that the agent may attack.")]
        [SerializeField] protected SharedGameObject m_PossibleTarget;

        /// <summary>
        /// Determine if the target should be switched.
        /// </summary>
        /// <returns>Success if the target was switched.</returns>
        public override TaskStatus OnUpdate()
        {
            // Don't switch targets if there are no attackers.
            if (m_PossibleTarget.Value == null) {
                return TaskStatus.Failure;
            }

            var switchTargets = false;
            if (m_Target.Value == null || m_PossibleTarget.Value != m_Target.Value.GetCachedParentComponent<UltimateCharacterController.Character.UltimateCharacterLocomotion>().gameObject) {
                // Switch to the target attacking the agent if:
                // - The agent does not currently have any targets.
                // - The target's health value is lower than the switch health.
                // - A random probability less than the switch probability.
                if (m_Target.Value == null) {
                    switchTargets = true;
                } else if (!m_IgnoreTargets.Value.Contains(m_PossibleTarget.Value.transform.gameObject)) {
                    var currentTargetHealth = m_Target.Value.GetCachedParentComponent<Health>();
                    if (currentTargetHealth.HealthValue > m_SwitchHealth.Value || Random.value < m_ForceSwitchProbability.Value) {
                        switchTargets = true;
                    }
                }
            }
            return switchTargets ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}