/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Editor.Managers
{
    using Opsive.Shared.Editor.Inspectors.Utility;
    using System;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Shows a starting window with useful links.
    /// </summary>
    [OrderedEditorItem("Welcome", 0)]
    public class WelcomeScreenManager : Manager
    {
        private const string c_DocumentationTextureGUID = "1aa9ee22dfabcd4448e35c993f4ba7ce";
        private const string c_VideosTextureGUID = "4de72846f26c9274388320291bf120e5";
        private const string c_ForumTextureGUID = "f5a282a6a970c23439541cb1ea1377d0";
        private const string c_DiscordTextureGUID = "fb41e416d5b45f24f906d86731326990";
        private const string c_ReviewTextureGUID = "238064bc653107444a5aaac9a3383112";

        private Texture2D m_DocumentationTexture;
        private Texture2D m_VideosTexture;
        private Texture2D m_ForumTexture;
        private Texture2D m_DiscordTexture;
        private Texture2D m_ReviewTexture;

        /// <summary>
        /// Initialize the manager after deserialization.
        /// </summary>
        public override void Initialize(MainManagerWindow mainManagerWindow)
        {
            base.Initialize(mainManagerWindow);

            m_DocumentationTexture = FindTexture(c_DocumentationTextureGUID);
            m_VideosTexture = FindTexture(c_VideosTextureGUID);
            m_ForumTexture = FindTexture(c_ForumTextureGUID);
            m_DiscordTexture = FindTexture(c_DiscordTextureGUID);
            m_ReviewTexture = FindTexture(c_ReviewTextureGUID);
        }

        /// <summary>
        /// Draws the Manager.
        /// </summary>
        public override void OnGUI()
        {
            EditorGUILayout.LabelField("Thank you for purchasing the Deathmatch AI Kit.\nThe resources below will help you get the most out of the kit.", InspectorStyles.WordWrapLabelCenter);
            GUILayout.Space(3);

            // Check for compatibility.
            var errorMessage = string.Empty;
#if FIRST_PERSON_CONTROLLER
#if !FIRST_PERSON_MELEE
            errorMessage = "Error: Unable to find the first person melee weapons. This configuration is not supported.\nUpgrade to the First Person Controller for full compatibility.";
#endif
#if !FIRST_PERSON_SHOOTER
            if (string.IsNullOrEmpty(errorMessage)) {
                errorMessage = "Error: Unable to find the first person shooter weapons. This configuration is not supported.\nUpgrade to the First Person Controller for full compatibility.";
            }
#endif
#endif
#if !ULTIMATE_CHARACTER_CONTROLLER_SHOOTER
            if (string.IsNullOrEmpty(errorMessage)) {
                errorMessage = "Error: Unable to find the shooter weapons. This configuration is not supported.\nUpgrade to the First Person Controller, Third Person Controller, or Ultimate Character Controller for full compatibility.";
            }
#endif
#if !ULTIMATE_CHARACTER_CONTROLLER_MELEE
            if (string.IsNullOrEmpty(errorMessage)) {
                errorMessage = "Error: Unable to find the melee weapons. This configuration is not supported.\nUpgrade to the First Person Controller, Third Person Controller, or Ultimate Character Controller for full compatibility.";
            }
#endif
            if (!string.IsNullOrEmpty(errorMessage)) {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                GUILayout.Space(3);
            }

            // Draw the header image.
            GUILayout.BeginHorizontal();
            var width = m_MainManagerWindow.position.width - m_MainManagerWindow.MenuWidth - m_DocumentationTexture.width;
            GUILayout.Space(width / 2);
            GUILayout.Label(m_DocumentationTexture, InspectorStyles.CenterLabel, GUILayout.Width(m_DocumentationTexture.width), GUILayout.Height(m_DocumentationTexture.height));
            var lastRect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.MouseUp && lastRect.Contains(Event.current.mousePosition)) {
                Application.OpenURL("https://opsive.com/documentation/deathmatch-ai-kit/");
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(3);

            // The remaining images should be drawn in a grid.
            GUILayout.BeginHorizontal();
            GUILayout.Space(width / 2 + 2);
            var selected = GUILayout.SelectionGrid(-1,
                new Texture2D[] { m_VideosTexture, m_ForumTexture, m_DiscordTexture, m_ReviewTexture},
                2,
                InspectorStyles.CenterLabel, GUILayout.Width(m_VideosTexture.width * 2));
            if (selected != -1) {
                switch (selected) {
                    case 0:
                        Application.OpenURL("https://opsive.com/videos/?pid=1095");
                        break;
                    case 1:
                        Application.OpenURL("https://opsive.com/forum");
                        break;
                    case 2:
                        Application.OpenURL("https://discord.gg/QX6VFgc");
                        break;
                    case 3:
                        Application.OpenURL("https://assetstore.unity.com/packages/slug/184000");
                        break;
                }
            }
            GUILayout.EndHorizontal();

            // Draw the version at the bottom of the window.
            lastRect = GUILayoutUtility.GetLastRect();
            var offset = 368;
#if UNITY_2019_3_OR_NEWER
            offset += 5;
#endif
            GUILayout.Space(m_MainManagerWindow.position.height - lastRect.yMax - offset);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Format("Deathmatch AI Kit version {0}", AssetInfo.Version));
            try {
                var version = new Version(AssetInfo.Version);
                if (!string.IsNullOrEmpty(m_MainManagerWindow.LatestVersion) && version.CompareTo(new Version(m_MainManagerWindow.LatestVersion)) < 0) {
                    EditorGUILayout.LabelField(string.Format(" New version available: {0}", m_MainManagerWindow.LatestVersion));
                }
            } catch (Exception /*e*/) { }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Finds the texture based on the GUID.
        /// </summary>
        /// <param name="guid">The GUID to find the texture with.</param>
        /// <returns>The texture with the specified GUID.</returns>
        private Texture2D FindTexture(string guid)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(assetPath)) {
                return AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D)) as Texture2D;
            }
            return null;
        }
    }
}