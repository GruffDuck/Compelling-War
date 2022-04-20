/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Editor
{
    using Opsive.Shared.Editor.Inspectors;
    using UnityEngine;
    using UnityEditor;

    /// <summary>
    /// Shows a custom inspector for CoverPoint.
    /// </summary>
    [CustomEditor(typeof(CoverPoint))]
    public class CoverPointInspector : InspectorBase
    {
        /// <summary>
        /// Draws the custom inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            var coverPoint = target as CoverPoint;
            if (coverPoint == null || serializedObject == null)
                return; // How'd this happen?

            base.OnInspectorGUI();

            // Show all of the fields.
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            if (Application.isPlaying) {
                GUI.enabled = false;
                EditorGUILayout.ObjectField("Occpant", coverPoint.Occupant, typeof(GameObject), true);
                GUI.enabled = true;
            }
            EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_LinkedCoverPoints"), true);
            EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_AttackOffset"));
            EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_MaxDistance"));
            EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_MinTargetDistance"));
            EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_MaxTargetDistance"));
            EditorGUILayout.PropertyField(PropertyFromName(serializedObject, "m_LookThreshold"));

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(coverPoint, "Inspector");
                serializedObject.ApplyModifiedProperties();
                Shared.Editor.Utility.EditorUtility.SetDirty(coverPoint);
            }
        }
    }
}