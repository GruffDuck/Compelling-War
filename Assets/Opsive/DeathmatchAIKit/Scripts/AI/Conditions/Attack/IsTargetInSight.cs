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
    [TaskDescription("Is the target in sight?")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class IsTargetInSight : Conditional
    {
        [Tooltip("The maximum field of view that the agent can see other targets (in degrees).")]
        [SerializeField] protected SharedFloat m_FieldOfView = 90;
        [Tooltip("The maximum distance that the agent can see other targets.")]
        [SerializeField] protected SharedFloat m_MaxTargetDistance;
        [Tooltip("The GameObject within sight.")]
        [SharedRequired] [SerializeField] protected SharedGameObject m_FoundTarget;
        [Tooltip("A set of targets that the agent should ignore.")]
        [SharedRequired] [SerializeField] protected SharedGameObjectSet m_IgnoreTargets;

        private DeathmatchAgent m_DeathmatchAgent;

        /// <summary>
        ///Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            m_DeathmatchAgent = gameObject.GetCachedComponent<DeathmatchAgent>();
        }

        /// <summary>
        /// Returns success if the target is in sight and is alive.
        /// </summary>
        /// <returns>Success if the target is within sight and is alive.</returns>
        public override TaskStatus OnUpdate()
        {
            m_FoundTarget.Value = null;
            var target = m_DeathmatchAgent.TargetInSight(m_FieldOfView.Value, m_MaxTargetDistance.Value, m_IgnoreTargets.Value);
            if (target != null) {
                var targetHealth = target.GetCachedComponent<Health>();
                if (targetHealth != null && targetHealth.Value > 0) {
                    m_FoundTarget.Value = m_DeathmatchAgent.GetTargetBoneTransform(target.transform).gameObject;
                }
            }
            return m_FoundTarget.Value != null ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
