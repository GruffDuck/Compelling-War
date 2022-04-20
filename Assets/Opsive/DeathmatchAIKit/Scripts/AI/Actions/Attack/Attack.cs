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
    using Opsive.Shared.Inventory;
    using Opsive.UltimateCharacterController.Character.Abilities.Items;
    using Opsive.UltimateCharacterController.Items.Actions;
    using Opsive.UltimateCharacterController.Inventory;
    using Opsive.UltimateCharacterController.Traits;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Attacks the target. The agent will only attack if the target is within sight.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class Attack : StartStopUse
    {
        [Tooltip("The maximum field of view that the agent can see other targets (in degrees).")]
        [SerializeField] protected SharedFloat m_FieldOfView = 90;
        [Tooltip("Is the target within sight?")]
        [SerializeField] protected SharedBool m_TargetInSight;
        [Tooltip("The amount to multiply the grenade force by.")]
        [SerializeField] protected SharedFloat m_GrenadeForceMultiplier = 0.7f;

        private DeathmatchAgent m_DeathmatchAgent;
        private ItemSetManagerBase m_ItemSetManager;
        private InventoryBase m_InventoryBase;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            m_DeathmatchAgent = gameObject.GetCachedComponent<DeathmatchAgent>();
            m_ItemSetManager = gameObject.GetCachedComponent<ItemSetManagerBase>();
            m_InventoryBase = gameObject.GetCachedComponent<InventoryBase>();
        }

        /// <summary>
        /// Perform the attack.
        /// </summary>
        /// <returns>Success if the target was attacked.</returns>
        public override TaskStatus OnUpdate()
        {
            // Reset the SharedBool and the Target Transform of the Deathmatch Agent. This will stop the agent from looking at the target. 
            m_TargetInSight.Value = false;

            // Don't attack if the target isn't in sight.
            var targetParent = m_AimTarget.Value.GetCachedParentComponent<Health>().transform;
            if (!m_DeathmatchAgent.TargetInSight(targetParent, m_FieldOfView.Value)) {
                return TaskStatus.Failure;
            }

            // Use should not be started if the character is equipping or unequipping.
            if (m_CharacterLocomotion.IsAbilityTypeActive<EquipUnequip>()) {
                return TaskStatus.Failure;
            }

            // Adjust the throwable velocity.
            if (m_SlotID.Value == -1) {
                for (int i = 0; i < m_InventoryBase.SlotCount; ++i) {
                    if (ItemIdentifierUpdate(m_ItemSetManager.GetEquipItemIdentifier(i), targetParent)) {
                        return TaskStatus.Failure;
                    }
                }
            } else {
                if (ItemIdentifierUpdate(m_ItemSetManager.GetEquipItemIdentifier(m_SlotID.Value), targetParent)) {
                    return TaskStatus.Failure;
                }
            }

            // The target should be attacked. Try to use the item.
            m_TargetInSight.Value = true;
            return base.OnUpdate();
        }

        /// <summary>
        /// Updates the task based on the IItemIdentifier.
        /// </summary>
        /// <param name="itemIdentifier">The IItemIdentifier that should be updated.</param>
        /// <param name="targetParent">The parent of the target Transform.</param>
        /// <returns>True if the attack should stop because of the ItemIdentifier.</returns>
        private bool ItemIdentifierUpdate(IItemIdentifier itemIdentifier, Transform targetParent)
        {
            if (itemIdentifier == null) {
                return false;
            }

            var weaponStat = m_DeathmatchAgent.WeaponStatForItemDefinition(itemIdentifier.GetItemDefinition());
            if (weaponStat == null) {
                return false;
            }

            // The attack should stop if the item is out of ammo.
            if (weaponStat.UsableItemDefinition != null && weaponStat.GetTotalAmmo() == 0) {
                return true;
            }

            // Don't attack if the equipped weapon is too far away from the target.
            if (weaponStat.GetUseLikelihood(Vector3.Distance(transform.position, targetParent.position)) == 0) {
                return true;
            }

            // If the item is a grenade then the throw velocity should be based off of the target distance.
            if (weaponStat.Class == DeathmatchAgent.WeaponStat.WeaponClass.Grenade) {
                var throwableItem = weaponStat.UsableItem as ThrowableItem;
                if (throwableItem != null) {
                    var velocity = throwableItem.Velocity;
                    velocity.z = Vector3.Distance(targetParent.position, transform.position) * m_GrenadeForceMultiplier.Value;
                    throwableItem.Velocity = velocity;
                }
            }

            return false;
        }

        /// <summary>
        /// Reset the Behavior Designer variables.
        /// </summary>
        public override void OnReset()
        {
            m_TargetInSight = false;
        }
    }
}