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
    using Opsive.UltimateCharacterController.Objects.CharacterAssist;
    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections.Generic;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
    
    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Searches for ammo.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class SearchForAmmo : SpeedChangeMovement
    {
        [Tooltip("The ItemDefinition which is low on ammo.")]
        [SharedRequired] [SerializeField] protected SharedItemDefinitionBase m_LowAmmoItemDefinition;
        [Tooltip("Specifies the maximum distance that the character will search for an item.")]
        [SerializeField] protected SharedFloat m_MaxDistance = 20;
        
        private Dictionary<SharedItemDefinitionBase, List<Transform>> m_ItemPickups = new Dictionary<SharedItemDefinitionBase, List<Transform>>();
        private Vector3 m_TargetPosition;
        private NavMeshPath m_NavMeshPath = new NavMeshPath();
        private bool m_PathFound;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            base.OnAwake();

            // Cache all of the ItemPickup locations to prevent having to use FindObjects every frame.
            var allItemPickups = GameObject.FindObjectsOfType<ItemPickup>();
            for (int i = 0; i < allItemPickups.Length; ++i) {
                var itemDefinitionAmounts = allItemPickups[i].GetItemDefinitionAmounts();
                for (int j = 0; j < itemDefinitionAmounts.Length; ++j) {
                    List<Transform> itemPickups;
                    if (!m_ItemPickups.TryGetValue(itemDefinitionAmounts[j].ItemDefinition, out itemPickups)) {
                        itemPickups = new List<Transform>();
                        m_ItemPickups.Add(itemDefinitionAmounts[j].ItemDefinition, itemPickups);
                    }
                    itemPickups.Add(allItemPickups[i].transform);
                }
            }
        }
        
        /// <summary>
        /// Determine the closest ammo position.
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            
            m_PathFound = false;
            var itemPickups = new List<Transform>();
            
            // Return if there are no ItemPickups matching the low-ammo ItemDefinition.
            var matchingItemDefinition = false;
            foreach (KeyValuePair<SharedItemDefinitionBase, List<Transform>> itemPickup in m_ItemPickups) {
                if (itemPickup.Key.Value == m_LowAmmoItemDefinition.Value) {
                    matchingItemDefinition = true;
                    itemPickups = itemPickup.Value;
                    break;
                }
            }
            if (!matchingItemDefinition) {
                return;
            }

            // Move to the closest ItemPickup.
            var closestDistance = Mathf.Infinity;
            float distance;
            for (int i = 0; i < itemPickups.Count; ++i) {
                // Don't go for the item if it's not there.
                if (!itemPickups[i].gameObject.activeInHierarchy) {
                    continue;
                }
                // Use the NavMesh to determine the closest position - just because the item is physically the closest it doesn't mean that the path distance is the closest.
                NavMesh.CalculatePath(transform.position, itemPickups[i].position, NavMesh.AllAreas, m_NavMeshPath);
                if (m_NavMeshPath.corners.Length > 0) {
                    distance = 0;
                    var prevCorner = m_NavMeshPath.corners[0];
                    for (int j = 1; j < m_NavMeshPath.corners.Length; ++j) {
                        distance += Vector3.Distance(m_NavMeshPath.corners[j], prevCorner);
                        prevCorner = m_NavMeshPath.corners[j];
                        // Stop determining the distance if too far away.
                        if (distance > m_MaxDistance.Value) {
                            distance = float.MaxValue;
                            break;
                        }
                    }
                    // Go to the position that has the least distance.
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        m_TargetPosition = itemPickups[i].position;
                    }
                    m_PathFound = true;
                }
            }

            if (m_PathFound) {
                SetDestination(m_TargetPosition);
            }
        }
        
        /// <summary>
        /// Return Success when the agent has arrived at the ammo position.
        /// </summary>
        /// <returns>Success when the agent has arrived at the ammo position, otherwise Running.</returns>
        public override TaskStatus OnUpdate()
        {
            if (!m_PathFound) {
                return TaskStatus.Failure;
            }
            if (HasArrived()) {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}