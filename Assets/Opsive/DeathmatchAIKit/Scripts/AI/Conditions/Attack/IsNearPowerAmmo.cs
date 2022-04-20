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
    using Opsive.Shared.Inventory;
    using Opsive.UltimateCharacterController.Objects.CharacterAssist;
    using System.Collections.Generic;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
    using WeaponStat = DeathmatchAgent.WeaponStat;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Returns success if near a Power weapon ammo pickup, otherwise returns failure.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class IsNearPowerAmmo : Conditional
    {
        [Tooltip("Maximum distance to check for pickups.")]
        [SerializeField] protected SharedFloat m_Distance = 10;
        [Tooltip("The position of the detected nearby pickup.")]
        [SharedRequired] [SerializeField] protected SharedVector3 m_TargetPosition;

        private DeathmatchAgent m_DeathmatchAgent;
        private WeaponStat m_PowerWeaponStat;

        private List<ItemDefinitionBase> m_ItemDefinitions = new List<ItemDefinitionBase>();
        private List<Transform> m_ItemPickups = new List<Transform>();

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            m_DeathmatchAgent = gameObject.GetCachedComponent<DeathmatchAgent>();
            
            // Cache the Power ItemIdentifier.
            for (int i = 0; i < m_DeathmatchAgent.AvailableWeapons.Length; ++i) {
                 if (m_DeathmatchAgent.AvailableWeapons[i].Class == DeathmatchAgent.WeaponStat.WeaponClass.Power) {
                    m_PowerWeaponStat = m_DeathmatchAgent.AvailableWeapons[i];
                 }
            }
            if (m_PowerWeaponStat == null) {
                return;
            }

            // Cache Power item pickups.
            var allItemPickups = GameObject.FindObjectsOfType<ItemPickup>();
            for (int i = 0; i < allItemPickups.Length; ++i) {
                for (int j = 0; j < allItemPickups[i].GetItemDefinitionAmounts().Length; ++j) {
                    // Only add the pickup to the list if it matches the agent's Power weapon ItemDefinition.
                    var itemDefinition = allItemPickups[i].GetItemDefinitionAmounts()[j].ItemDefinition;
                    if (itemDefinition == m_PowerWeaponStat.ItemDefinition) {
                        m_ItemDefinitions.Add(itemDefinition);
                        m_ItemPickups.Add(allItemPickups[i].transform);
                    }
                }
            }
        }
        
        /// <summary>
        /// Returns success if a Power weapon pickup is near.
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
                m_TargetPosition.Value = m_ItemPickups[closestIndex].position;
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}