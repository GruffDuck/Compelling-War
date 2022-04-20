/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using BehaviorDesigner.Runtime.Tasks.UltimateCharacterController;
    using Opsive.Shared.Game;
    using Opsive.Shared.Events;
    using Opsive.UltimateCharacterController.Character.Abilities.Items;
    using Opsive.UltimateCharacterController.Items.Actions;
    using Opsive.UltimateCharacterController.Inventory;
    using Opsive.UltimateCharacterController.Traits;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Tries to throw the grenade. This task requires the grenade to already be equipped.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class TryThrowGrenade : StartStopUse
    {
        [Tooltip("A random probability that the grenade can be thrown.")]
        [SerializeField] protected SharedFloat m_ThrowProbability = 0.7f;
        [Tooltip("The maximum field of view that the agent can see other targets (in degrees)")]
        [SerializeField] protected SharedFloat m_FieldOfView = 90;
        [Tooltip("Specifies the amount of time to wait before the grenade is able to be thrown again.")]
        [SerializeField] protected SharedFloat m_GrenadeTimeout = 4;
        [Tooltip("Specifies the amount of time to wait before selecting the power weapon again after shooting it.")]
        [SerializeField] protected SharedFloat m_PowerTimeout = 2;
        [Tooltip("The amount to multiply the grenade force by.")]
        [SerializeField] protected SharedFloat m_GrenadeForceMultiplier = 0.7f;

        private DeathmatchAgent m_DeathmatchAgent;
        private InventoryBase m_InventoryBase;
        private ItemSetManagerBase m_ItemSetManager;

        private bool m_Thrown;
        private float m_GrenadeThrowTime;
        private float m_PowerUseTime;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            m_DeathmatchAgent = gameObject.GetCachedComponent<DeathmatchAgent>();
            m_InventoryBase = gameObject.GetCachedComponent<InventoryBase>();
            m_ItemSetManager = gameObject.GetCachedComponent<ItemSetManagerBase>();

            m_GrenadeThrowTime = -m_GrenadeTimeout.Value;
            m_PowerUseTime = -m_PowerTimeout.Value;
            EventHandler.RegisterEvent<IUsableItem>(gameObject, "OnUseAbilityUsedItem", OnUsedItem);
        }

        /// <summary>
        /// Tries to throw the grenade.
        /// </summary>
        /// <returns>Success if the target was attacked.</returns>
        public override TaskStatus OnUpdate()
        {
            // If the grenade has already been thrown defer to the base class.
            if (m_Thrown) {
                if (!m_UseAbility.IsActive) {
                    return TaskStatus.Success;
                }
                return base.OnUpdate();
            }

            if (Random.value > m_ThrowProbability.Value) {
                return TaskStatus.Failure;
            }

            if (m_SlotID.Value == -1) {
                return TaskStatus.Failure;
            }

            // The grenade can't be thrown if there are no grenades left.
            var item = m_InventoryBase.GetActiveItem(m_SlotID.Value);
            if (item == null || m_InventoryBase.GetItemIdentifierAmount(item.ItemIdentifier) == 0) {
                return TaskStatus.Failure;
            }

            // The item has to be a grenade.
            var weaponStat = m_DeathmatchAgent.WeaponStatForItemDefinition(item.ItemDefinition);
            if (weaponStat.Class != DeathmatchAgent.WeaponStat.WeaponClass.Grenade) {
                return TaskStatus.Failure;
            }

            // Don't throw the grenade if the target is too far away.
            var targetParent = m_AimTarget.Value.GetCachedParentComponent<Health>().transform;
            if (weaponStat.GetUseLikelihood(Vector3.Distance(transform.position, targetParent.position)) == 0) {
                return TaskStatus.Failure;
            }

            // Don't attack if the target isn't in sight.
            if (!m_DeathmatchAgent.TargetInSight(targetParent, m_FieldOfView.Value)) {
                return TaskStatus.Failure;
            }

            // Use should not be started if the character is equipping or unequipping.
            if (m_CharacterLocomotion.IsAbilityTypeActive<EquipUnequip>()) {
                return TaskStatus.Failure;
            }

            // Don't throw the grenade if:
            // - A power weapon has been used recently.
            // - It has been thrown recently.
            // - The target isn't at the same relative height as the agent.
            if (m_PowerUseTime + m_PowerTimeout.Value > Time.time || 
                m_GrenadeThrowTime + m_GrenadeTimeout.Value > Time.time ||
                Mathf.Abs(targetParent.position.y - transform.position.y) > m_CharacterLocomotion.Height) {
                return TaskStatus.Failure;
            }

            // The grenade can only be thrown based on the use liklihood.
            var targetDistance = Vector3.Distance(transform.position, targetParent.position);
            if (targetDistance < weaponStat.MinUseDistance || targetDistance > weaponStat.MaxUseDistance) {
                return TaskStatus.Failure;
            }

            // The grenade should not be used if a power weapon is equipped.
            for (int i = 0; i < m_InventoryBase.SlotCount; ++i) {
                if (i == m_SlotID.Value) {
                    continue;
                }

                var equippedItem = m_InventoryBase.GetActiveItem(m_SlotID.Value);
                if (equippedItem != null && m_DeathmatchAgent.WeaponStatForItemDefinition(equippedItem.ItemDefinition).Class == DeathmatchAgent.WeaponStat.WeaponClass.Power) {
                    return TaskStatus.Failure;
                }

                var equippingItem = m_ItemSetManager.GetNextItemIdentifier(i, out var categoryIndex);
                if (equippedItem != null && m_DeathmatchAgent.WeaponStatForItemDefinition(equippingItem.GetItemDefinition()).Class == DeathmatchAgent.WeaponStat.WeaponClass.Power) {
                    return TaskStatus.Failure;
                }
            }

            // Adjust the throwable velocity.
            var throwableItem = weaponStat.UsableItem as ThrowableItem;
            if (throwableItem != null) {
                var velocity = throwableItem.Velocity;
                velocity.z = Vector3.Distance(targetParent.position, transform.position) * m_GrenadeForceMultiplier.Value;
                throwableItem.Velocity = velocity;
            }

            // The grenade can be thrown.
            var status = base.OnUpdate();
            // A non-failure status indicates that the grenade has been thrown. Update the time.
            if (status == TaskStatus.Running || status == TaskStatus.Success) {
                m_GrenadeThrowTime = Time.time;
                m_Thrown = true;
            }
            return status;
        }

        /// <summary>
        /// The task has ended.
        /// </summary>
        public override void OnEnd()
        {
            base.OnEnd();

            m_Thrown = false;
        }

        /// <summary>
        /// The Use ability has used the IUsableItem.
        /// </summary>
        /// <param name="usableItem">The IUsableItem that has been used.</param>
        private void OnUsedItem(IUsableItem usableItem)
        {
            var item = usableItem.Item;
            var weaponStat = m_DeathmatchAgent.WeaponStatForItemDefinition(item.ItemDefinition);
            if (weaponStat.Class == DeathmatchAgent.WeaponStat.WeaponClass.Power) {
                m_PowerUseTime = Time.time;
            } else if (weaponStat.Class == DeathmatchAgent.WeaponStat.WeaponClass.Grenade) {
                m_GrenadeThrowTime = Time.time;
                m_CharacterLocomotion.TryStopAbility(m_UseAbility, true);
            }
        }

        /// <summary>
        /// The behavior tree has completed.
        /// </summary>
        public override void OnBehaviorComplete()
        {
            base.OnBehaviorComplete();

            EventHandler.UnregisterEvent<IUsableItem>(gameObject, "OnUseAbilityUsedItem", OnUsedItem);
        }
    }
}