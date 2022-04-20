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
    [TaskDescription("Requests backup from other teammates.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class RequestBackup : Action
    {
        [Tooltip("The GameObject to attack")]
        [SerializeField] protected SharedGameObject m_Target;

        /// <summary>
        /// Request backup from the agent's teammates.
        /// </summary>
        /// <returns>Always returns Success.</returns>
        public override TaskStatus OnUpdate()
        {
            TeamManager.RequestBackup(gameObject, m_Target.Value.GetCachedParentComponent<UltimateCharacterController.Character.UltimateCharacterLocomotion>().gameObject);

            return TaskStatus.Success;
        }
    }
}