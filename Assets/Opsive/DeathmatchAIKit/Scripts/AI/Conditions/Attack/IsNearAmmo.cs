/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Conditions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using BehaviorDesigner.Runtime.Tasks.UltimateCharacterController;
    using Opsive.Shared.Inventory;
    using Opsive.UltimateCharacterController.Objects.CharacterAssist;
    using System.Collections.Generic;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
    using WeaponStat = DeathmatchAgent.WeaponStat;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Is the agent near ammo and needs that ammo?")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class IsNearAmmo : Conditional
    {
        [Tooltip("The agent needs the weapon if the inventory current has less than the specified amount.")]
        [SerializeField] protected SharedInt m_PickupAmount = 5;
        [Tooltip("The agent needs the power weapon if the inventory current has less than the specified amount.")]
        [SerializeField] protected SharedInt m_PowerWeaponPickupAmount = 2;
        [Tooltip("The agent will seek the ammo if performing a distance check and is within the specified distance.")]
        [SerializeField] protected SharedFloat m_Distance = 10;
        [Tooltip("The ItemType of the ammo that the agent is near.")]
        [SharedRequired] [SerializeField] protected SharedItemDefinitionBase m_ItemDefinition;
        [Tooltip("The position of the detected nearby pickup.")]
        [SharedRequired] [SerializeField] protected SharedVector3 m_TargetPosition;

        private DeathmatchAgent m_DeathmatchAgent;
        private WeaponStat m_PowerItemWeaponStat;
        private List<ItemDefinitionBase> m_ItemDefinitions = new List<ItemDefinitionBase>();
        private List<Transform> m_ItemPickups = new List<Transform>();

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            m_DeathmatchAgent = GetComponent<DeathmatchAgent>();
            
            // Cache item pickups.
            var allItemPickups = GameObject.FindObjectsOfType<ItemPickup>();
            for (int i = 0; i < allItemPickups.Length; ++i) {
                var itemDefinitionAmounts = allItemPickups[i].GetItemDefinitionAmounts();
                for (int j = 0; j < itemDefinitionAmounts.Length; ++j) {
                    if (m_DeathmatchAgent.WeaponStatForItemDefinition(itemDefinitionAmounts[j].ItemDefinition) == null) {
                        continue;
                    }

                    m_ItemDefinitions.Add(itemDefinitionAmounts[j].ItemDefinition);
                    m_ItemPickups.Add(allItemPickups[i].transform);
                    break;
                }
            }
            
            // Cache the Power ItemIdentifier.
            for (int i = 0; i < m_DeathmatchAgent.AvailableWeapons.Length; ++i) {
                 if (m_DeathmatchAgent.AvailableWeapons[i].Class == DeathmatchAgent.WeaponStat.WeaponClass.Power) {
                    m_PowerItemWeaponStat = m_DeathmatchAgent.AvailableWeapons[i];
                    break;
                 }
            }
        }
        
        /// <summary>
        /// Returns success if an item pickup is near.
        /// </summary>
        /// <returns></returns>
        public override TaskStatus OnUpdate()
        {
            // Determine if the agent is close to any pickups.
            var closestDistance = float.MaxValue;
            var closestIndex = -1;
            for (int i = 0; i < m_ItemDefinitions.Count; ++i) {
                if (!m_ItemPickups[i].gameObject.activeInHierarchy) {
                    continue;
                }
                var itemDistance = (m_ItemPickups[i].position - transform.position).magnitude;
                if (itemDistance < m_Distance.Value && itemDistance < closestDistance) {
                    closestDistance = itemDistance;
                    closestIndex = i;
                }
            }

            if (closestIndex != -1) {
                var weaponStat = m_DeathmatchAgent.WeaponStatForItemDefinition(m_ItemDefinitions[closestIndex]);
                if (weaponStat == null) {
                    return TaskStatus.Failure;
                }

                // Check the ammo amount based on the total amount.
                m_TargetPosition.Value = m_ItemPickups[closestIndex].position;
                return weaponStat.GetTotalAmmo() < (weaponStat == m_PowerItemWeaponStat ? m_PowerWeaponPickupAmount.Value : m_PickupAmount.Value) ? TaskStatus.Success : TaskStatus.Failure;
            }

            return TaskStatus.Failure;
        }
    }
}