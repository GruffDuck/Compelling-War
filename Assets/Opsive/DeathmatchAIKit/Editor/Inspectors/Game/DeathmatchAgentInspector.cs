/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Editor
{
    using Opsive.DeathmatchAIKit.AI;
    using Opsive.Shared.Inventory;
    using Opsive.Shared.Editor.Inspectors.StateSystem;
    using System;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;

    /// <summary>
    /// Shows a custom inspector for DeathmatchAgent.
    /// </summary>
    [CustomEditor(typeof(DeathmatchAgent))]
    public class DeathmatchAgentInspector : StateBehaviorInspector
    {
        private DeathmatchAgent m_DeathmatchAgent;
        private SerializedProperty m_AvailableWeapons;
        private ReorderableList m_ReorderableWeaponList;

        /// <summary>
        /// Initializes the inspector.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            m_DeathmatchAgent = target as DeathmatchAgent;
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
                if (Application.isPlaying) {
                    GUI.enabled = false;
                    EditorGUILayout.ObjectField("Cover Point", m_DeathmatchAgent.CoverPoint, typeof(GameObject), true);
                    GUI.enabled = true;
                }
                EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_PauseOnDeath"));
                var addToTeamProperty = PropertyFromName(serializedObject, "m_AddToTeam");
                EditorGUILayout.PropertyField(addToTeamProperty);
                if (addToTeamProperty.boolValue) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_TeamIndex"));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_MaxColliderCount"));
                EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_LookTransform"));
                EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_TargetBone"));
                EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_DistanceScoreWeight"));
                EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_AngleScoreWeight"));

                if (Foldout("Weapon Stats")) {
                    EditorGUI.indentLevel++;
                    if (m_ReorderableWeaponList == null) {
                        m_AvailableWeapons = PropertyFromName(serializedObject, "m_AvailableWeapons");
                        m_ReorderableWeaponList = new ReorderableList(serializedObject, m_AvailableWeapons, false, true, true, true);
                        m_ReorderableWeaponList.drawHeaderCallback = (Rect rect) =>
                        {
                            EditorGUI.LabelField(rect, "Available Weapons");
                        };
                        m_ReorderableWeaponList.drawElementCallback = OnAvailableWeaponListDraw;
                        m_ReorderableWeaponList.onAddCallback = OnAvailableWeaponListAdd;
                        m_ReorderableWeaponList.onRemoveCallback = OnAvailableWeaponListRemove;
                        m_ReorderableWeaponList.onSelectCallback = (ReorderableList list) =>
                        {
                            m_DeathmatchAgent.SelectedAvailableWeapon = list.index;
                        };
                        if (m_DeathmatchAgent.SelectedAvailableWeapon != -1) {
                            m_ReorderableWeaponList.index = m_DeathmatchAgent.SelectedAvailableWeapon;
                        }
                    }
                    m_ReorderableWeaponList.DoLayoutList();

                    // Draw the selected weapon.
                    if (m_ReorderableWeaponList.index != -1) {
                        if (m_ReorderableWeaponList.index < m_AvailableWeapons.arraySize) {
                            GUILayout.Space(4);
                            var weaponStatProperty = m_AvailableWeapons.GetArrayElementAtIndex(m_ReorderableWeaponList.index);
                            var itemDefinitionProperty = weaponStatProperty.FindPropertyRelative("m_ItemDefinition");
                            var name = "(none)";
                            if (itemDefinitionProperty.objectReferenceValue is ItemDefinitionBase) {
                                name = (itemDefinitionProperty.objectReferenceValue as ItemDefinitionBase).name;
                            }
                            EditorGUILayout.LabelField(name, Shared.Editor.Inspectors.Utility.InspectorStyles.BoldLabel);
                            EditorGUILayout.PropertyField(itemDefinitionProperty);
                            EditorGUILayout.PropertyField(weaponStatProperty.FindPropertyRelative("m_Class"));
                            EditorGUILayout.PropertyField(weaponStatProperty.FindPropertyRelative("m_UseLikelihood"));
                            EditorGUILayout.PropertyField(weaponStatProperty.FindPropertyRelative("m_MinUseDistance"));
                            EditorGUILayout.PropertyField(weaponStatProperty.FindPropertyRelative("m_MaxUseDistance"));
                            EditorGUILayout.PropertyField(weaponStatProperty.FindPropertyRelative("m_GroupDamage"));
                        } else {
                            m_ReorderableWeaponList.index = m_DeathmatchAgent.SelectedAvailableWeapon = -1;
                        }
                    }
                    EditorGUI.indentLevel--;
                }
            };
            return baseCallback;
        }

        /// <summary>
        /// Draws all of the added available weapons.
        /// </summary>
        private void OnAvailableWeaponListDraw(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.BeginChangeCheck();
            var weaponStatProperty = m_AvailableWeapons.GetArrayElementAtIndex(index);
            var itemDefinitionProperty = weaponStatProperty.FindPropertyRelative("m_ItemDefinition");
            var name = "(none)";
            if (itemDefinitionProperty.objectReferenceValue is ItemDefinitionBase) {
                name = (itemDefinitionProperty.objectReferenceValue as ItemDefinitionBase).name;
            }
            EditorGUI.LabelField(new Rect(rect.x, rect.y + 1, rect.width, EditorGUIUtility.singleLineHeight), name);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(m_DeathmatchAgent, "Inspector");
                serializedObject.ApplyModifiedProperties();
                Shared.Editor.Utility.EditorUtility.SetDirty(m_DeathmatchAgent);
            }
        }

        /// <summary>
        /// The ReordableList add button has been pressed. Add a new weapon.
        /// </summary>
        private void OnAvailableWeaponListAdd(ReorderableList list)
        {
            m_AvailableWeapons.InsertArrayElementAtIndex(m_AvailableWeapons.arraySize);
            list.index = m_DeathmatchAgent.SelectedAvailableWeapon = m_AvailableWeapons.arraySize - 1;
            m_AvailableWeapons.serializedObject.ApplyModifiedProperties();
            Shared.Editor.Utility.EditorUtility.SetDirty(m_DeathmatchAgent);
        }

        /// <summary>
        /// The ReordableList remove button has been pressed. Remove the selected weapon.
        /// </summary>
        private void OnAvailableWeaponListRemove(ReorderableList list)
        {
            m_AvailableWeapons.DeleteArrayElementAtIndex(list.index);
            list.index = list.index - 1;
            if (list.index == -1 && m_AvailableWeapons.arraySize > 0) {
                list.index = 0;
            }
            m_DeathmatchAgent.SelectedAvailableWeapon = list.index;
            Shared.Editor.Utility.EditorUtility.SetDirty(m_DeathmatchAgent);
        }
    }
}