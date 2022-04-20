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
    using Opsive.Shared.Game;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
    using WeaponStat = DeathmatchAgent.WeaponStat;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Is the agent low on ammo?")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class IsAmmoLow : Conditional
    {
        [Tooltip("The agent is considered to have low ammo when the count is less than the specified value.")]
        [SerializeField] protected SharedInt m_Amount = 5;
        [Tooltip("Should the agent consider if any power weapons are considered low?")]
        [SerializeField] protected SharedBool m_PowerWeapon;
        [Tooltip("Should the total ammo be considered instead of the individual item's ammo?")]
        [SerializeField] protected SharedBool m_TotalAmmo;
        [Tooltip("The ItemDefinition of the weapon which is low on ammo.")]
        [SharedRequired] [SerializeField] protected SharedItemDefinitionBase m_LowAmmoItemDefinition;
        
        private WeaponStat m_PrimaryWeaponStat;
        private WeaponStat m_SecondaryWeaponStat;
        private WeaponStat m_PowerWeaponStat;
        private DeathmatchAgent m_DeathmatchAgent;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            m_DeathmatchAgent = gameObject.GetCachedComponent<DeathmatchAgent>();

            // Loop through the available weapons to find the correct weapon classes.
            for (int i = 0; i < m_DeathmatchAgent.AvailableWeapons.Length; ++i) {
                 if (m_DeathmatchAgent.AvailableWeapons[i].Class == DeathmatchAgent.WeaponStat.WeaponClass.Primary) {
                    m_PrimaryWeaponStat = m_DeathmatchAgent.AvailableWeapons[i];
                 } else if (m_DeathmatchAgent.AvailableWeapons[i].Class == DeathmatchAgent.WeaponStat.WeaponClass.Secondary) {
                    m_SecondaryWeaponStat = m_DeathmatchAgent.AvailableWeapons[i];
                 } else if (m_DeathmatchAgent.AvailableWeapons[i].Class == DeathmatchAgent.WeaponStat.WeaponClass.Power) {
                    m_PowerWeaponStat = m_DeathmatchAgent.AvailableWeapons[i];
                 }
            }
        }
        
        /// <summary>
        /// Returns Success if the agent is low on ammo.
        /// </summary>
        /// <returns>Success if the agent is low on ammo.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_PowerWeapon.Value) {
                if (m_PowerWeaponStat != null && m_PowerWeaponStat.GetTotalAmmo() < m_Amount.Value) {
                    // Power weapons are treated independently of the primary and secondary weapons.
                    m_LowAmmoItemDefinition.Value = m_PowerWeaponStat.UsableItemDefinition;
                    return TaskStatus.Success;
                }
            } else {
                // If TotalAmmo is true then it doesn't matter which item has ammo as long as at least one item does have ammo.
                // Always prioritise searching for Power weapon ammo if m_PowerWeapon is true.
                if (m_TotalAmmo.Value) {
                    if (m_PrimaryWeaponStat != null && m_SecondaryWeaponStat != null && (!m_PowerWeapon.Value || m_PowerWeaponStat != null)
                            && m_PrimaryWeaponStat.GetTotalAmmo() < m_Amount.Value && m_SecondaryWeaponStat.GetTotalAmmo() < m_Amount.Value && 
                            (!m_PowerWeapon.Value || m_PowerWeaponStat.GetTotalAmmo() < m_Amount.Value)) {
                        m_LowAmmoItemDefinition.Value = (m_PowerWeapon.Value ? m_PowerWeaponStat : m_PrimaryWeaponStat).ItemDefinition;
                        return TaskStatus.Success;
                    }
                } else {
                    if (m_PowerWeapon.Value && m_PowerWeaponStat != null && m_PowerWeaponStat.GetTotalAmmo() < m_Amount.Value) {
                        m_LowAmmoItemDefinition.Value = m_PowerWeaponStat.ItemDefinition;
                        return TaskStatus.Success;
                    }
                    
                    if (m_PrimaryWeaponStat != null && m_PrimaryWeaponStat.GetTotalAmmo() < m_Amount.Value) {
                        m_LowAmmoItemDefinition.Value = m_PrimaryWeaponStat.ItemDefinition;
                        return TaskStatus.Success;
                    }
                    
                    if (m_SecondaryWeaponStat != null && m_SecondaryWeaponStat.GetTotalAmmo() < m_Amount.Value) {
                        m_LowAmmoItemDefinition.Value = m_SecondaryWeaponStat.ItemDefinition;
                        return TaskStatus.Success;
                    }
                }
            }
            
            return TaskStatus.Failure;
        }
    }
}