
namespace Opsive.DeathmatchAIKit.Demo.UI
{
    using Opsive.Shared.Events;
    using Opsive.Shared.Input;
    using Opsive.Shared.Inventory;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Character.Abilities.Items;
    using Opsive.UltimateCharacterController.Inventory;
    using Opsive.UltimateCharacterController.UI;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The ItemWheelMonitor is the parent class for all of the slices within the Item Wheel.
    /// </summary>
    public class ItemWheelMonitor : CharacterMonitor
    {
        [Tooltip("The mapping to the Item Wheel input.")]
        [SerializeField] protected string m_ToggleItemWheel = "Toggle Item Wheel";
        [Tooltip("The category that the ability should respond to.")]
        [SerializeField] protected uint m_CategoryID;
        [Tooltip("Any additional GameObjects which should be toggled when the item wheel is visible")]
        [SerializeField] protected GameObject[] m_ActiveObjects;

        private EquipUnequip m_EquipUnequip;
        private PlayerInput m_PlayerInput;
        private ItemWheelSliceMonitor[] m_WheelSlices;

        private Dictionary<ItemDefinitionBase, int> m_ItemDefinitionByItemIndex = new Dictionary<ItemDefinitionBase, int>();

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_WheelSlices = GetComponentsInChildren<ItemWheelSliceMonitor>(true);

            // Start disabled. OnAttachCharacter will enable the component.
            ToggleVisiblity(false);
        }

        /// <summary>
        /// Attaches the monitor to the specified character.
        /// </summary>
        /// <param name="character">The character to attach the monitor to.</param>
        protected override void OnAttachCharacter(GameObject character)
        {
            base.OnAttachCharacter(character);

            if (m_Character == null) {
                return;
            }

            // Cache the EquipUnequip ability that will be equipping or unequipping the item.
            var characterLocomotion = m_Character.GetCachedComponent<UltimateCharacterLocomotion>();
            var equipUnequipAbilities = characterLocomotion.GetAbilities<EquipUnequip>();
            for (int i = 0; i < equipUnequipAbilities.Length; ++i) {
                if (equipUnequipAbilities[i].ItemSetCategoryID == m_CategoryID) {
                    m_EquipUnequip = equipUnequipAbilities[i];
                    break;
                }
            }
            if (m_EquipUnequip == null) {
                Debug.LogError($"Error: Unable to find the EquipUnequip ability with category ID {m_CategoryID}.");
                return;
            }

            // Keep a mapping between the IItemIdentifier and ItemSetIndex.
            var itemSetManager = m_Character.GetCachedComponent<ItemSetManagerBase>();
            var categoryIndex = itemSetManager.CategoryIDToIndex(m_CategoryID);
            if (categoryIndex == -1) {
                Debug.LogError($"Error: Unable to find the category index with category ID {m_CategoryID}.");
                return;
            }
            var categoryItemSets = itemSetManager.CategoryItemSets[categoryIndex];
            for (int i = 0; i < categoryItemSets.ItemSetList.Count; ++i) {
                var itemSet = categoryItemSets.ItemSetList[i];
                for (int j = 0; j < itemSet.Slots.Length; ++j) {
                    if (itemSet.Slots[j] != null && !m_ItemDefinitionByItemIndex.ContainsKey(itemSet.Slots[j])) {
                        m_ItemDefinitionByItemIndex.Add(itemSet.Slots[j], i);
                    }
                }
            }

            for (int i = 0; i < m_WheelSlices.Length; ++i) {
                m_WheelSlices[i].Initialize(m_Character);
            }

            m_PlayerInput = m_Character.GetCachedComponent<PlayerInput>();
            enabled = true;
        }

        /// <summary>
        /// Respond to changes when the item wheel button is pressed.
        /// </summary>
        private void Update()
        {
            if (m_PlayerInput.GetButtonDown(m_ToggleItemWheel)) {
                ToggleVisiblity(true);
            } else if (m_PlayerInput.GetButtonUp(m_ToggleItemWheel)) {
                ToggleVisiblity(false);
            }
        }

        /// <summary>
        /// A wheel slice has been selected. Equip the selected item and close the wheel.
        /// </summary>
        /// <param name="itemDefinition">The selected item.</param>
        public void ItemSelected(ItemDefinitionBase itemDefinition)
        {
            if (!m_ItemDefinitionByItemIndex.TryGetValue(itemDefinition, out var index)) {
                return;
            }

            m_EquipUnequip.StartEquipUnequip(index);
            ToggleVisiblity(false);
        }

        /// <summary>
        /// Show or hide the item wheel.
        /// </summary>
        /// <param name="visible">Should the wheel be visible?</param>
        private void ToggleVisiblity(bool visible)
        {
            // Let the slices and other objects know that the wheel has been shown. When the wheel is visible regular gameplay input should stop.
            if (m_Character != null) {
                for (int i = 0; i < m_WheelSlices.Length; ++i) {
                    m_WheelSlices[i].ToggleVisiblity(visible);
                }
                EventHandler.ExecuteEvent<bool>(m_Character, "OnEnableGameplayInput", !visible);
            }

            for (int i = 0; i < m_ActiveObjects.Length; ++i) {
                m_ActiveObjects[i].SetActive(visible);
            }

            Cursor.visible = visible;
            Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        /// <summary>
        /// Can the UI be shown?
        /// </summary>
        /// <returns>True if the UI can be shown.</returns>
        protected override bool CanShowUI()
        {
            return base.CanShowUI() && m_EquipUnequip != null;
        }
    }
}