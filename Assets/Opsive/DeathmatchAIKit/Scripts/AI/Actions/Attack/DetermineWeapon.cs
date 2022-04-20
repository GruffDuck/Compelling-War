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
    using Opsive.UltimateCharacterController.Inventory;
    using Opsive.UltimateCharacterController.Items.Actions;
    using Opsive.UltimateCharacterController.Traits;
    using Opsive.Shared.Events;
    using Opsive.Shared.Game;
    using System.Collections.Generic;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
    using WeaponClass = DeathmatchAgent.WeaponStat.WeaponClass;
    using Opsive.UltimateCharacterController.Character;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Determines the best weapon to be used against the target.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class DetermineWeapon : Action
    {
        [Tooltip("Specifies the target to attack.")]
        [SerializeField] protected SharedGameObject m_Target;
        [Tooltip("The ID of the slot that should be determined. A value of -1 indicates that no slot is specified.")]
        [SerializeField] protected SharedInt m_SlotID = -1;
        [Tooltip("The agent has low health when the health is lower than this value.")]
        [SerializeField] protected SharedFloat m_LowHealth;
        [Tooltip("The size of the radius to determine if the target is within a group.")]
        [SerializeField] protected SharedFloat m_GroupRadius = 5;
        [Tooltip("The number of targets which must be within the radius in order to be considered a group.")]
        [SerializeField] protected SharedInt m_GroupCount = 2;
        [Tooltip("Each available weapon is going to be checked to determine its use likelihood. Switch weapons if the use likelihood is greater than the specified value.")]
        [SerializeField] protected SharedFloat m_LikelihoodAdvantage = 1;
        [Tooltip("Specifies the amount of time to wait before selecting the grenade again after throwing it.")]
        [SerializeField] protected SharedFloat m_GrenadeTimeout = 4;
        [Tooltip("Specifies the amount of time to wait before selecting the power weapon again after shooting it.")]
        [SerializeField] protected SharedFloat m_PowerTimeout = 2;
        [Tooltip("The category that the ability should respond to.")]
        [ItemSetCategoryDrawer] [SerializeField] protected SharedUInt m_CategoryID;
        [Tooltip("The ItemSet index that should be equipped.")]
        [SerializeField] [SharedRequired] protected SharedInt m_ItemSetIndex;

        private List<DeathmatchAgent.WeaponStat> m_WeaponPool = new List<DeathmatchAgent.WeaponStat>();
        private Collider[] m_HitColliders;
        private float m_GrenadeThrowTime;
        private float m_PowerUseTime;

        private DeathmatchAgent m_DeathmatchAgent;
        private CharacterLayerManager m_CharacterLayerManager;
        private InventoryBase m_Inventory;
        private Health m_Health;

        /// <summary>
        /// Cache the component references and initialize the SharedFields.
        /// </summary>
        public override void OnAwake()
        {
            m_DeathmatchAgent = gameObject.GetCachedComponent<DeathmatchAgent>();
            m_CharacterLayerManager = gameObject.GetCachedComponent<CharacterLayerManager>();
            m_Inventory = gameObject.GetCachedComponent<InventoryBase>();
            m_Health = gameObject.GetCachedComponent<Health>();
            
            m_HitColliders = new Collider[m_GroupCount.Value + 1];
            m_GrenadeThrowTime = -m_GrenadeTimeout.Value;
            m_PowerUseTime = -m_PowerTimeout.Value;

            EventHandler.RegisterEvent<IUsableItem>(gameObject, "OnUseAbilityUsedItem", OnUsedItem);
        }
        /// <summary>
        /// Determines which weapon to switch to. Returns Success if a new weapon should be switched to.
        /// </summary>
        /// <returns>Success if a new weapon should be switched to.</returns>
        public override TaskStatus OnUpdate()
        {
            // Return failure if target doesn't exist.
            if (m_Target.Value == null) {
                return TaskStatus.Failure;
            }

            // Get a list of all weapons that the character currently has. The character can initially use any available weapon.
            m_WeaponPool.Clear();
            var allItems = m_Inventory.GetAllItems();
            for (int i = 0; i < allItems.Count; ++i) {
                // Weapons can be filtered by the slot.
                if (m_SlotID.Value != -1 && allItems[i].SlotID != m_SlotID.Value) {
                    continue;
                }
                m_WeaponPool.Add(m_DeathmatchAgent.WeaponStatForItemDefinition(allItems[i].ItemDefinition));
            }
            
            // Start eliminating possible weapons.
            var targetDistance = float.MaxValue;
            Transform targetParent = null;
            if (m_Target.Value != null) {
                targetParent = m_Target.Value.GetCachedParentComponent<Health>().transform;
                targetDistance = Vector3.Distance(transform.position, targetParent.position);
            }
            var containsGroupDamage = false;
            for (int i = m_WeaponPool.Count - 1; i > -1; --i) {
                // The agent has to have the weapon.
                if (m_Inventory.GetItemIdentifierAmount(m_WeaponPool[i].Item.ItemIdentifier) == 0) {
                    m_WeaponPool.RemoveAt(i);
                    continue;
                }

                // Can't use the weapon if it is out of ammo.
                if (m_WeaponPool[i].Class != WeaponClass.Melee) {
                    var usableItemAmount = m_WeaponPool[i].GetTotalAmmo();
                    if (usableItemAmount == 0) {
                        m_WeaponPool.RemoveAt(i);
                        continue;
                    }
                }
                
                // Can't use the weapon if it is out of range (its use likelihood is 0 at the current distance to target)
                if (targetDistance != float.MaxValue) {
                    if (m_WeaponPool[i].GetUseLikelihood(targetDistance) <= 0) {
                        m_WeaponPool.RemoveAt(i);
                        continue;
                    }
                }
                
                // Don't use the grenade if:
                // - It has been thrown recently.
                // - A rocket has been fired recently.
                // - The target isn't at the same relative height as the agent.
                if (m_WeaponPool[i].Class == WeaponClass.Grenade) {
                    if (m_GrenadeThrowTime + m_GrenadeTimeout.Value > Time.time || m_PowerUseTime + m_PowerTimeout.Value > Time.time ||
                        Mathf.Abs(targetParent.position.y - transform.position.y) > 2) {
                        m_WeaponPool.RemoveAt(i);
                        continue;
                    }
                }
                
                // Don't use the rocket if:
                // - It has been fired recently.
                // - A grenade has been thrown recently.
                if (m_WeaponPool[i].Class == WeaponClass.Power) {
                    if (m_PowerUseTime + m_PowerTimeout.Value > Time.time || m_GrenadeThrowTime + m_GrenadeTimeout.Value > Time.time) {
                        m_WeaponPool.RemoveAt(i);
                        continue;
                    }
                }
                
                // Only check for groups if at least one weapon is marked for group damage.
                if (!containsGroupDamage) {
                    containsGroupDamage = m_WeaponPool[i].GroupDamage;
                }
            }
            
            // Any of remaining weapons in the weapon pool can be used. Determine which weapon is best for the current situation.
            if (m_WeaponPool.Count == 1) {
                m_ItemSetIndex.Value = m_WeaponPool[0].ItemSetIndex;
                return TaskStatus.Success;
            }
            
            var itemIndex = -1;
            // Use a power weapon if low on health and not too close to the target.
            if (m_Health.Value < m_LowHealth.Value) {
                itemIndex = GetWeaponClassIndex(WeaponClass.Power);
                if (itemIndex != -1 && m_WeaponPool[itemIndex].GetUseLikelihood(targetDistance) > 0) {
                    m_ItemSetIndex.Value = m_WeaponPool[itemIndex].ItemSetIndex;
                    return TaskStatus.Success;
                }
            }
            
            // Use a weapon which adds group damage if there are many enemies near the target. Use > because the current agent will be included in the group count.
            var maxLikelihood = float.MinValue;
            if (containsGroupDamage) {
                var overlapCount = Physics.OverlapSphereNonAlloc(targetParent.position, m_GroupRadius.Value, m_HitColliders, (1 << gameObject.layer) | m_CharacterLayerManager.EnemyLayers, QueryTriggerInteraction.Ignore);
                if (overlapCount >= m_GroupCount.Value) {
                    var containsTeammates = false;
                    // Do not use a group damage weapon if there are teammates nearby.
                    if (TeamManager.IsInstantiated) {
                        for (int i = 0; i < overlapCount; ++i) {
                            if (m_HitColliders[i].gameObject == gameObject) {
                                continue;
                            }

                            if (TeamManager.IsTeammate(gameObject, m_HitColliders[i].gameObject)) {
                                containsTeammates = true;
                                break;
                            }
                        }
                    }

                    if (!containsTeammates) {
                        for (int i = 0; i < m_WeaponPool.Count; ++i) {
                            if (m_WeaponPool[i].GroupDamage) {
                                var likelihood = m_WeaponPool[i].GetUseLikelihood(targetDistance);
                                if (likelihood > maxLikelihood) {
                                    maxLikelihood = likelihood;
                                    itemIndex = i;
                                }
                            }
                        }
                        
                        if (itemIndex != -1) {
                            m_ItemSetIndex.Value = m_WeaponPool[itemIndex].ItemSetIndex;
                            return TaskStatus.Success;
                        }
                    }
                }
            }
            
            // Loop through all of the available weapons. Determine if a particular weapon has a clear advantage for the particular distance.
            maxLikelihood = float.MinValue;
            var secondaryLikelihood = float.MinValue;
            for (int i = 0; i < m_WeaponPool.Count; ++i) {
                var likelihood = m_WeaponPool[i].GetUseLikelihood(targetDistance);
                if (likelihood > maxLikelihood) {
                    secondaryLikelihood = maxLikelihood;
                    maxLikelihood = likelihood;
                    itemIndex = i;
                }
            }
            if (itemIndex != -1 && secondaryLikelihood != float.MinValue && maxLikelihood / secondaryLikelihood > m_LikelihoodAdvantage.Value) {
                m_ItemSetIndex.Value = m_WeaponPool[itemIndex].ItemSetIndex;
                return TaskStatus.Success;
            }
            
            // There isn't a clear advantage of using one weapon over another. Use the primary weapon if it is available, otherwise the secondary.
            itemIndex = GetWeaponClassIndex(WeaponClass.Primary);
            if (itemIndex != -1) {
                m_ItemSetIndex.Value = m_WeaponPool[itemIndex].ItemSetIndex;
                return TaskStatus.Success;
            }
            itemIndex = GetWeaponClassIndex(WeaponClass.Secondary);
            if (itemIndex != -1) {
                m_ItemSetIndex.Value = m_WeaponPool[itemIndex].ItemSetIndex;
                return TaskStatus.Success;
            }
            // There is no primary or secondary weapon available. Use the melee weapon.
            itemIndex = GetWeaponClassIndex(WeaponClass.Melee);
            if (itemIndex != -1) {
                m_ItemSetIndex.Value = m_WeaponPool[itemIndex].ItemSetIndex;
                return TaskStatus.Success;
            }
            
            // No items are within their range. If the target distance is too far then use the primary or secondary weapon (depending on which weapon has ammo).
            // If the target distance is too close or both weapons are out of ammo then then use the melee weapon.
            DeathmatchAgent.WeaponStat primaryWeaponStat = null;
            DeathmatchAgent.WeaponStat secondaryWeaponStat = null;
            DeathmatchAgent.WeaponStat meleeWeaponStat = null;
            DeathmatchAgent.WeaponStat weaponStat = null;
            for (int i = 0; i < allItems.Count; ++i) {
                if ((weaponStat = m_DeathmatchAgent.WeaponStatForItemDefinition(allItems[i].ItemDefinition)).Class == WeaponClass.Primary) {
                    primaryWeaponStat = weaponStat;
                } else if ((weaponStat = m_DeathmatchAgent.WeaponStatForItemDefinition(allItems[i].ItemDefinition)).Class == WeaponClass.Secondary) {
                    secondaryWeaponStat = weaponStat;
                } else if ((weaponStat = m_DeathmatchAgent.WeaponStatForItemDefinition(allItems[i].ItemDefinition)).Class == WeaponClass.Melee) {
                    meleeWeaponStat = weaponStat;
                }
            }
            
            // Use the primary or secondary weapon if the target is far away.
            if (primaryWeaponStat != null && targetDistance > primaryWeaponStat.MaxUseDistance) {
                if (primaryWeaponStat.GetTotalAmmo() > 0) {
                    m_ItemSetIndex.Value = primaryWeaponStat.ItemSetIndex;
                    return TaskStatus.Success;
                }
                
                if (secondaryWeaponStat != null && secondaryWeaponStat.GetTotalAmmo() > 0) {
                    m_ItemSetIndex.Value = secondaryWeaponStat.ItemSetIndex;
                    return TaskStatus.Success;
                }
            }
            
            // Either the target distance is too close or the primary and secondary weapons are out of ammo. Return the melee weapon.
            if (meleeWeaponStat != null) {
                m_ItemSetIndex.Value = meleeWeaponStat.ItemSetIndex;
                return TaskStatus.Success;
            }

            // There are no weapons available. Return failure.
            return TaskStatus.Failure;
        }

        /// <summary>
        /// Returns the index of the specified WeaponClass.
        /// </summary>
        /// <param name="weaponClass">The WeaponClass to compare to.</param>
        /// <returns>The index of the specified WeaonClass. -1 if there are no weapons within the weapon pool which match the WeaponClass.</returns>
        private int GetWeaponClassIndex(WeaponClass weaponClass)
        {
            for (int i = 0; i < m_WeaponPool.Count; ++i) {
                if (m_WeaponPool[i].Class == weaponClass) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// The Use ability has used the IUsableItem.
        /// </summary>
        /// <param name="usableItem">The IUsableItem that has been used.</param>
        private void OnUsedItem(IUsableItem usableItem)
        {
            var item = usableItem.Item;
            var weaponStat = m_DeathmatchAgent.WeaponStatForItemDefinition(item.ItemDefinition);
            if (weaponStat.Class == WeaponClass.Power) {
                m_PowerUseTime = Time.time;
            } else if (weaponStat.Class == WeaponClass.Grenade) {
                m_GrenadeThrowTime = Time.time;
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