using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using BehaviorDesigner.Editor;
using Opsive.DeathmatchAIKit.AI;

namespace Opsive.DeathmatchAIKit.Editor.AI
{
    [CustomObjectDrawer(typeof(HashSet<GameObject>))]
    public class GameObjectSetDrawer : ObjectDrawer
    {
        public override void OnGUI(GUIContent label)
        {
            var gameObjectSet = value as HashSet<GameObject>;
            EditorGUILayout.BeginVertical();
            if (FieldInspector.DrawFoldout(gameObjectSet.GetHashCode(), label)) {
                EditorGUI.indentLevel++;
                if (gameObjectSet.Count == 0) {
                    EditorGUILayout.LabelField("No objects in set.");
                } else {
                    foreach (var item in gameObjectSet) {
                        EditorGUILayout.LabelField(item.name);
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}