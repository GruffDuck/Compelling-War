/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Conditions
{
    using UnityEngine;
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Inventory;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Is the target near explosive?")]
    [TaskIcon("Assets/Deathmatch AI Kit/Editor/Images/Icons/DeathmatchAIKitIcon.png")]
    public class IsTargetNearExplosive : Conditional
    {
        [Tooltip("The current target GameObject.")]
        [SerializeField] protected SharedGameObject m_Target;
        [Tooltip("The distance to check for explosives.")]
        [SerializeField] protected SharedFloat m_Radius = 10;
        [Tooltip("Ignore the explosive if it is too close to the agent.")]
        [SerializeField] protected SharedFloat m_TooCloseDistance = 5;
        [Tooltip("The layers that the explosive GameObjects are set to.")]
        [SerializeField] protected LayerMask m_ExplosiveLayer;
        [Tooltip("The found explosive.")]
        [SerializeField] protected SharedGameObject m_Explosive;

        private InventoryBase m_Inventory;
        private DeathmatchAgent m_DeathmatchAgent;
        private Collider[] m_HitColliders;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            m_Inventory = gameObject.GetCachedComponent<InventoryBase>();
            m_DeathmatchAgent = gameObject.GetCachedComponent<DeathmatchAgent>();
            m_HitColliders = new Collider[10];
        }

        /// <summary>
        /// Returns success if there is an explosive within distance of the agent.
        /// </summary>
        /// <returns>Success if there is an explosive within distance of the agent.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_Target.Value == null) {
                return TaskStatus.Failure;
            }

            // Do not attack the target if the current agent is holding a melee weapon.
            for (int i = 0; i < m_Inventory.SlotCount; ++i) {
                var item = m_Inventory.GetActiveItem(i);
                if (item == null) {
                    continue;
                }

                var weaponStat = m_DeathmatchAgent.WeaponStatForItemDefinition(item.ItemDefinition);
                if (weaponStat.Class == DeathmatchAgent.WeaponStat.WeaponClass.Melee) {
                    return TaskStatus.Failure;
                }
            }

            GameObject explosive = null;
            var closestDistance = float.MaxValue;
            var targetTransform = m_Target.Value.transform;
            var hitCount = 0;
            if ((hitCount = Physics.OverlapSphereNonAlloc(m_Target.Value.transform.position, m_Radius.Value, m_HitColliders, m_ExplosiveLayer, QueryTriggerInteraction.Ignore)) > 0) {
                for (int i = 0; i < hitCount; ++i) {
                    // Ignore exposives too close to the current agent.
                    if ((transform.position - m_HitColliders[i].transform.position).magnitude < m_TooCloseDistance.Value) {
                        continue;
                    }

                    // Find the closest explosive to the target.
                    var distance = (targetTransform.position - m_HitColliders[i].transform.position).magnitude;
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        explosive = m_HitColliders[i].gameObject;
                    }
                }
            }

            m_Explosive.Value = explosive;

            return explosive != null ? TaskStatus.Success : TaskStatus.Failure;
        }

        /// <summary>
        /// Reset the Behavior Designer variables.
        /// </summary>
        public override void OnReset()
        {
            m_Target = null;
            m_Radius = 10;
            m_TooCloseDistance = 5;
            m_Explosive = null;
        }
    }
}
