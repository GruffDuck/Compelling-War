/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using BehaviorDesigner.Runtime.Tasks;
    using Opsive.DeathmatchAIKit.Character.Abilities;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Character.Abilities;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Gets the cover position from a cover point.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class StartCover : Action
    {
        [Tooltip("The current cover point")]
        [SerializeField] protected SharedCoverPoint m_CoverPoint;

        private UltimateCharacterLocomotion m_CharacterLocomotion;
        private Cover m_Cover;
        private MoveTowards m_MoveTowards;

        private bool m_Start;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            base.OnAwake();

            m_CharacterLocomotion = gameObject.GetCachedComponent<UltimateCharacterLocomotion>();
            m_Cover = m_CharacterLocomotion.GetAbility<Cover>();
            m_MoveTowards = m_CharacterLocomotion.GetAbility<MoveTowards>();
        }

        /// <summary>
        /// Start the Cover ability.
        /// </summary>
        public override void OnStart()
        {
            m_Cover.PredeterminedMoveTowardsLocation = m_CoverPoint.Value.MoveTowardsLocation;
            m_Start = m_Cover.IsActive || m_CharacterLocomotion.TryStartAbility(m_Cover);
        }

        /// <summary>
        /// Returns running until the cover ability has started.
        /// </summary>
        /// <returns>Running until the cover ability has started.</returns>
        public override TaskStatus OnUpdate()
        {
            if (!m_Start) {
                return TaskStatus.Failure;
            }

            // The agent may be moving into cover.
            if (m_MoveTowards != null && m_MoveTowards.IsActive && m_MoveTowards.OnArriveAbility == m_Cover) {
                return TaskStatus.Running;
            }

            return m_Cover.IsActive ? TaskStatus.Success : TaskStatus.Running;
        }

        /// <summary>
        /// The task has ended.
        /// </summary>
        public override void OnEnd()
        {
            base.OnEnd();

            // The task may have been aborted before cover has a chance to start.
            if (m_MoveTowards != null && m_MoveTowards.IsActive && m_MoveTowards.OnArriveAbility == m_Cover) {
                m_CharacterLocomotion.TryStopAbility(m_MoveTowards, true);
            }
        }
    }
}