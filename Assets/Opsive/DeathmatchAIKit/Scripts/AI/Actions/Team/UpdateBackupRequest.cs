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
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Updates the backup request with a new target.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class UpdateBackupRequest : Action
    {
        [Tooltip("The GameObject to attack")]
        [SerializeField] protected SharedGameObject m_Target;

        /// <summary>
        /// Update the backup request.
        /// </summary>
        /// <returns>Always returns Success.</returns>
        public override TaskStatus OnUpdate()
        {
            TeamManager.UpdateBackupRequest(gameObject, m_Target.Value.GetCachedParentComponent<UltimateCharacterController.Character.UltimateCharacterLocomotion>().gameObject);

            return TaskStatus.Success;
        }
    }
}