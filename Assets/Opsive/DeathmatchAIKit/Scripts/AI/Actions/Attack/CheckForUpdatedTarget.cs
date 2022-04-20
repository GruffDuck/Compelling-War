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
    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Determines if the agent should switch targets after being requested for backup.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class CheckForUpdatedTarget : Action
    {
        [Tooltip("The agent requesting backup")]
        [SerializeField] protected SharedGameObject m_Requstor;
        [Tooltip("The target which the requestor asked for backup because of")]
        [SerializeField] protected SharedGameObject m_BackupTarget;

        private bool m_EventReceived = false;
        private bool m_Registered = false;
        private bool m_UpdateTarget;

        /// <summary>
        /// Register for the UpdateBackupRequest event.
        /// </summary>
        public override void OnStart()
        {
            if (!m_Registered) {
                Owner.RegisterEvent<object, object>("UpdateBackupRequest", UpdateBackupRequest);
                m_Registered = true;
                m_UpdateTarget = false;
            }
        }

        /// <summary>
        /// Determine if the target should be switched.
        /// </summary>
        /// <returns>Success if the target was switched.</returns>
        public override TaskStatus OnUpdate()
        {
            return m_UpdateTarget ? TaskStatus.Success : TaskStatus.Failure;
        }

        /// <summary>
        /// The agent has updated their target.
        /// </summary>
        /// <param name="requestor">The agent requesting backup.</param>
        /// <param name="target">Thew new backup target.</param>
        private void UpdateBackupRequest(object requestor, object target)
        {
            // Only update the backup target if the requestor is the same as the existing requestor. This will prevent another agent from switching to another agent's target.
            if (requestor.Equals(m_Requstor.Value)) {
                m_BackupTarget.Value = target as GameObject;
                m_UpdateTarget = true;
            }
            m_EventReceived = true;
        }

        /// <summary>
        /// The task has ended. Unsubscribe from the event.
        /// </summary>
        public override void OnEnd()
        {
            if (m_EventReceived) {
                Owner.UnregisterEvent<object, object>("UpdateBackupRequest", UpdateBackupRequest);
                m_Registered = false;
            }
            m_EventReceived = false;
        }
    }
}