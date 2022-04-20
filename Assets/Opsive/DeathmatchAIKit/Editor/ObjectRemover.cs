/// ---------------------------------------------
/// Ultimate Character Controller
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

#if !ULTIMATE_CHARACTER_CONTROLLER_EXTENSION_DEBUG

namespace Opsive.DeathmatchAIKit.Editor
{
    using Opsive.UltimateCharacterController.Demo.References;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEditor;

    /// <summary>
    /// An ObjectRemover created specifically for the Deathmatch AI Kit.
    /// </summary>
    [InitializeOnLoad]
    public class ObjectRemover
    {
        private static Scene s_ActiveScene;

        /// <summary>
        /// Registers for the scene change callback.
        /// </summary>
        static ObjectRemover()
        {
            EditorApplication.update += Update;
        }

        /// <summary>
        /// The scene has been changed.
        /// </summary>
        private static void Update()
        {
            var scene = SceneManager.GetActiveScene();

            if (scene == s_ActiveScene || Application.isPlaying) {
                return;
            }
            s_ActiveScene = scene;

            // Only the spark scene is be affected.
            var scenePath = s_ActiveScene.path.Replace("\\", "/");
            if (!scenePath.Contains("Spark")) {
                return;
            }

            var objectReferences = GameObject.FindObjectOfType<ObjectReferences>();
            if (objectReferences == null) {
                return;
            }

            var objectRemover = typeof(Opsive.UltimateCharacterController.Editor.References.ObjectRemover);
            var processObjectReferencesMethod = objectRemover.GetMethod("ProcessObjectReferences", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            processObjectReferencesMethod.Invoke(null, new object[] { objectReferences, true });
        }
    }
}

#endif