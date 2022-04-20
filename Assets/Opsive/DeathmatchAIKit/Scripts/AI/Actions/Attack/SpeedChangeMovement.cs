/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using BehaviorDesigner.Runtime.Tasks.Movement;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Character.Abilities;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Allows the character to traverse the NavMesh with the SpeedChange ability.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public abstract class SpeedChangeMovement : NavMeshMovement
    {
        [Tooltip("The probability that the speed change ability will be activated.")]
        [SerializeField] protected SharedFloat m_StartSpeedChangeProbability = 0.9f;

        private UltimateCharacterLocomotion m_CharacterLocomotion;
        private SpeedChange m_SpeedChange;
        private bool m_SpeedChangeStarted;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            base.OnAwake();

            m_CharacterLocomotion = gameObject.GetCachedComponent<UltimateCharacterLocomotion>();
        }

        /// <summary>
        /// Potentially start the SpeedChange ability.
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();

            // There's a chance that the speed change ability should be started.
            m_SpeedChangeStarted = false;
            if (Random.value < m_StartSpeedChangeProbability.Value) {
                if (m_SpeedChange == null) { m_SpeedChange = m_CharacterLocomotion.GetAbility<SpeedChange>(); }
                if (m_SpeedChange != null) {
                    m_CharacterLocomotion.TryStartAbility(m_SpeedChange);
                    m_SpeedChangeStarted = true;
                }
            }
        }

        /// <summary>
        /// Stops the SpeedChange ability if it was activated.
        /// </summary>
        /// <returns>Success when the agent has arrived at the health position, otherwise Running.</returns>
        public override void OnEnd()
        {
            if (m_SpeedChangeStarted) {
                m_CharacterLocomotion.TryStopAbility(m_SpeedChange);
            }
        }
    }
}