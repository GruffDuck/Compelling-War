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
    [TaskDescription("Is the agent's health low?")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class IsHealthLow : Conditional
    {
          [Tooltip("The value to compare to.")]
          [SerializeField] protected SharedFloat m_Amount;

          private Health m_Health;

          /// <summary>
          /// Initialize the default values.
          /// </summary>
          public override void OnAwake()
          {
              m_Health = gameObject.GetCachedComponent<Health>();
          }

          /// <summary>
          /// Returns Success if the agent's health is low.
          /// </summary>
          /// <returns>Success if the agent's health is low.</returns>
          public override TaskStatus OnUpdate()
          {
              return m_Health.Value < m_Amount.Value ? TaskStatus.Success : TaskStatus.Failure;
          }

          /// <summary>
          /// Reset the SharedVariable values.
          /// </summary>
          public override void OnReset()
          {
              m_Amount = 0;
          }
    }
}