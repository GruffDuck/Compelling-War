

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Character.Abilities.Items;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Resets the variables.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class Reset : Action
    {
        [Tooltip("The GameObject to attack.")]
        [SerializeField] protected SharedGameObject m_Target;
        [Tooltip("The possible GameObject to attack.")]
        [SerializeField] protected SharedGameObject m_PossibleTarget;
        [Tooltip("A set of targets that the agent should ignore.")]
        [SerializeField] protected SharedGameObjectSet m_IgnoreTargets;
        [Tooltip("The GameObject which the agent is using as cover.")]
        [SerializeField] protected SharedCoverPoint m_CoverPoint;
        [Tooltip("The leader of the group.")]
        [SerializeField] protected SharedGameObject m_Leader;
        [Tooltip("The teammate requesting backup.")]
        [SerializeField] protected SharedGameObject m_BackupRequestor;
        [Tooltip("The target of the teammate who is requesting backup.")]
        [SerializeField] protected SharedGameObject m_BackupTarget;
        [Tooltip("A bool representing if the agent should search for the target.")]
        [SerializeField] protected SharedBool m_Search;
        [Tooltip("A bool representing if the agent can attack the target.")]
        [SerializeField] protected SharedBool m_CanAttack;
        [Tooltip("Should the cancel backup request be sent?")]
        [SerializeField] protected SharedBool m_CancelBackupRequest;
        [Tooltip("Should the aim ability be stopped?")]
        [SerializeField] protected SharedBool m_StopAimAbility;

        private UltimateCharacterLocomotion m_CharacterLocomotion;
        private Aim m_AimAbility;
        private LocalLookSource m_LocalLookSource;

        /// <summary>
        /// Initializet the default values.
        /// </summary>
        public override void OnAwake()
        {
            m_CharacterLocomotion = gameObject.GetCachedComponent<UltimateCharacterLocomotion>();
            m_AimAbility = m_CharacterLocomotion.GetAbility<Aim>();
            m_LocalLookSource = gameObject.GetCachedComponent<LocalLookSource>();
        }

        /// <summary>
        /// Reset the SharedVariables.
        /// </summary>
        /// <returns>Always returns Success.</returns>
        public override TaskStatus OnUpdate()
        {
            if (!m_Target.IsNone) {
                m_Target.Value = null;
                m_LocalLookSource.Target = null;
            }
            if (!m_PossibleTarget.IsNone) {
                m_PossibleTarget.Value = null;
            }
            if (!m_IgnoreTargets.IsNone) {
                m_IgnoreTargets.Value.Clear();
            }
            if (!m_CoverPoint.IsNone) {
                if (m_CoverPoint.Value != null) {
                    m_CoverPoint.Value.Occupant = null;
                }
                m_CoverPoint.Value = null;
            }
            if (!m_Leader.IsNone) {
                m_Leader.Value = null;
            }
            if (!m_BackupRequestor.IsNone) {
                m_BackupRequestor.Value = null;
            }
            if (!m_BackupTarget.IsNone) {
                m_BackupTarget.Value = null;
            }
            if (!m_Search.IsNone) {
                m_Search.Value = false;
            }
            if (!m_CanAttack.IsNone) {
                m_CanAttack.Value = true;
            }
            if (TeamManager.IsInstantiated && m_CancelBackupRequest.Value) {
                TeamManager.CancelBackupRequest(gameObject);
            }
            if (m_StopAimAbility.Value) {
                m_CharacterLocomotion.TryStopAbility(m_AimAbility);
            }

            return TaskStatus.Success;
        }
    }
}