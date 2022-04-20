/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.UI
{
    using Opsive.Shared.Game;
    using Opsive.Shared.Inventory;
    using Opsive.UltimateCharacterController.Inventory;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The ItemWheelSliceMonitor will keep each slice of the ItemWheel's UI up in sync with the rest of the game. This includes selecting the currently active Item
    /// and disabling any slices that correspond to an Item that the character does not have in their inventory.
    /// </summary>
    public class ItemWheelSliceMonitor : MonoBehaviour
    {
        [Tooltip("The ItemDefinition that this slice is monitoring.")]
        [SerializeField] protected ItemDefinitionBase m_ItemDefinition;
        [Tooltip("The color of the image when the ItemDefinition is active.")]
        [SerializeField] protected Color m_SelectedColor;
        [Tooltip("The color of the image when the ItemDefinition is not active.")]
        [SerializeField] protected Color m_NotSelectedColor = Color.white;
        [Tooltip("The color of the image when the ItemDefinition is not in the character's inventory.")]
        [SerializeField] protected Color m_DisabledColor;

        private Image[] m_Images;
        private ItemWheelMonitor m_ItemWheelMonitor;
        private IItemIdentifier m_ItemIdentifier;

        private InventoryBase m_Inventory;

        /// <summary>
        /// Initializes the wheel slice.
        /// </summary>
        /// <param name="character"></param>
        public void Initialize(GameObject character)
        {
            if (m_Images == null) {
                m_Images = GetComponentsInChildren<Image>();
                m_ItemWheelMonitor = transform.GetComponentInParent<ItemWheelMonitor>();
            }

            m_Inventory = character.GetCachedComponent<InventoryBase>();
        }

        /// <summary>
        /// Show or hide the item slice.
        /// </summary>
        /// <param name="visible">Should the item slice be shown?</param>
        public void ToggleVisiblity(bool visible)
        {
            // Set the correct color if the slice is visible.
            if (visible) {
                // The ItemIdentifier needs to be populated.
                if (m_ItemIdentifier == null) {
                    var allItemIdentifiers = m_Inventory.GetAllItemIdentifiers();
                    for (int i = 0; i < allItemIdentifiers.Count; ++i) {
                        if (allItemIdentifiers[i].GetItemDefinition() == m_ItemDefinition) {
                            m_ItemIdentifier = allItemIdentifiers[i];
                            break;
                        }
                    }
                }

                Color color;
                if (m_ItemIdentifier == null) {
                    color = m_DisabledColor;
                } else {
                    // Determine if the item is active by looping through the inventory.
                    var isActive = false;
                    for (int i = 0; i < m_Inventory.SlotCount; ++i) {
                        var item = m_Inventory.GetActiveItem(i);
                        if (item != null && m_ItemIdentifier == item.ItemIdentifier) {
                            isActive = true;
                            break;
                        }
                    }
                    color = isActive ? m_SelectedColor : m_NotSelectedColor;
                }

                // The images should have the same color.
                for (int i = 0; i < m_Images.Length; ++i) {
                    m_Images[i].color = color;
                }
            }
        }

        /// <summary>
        /// This item has been selected. Let the parent wheel monitor know of the change.
        /// </summary>
        public void ItemSelected()
        {
            m_ItemWheelMonitor.ItemSelected(m_ItemDefinition);
        }
    }
}