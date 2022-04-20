namespace Opsive.DeathmatchAIKit.Demo.Editor.Inspectors.UI
{
    using Opsive.DeathmatchAIKit.Demo.UI;
    using Opsive.UltimateCharacterController.Editor.Managers;
    using Opsive.UltimateCharacterController.Inventory;
    using Opsive.Shared.Editor.Inspectors.StateSystem;
    using System;
    using UnityEngine;
    using UnityEditor;

    /// <summary>
    /// Shows a custom inspector for the ItemWheelMonitor component.
    /// </summary>
    [CustomEditor(typeof(ItemWheelMonitor))]
    public class ItemWheelMonitorInspector : StateBehaviorInspector
    {
        private ItemCollection m_ItemCollection;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            m_ItemCollection = ManagerUtility.FindItemCollection(this);
        }

        /// <summary>
        /// Returns the actions to draw before the State list is drawn.
        /// </summary>
        /// <returns>The actions to draw before the State list is drawn.</returns>
        protected override Action GetDrawCallback()
        {
            var baseCallback = base.GetDrawCallback();

            baseCallback += () =>
            {
                EditorGUILayout.PropertyField(PropertyFromName("m_Character"));
                EditorGUILayout.PropertyField(PropertyFromName("m_Visible"));
                EditorGUILayout.PropertyField(PropertyFromName("m_ToggleItemWheel"));
                DrawCategories();
                EditorGUILayout.PropertyField(PropertyFromName("m_ActiveObjects"), true);
            };

            return baseCallback;
        }

        /// <summary>
        /// Draws all of the ItemSet categories within a dropdown.
        /// </summary>
        private void DrawCategories()
        {
            m_ItemCollection = EditorGUILayout.ObjectField("Item Collection", m_ItemCollection, typeof(ItemCollection), false) as ItemCollection;

            var categoryIDProperty = PropertyFromName("m_CategoryID");
            var categoryNames = new string[((m_ItemCollection != null && m_ItemCollection.Categories != null) ? m_ItemCollection.Categories.Length : 0) + 1];
            categoryNames[0] = "(Not Specified)";
            var selected = 0;
            if (categoryNames.Length > 1 && GUI.enabled) {
                for (int i = 0; i < m_ItemCollection.Categories.Length; ++i) {
                    categoryNames[i + 1] = m_ItemCollection.Categories[i].name;
                    if (categoryIDProperty.intValue == m_ItemCollection.Categories[i].ID || (categoryIDProperty.intValue == m_ItemCollection.Categories[i].ID - int.MaxValue)) {
                        selected = i + 1;
                    }
                }
            }

            var newSelected = EditorGUILayout.Popup("Category", selected != -1 ? selected : 0, categoryNames);
            if (selected != newSelected) {
                if (newSelected == 0) {
                    categoryIDProperty.intValue = 0;
                } else {
                    categoryIDProperty.intValue = (int)m_ItemCollection.Categories[newSelected - 1].ID;
                    Debug.Log("Set " + m_ItemCollection.Categories[newSelected - 1].name + "  "+ m_ItemCollection.Categories[newSelected - 1].ID);
                }
                GUI.changed = true;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}