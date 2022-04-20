/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using Opsive.UltimateCharacterController.Game;
    using UnityEngine;
    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Stores the position of the Transform.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class GetLastPosition : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The position of the Transform.")]
        [RequiredField] public SharedVector3 m_Position;

        private RaycastHit m_RaycastHit;
        private Transform targetTransform;
        private GameObject prevGameObject;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                targetTransform = currentGameObject.GetComponent<Transform>();
                prevGameObject = currentGameObject;
            }
        }

        /// <summary>
        /// Stores the position of the Transform.
        /// </summary>
        /// <returns>Always returns Success.</returns>
        public override TaskStatus OnUpdate()
        {
            if (targetTransform == null) {
                Debug.LogWarning("Transform is null.");
                return TaskStatus.Failure;
            }

            // The character may not be on the ground so fire a raycast from the character's position down so it will hit the ground.
            if (Physics.Raycast(targetTransform.position, Vector3.down, out m_RaycastHit, float.MaxValue, 
                ~((1 << LayerManager.TransparentFX) | (1 << LayerManager.IgnoreRaycast) | (1 << LayerManager.UI) | (1 << LayerManager.VisualEffect) | (1 << LayerManager.Overlay) | (1 << LayerManager.SubCharacter) | (1 << LayerManager.Character)))) {
                m_Position.Value = m_RaycastHit.point;
            } else {
                m_Position.Value = targetTransform.position;
            }

            return TaskStatus.Success;
        }
    }
}