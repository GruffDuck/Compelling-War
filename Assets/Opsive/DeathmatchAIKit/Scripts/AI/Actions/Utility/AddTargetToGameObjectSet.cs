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
    using Opsive.Shared.Game;
    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Adds the target parent to the GameObject Set.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class AddTargetToGameObjectSet : Action
    {
        [Tooltip("The value to add to the SharedGameObjectList.")]
        public SharedGameObject target;
        [RequiredField]
        [Tooltip("The SharedGameObjectSet to add to")]
        public SharedGameObjectSet targetVariable;

        /// <summary>
        /// Adds the target to the GameObjectSet. 
        /// </summary>
        /// <returns></returns>
        public override TaskStatus OnUpdate()
        {
            // Use the target parent instead of the direct GameObject.
            targetVariable.Value.Add(target.Value.GetCachedParentComponent<UltimateCharacterController.Character.UltimateCharacterLocomotion>().gameObject);

            return TaskStatus.Success;
        }

        /// <summary>
        /// Reset the Behavior Designer variables.
        /// </summary>
        public override void OnReset()
        {
            target = null;
            targetVariable = null;
        }
    }
}