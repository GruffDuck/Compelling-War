/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using UnityEngine;
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using Opsive.DeathmatchAIKit.Character.Abilities;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Character.Abilities.Items;
    using Opsive.Shared.Events;
    using Opsive.Shared.Game;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Manages movement while in cover.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class CoverMovement : Action
    {
        [Tooltip("The target GameObject")]
        [SerializeField] protected SharedGameObject m_Target;
        [Tooltip("The minimum amount of time that the agent can wait to aim while in cover")]
        [SerializeField] protected SharedFloat m_MinWaitDuration;
        [Tooltip("The maximum amount of time that the agent can wait to aim while in cover")]
        [SerializeField] protected SharedFloat m_MaxWaitDuration;
        [Tooltip("The minimum amount of time that the agent can aim while in cover")]
        [SerializeField] protected SharedFloat m_MinAimDuration;
        [Tooltip("The maximum amount of time that the agent can aim while in cover")]
        [SerializeField] protected SharedFloat m_MaxAimDuration;
        [Tooltip("The point to take cover at")]
        [SharedRequired] [SerializeField] protected SharedCoverPoint m_CoverPoint;
        [Tooltip("Can the agent attack?")]
        [SharedRequired] [SerializeField] protected SharedBool m_CanAttack;

        private UltimateCharacterLocomotion m_CharacterLocomotion;
        private DeathmatchAgent m_DeathmatchAgent;
        private Aim m_Aim;
        private Cover m_Cover;

        private float m_LastAimTime;
        private float m_LastWaitTime;
        private float m_Duration;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            m_CharacterLocomotion = gameObject.GetCachedComponent<UltimateCharacterLocomotion>();
            m_DeathmatchAgent = gameObject.GetCachedComponent<DeathmatchAgent>();
            m_Aim = m_CharacterLocomotion.GetAbility<Aim>();
            m_Cover = m_CharacterLocomotion.GetAbility<Cover>();
        }

        /// <summary>
        /// Start the cover ability.
        /// </summary>
        public override void OnStart()
        {
            EventHandler.RegisterEvent<float, Vector3, Vector3, GameObject, Collider>(gameObject, "OnHealthDamage", OnDamage);

            // Initialize the default values.
            m_Duration = Random.Range(m_MinWaitDuration.Value, m_MaxWaitDuration.Value);
            m_LastWaitTime = Time.time;
            m_LastAimTime = -Mathf.Max(m_MaxWaitDuration.Value, m_MaxAimDuration.Value);

            // The agent should not aim immediately after taking cover.
            m_CharacterLocomotion.TryStopAbility(m_Aim);
        }

        /// <summary>
        /// Move between an aiming and non-aiming state.
        /// </summary>
        /// <returns>Always returns a status of Running - this task must be interrupted.</returns>
        public override TaskStatus OnUpdate()
        {
            // Wait to move within cover until the agent is in cover position.
            if (m_Cover.CurrentCoverState < Cover.CoverState.Strafe) {
                return TaskStatus.Running;
            }

            // If the target is null then keep waiting for a target to be in sight of the cover point.
            if (m_Target.Value == null) {
             //   return TaskStatus.Running;
            }

            // Break from cover if the target is no longer in sight.
          //  if (m_DeathmatchAgent.LineOfSight(m_Target.Value.transform, false) == null) {
          //      return TaskStatus.Failure;
          //  }

            // Alternate between aiming and non-aiming. Don't aim if the character is reloading.
            var isReloading = m_CharacterLocomotion.IsAbilityTypeActive<UltimateCharacterController.Character.Abilities.Items.Reload>();
            if (m_Aim.IsActive) {
                if (m_LastAimTime + m_Duration < Time.time || isReloading) {
                    m_CanAttack.Value = false;
                    m_CharacterLocomotion.TryStopAbility(m_Aim);
                    m_LastWaitTime = Time.time;
                    m_Duration = Random.Range(m_MinWaitDuration.Value, m_MaxWaitDuration.Value);
                }
            } else {
                if (m_LastWaitTime + m_Duration < Time.time && !isReloading) {
                    m_CharacterLocomotion.TryStartAbility(m_Aim);
                    m_LastAimTime = Time.time;
                    m_Duration = Random.Range(m_MinAimDuration.Value, m_MaxAimDuration.Value);
                }
            }
            return TaskStatus.Running;
        }

        /// <summary>
        /// Stop the cover ability.
        /// </summary>
        public override void OnEnd()
        {
            m_CharacterLocomotion.TryStopAbility(m_Aim);
            m_CharacterLocomotion.TryStopAbility(m_Cover);
            EventHandler.UnregisterEvent<float, Vector3, Vector3, GameObject, Collider>(gameObject, "OnHealthDamage", OnDamage);
        }

        /// <summary>
        /// The character has taken damage.
        /// </summary>
        /// <param name="amount">The amount of damage taken.</param>
        /// <param name="position">The position of the damage.</param>
        /// <param name="force">The amount of force applied to the object while taking the damage.</param>
        /// <param name="attacker">The GameObject that did the damage.</param>
        /// <param name="hitCollider">The Collider that was hit.</param>
        private void OnDamage(float amount, Vector3 position, Vector3 force, GameObject attacker, Collider hitCollider)
        {
            m_CharacterLocomotion.TryStopAbility(m_Aim);
            m_LastWaitTime = Time.time;
            m_Duration = Random.Range(m_MinWaitDuration.Value, m_MaxWaitDuration.Value);
        }
    }
}