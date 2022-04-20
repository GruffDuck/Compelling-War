/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Editor.Managers
{
    using Shared.Editor.Inspectors.Input;
    using Opsive.DeathmatchAIKit.Demo;
    using Opsive.UltimateCharacterController.Editor.Managers;
    using Opsive.UltimateCharacterController.Objects;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Sets up the project and scene for use with the Deathmatch AI Kit demo scene.
    /// </summary>
    [OrderedEditorItem("Setup", 1)]
    public class SetupManager : Manager
    {
        /// <summary>
        /// Draws the Manager.
        /// </summary>
        public override void OnGUI()
        {
            ManagerUtility.DrawControlBox("Project Setup", null, "Sets up the project layers and inputs to be used with the Deathmatch AI Kit demo scene. See the documentation for details.", 
                                            true, "Setup Project", SetupProject, "The deathmatch project has been setup.");
            ManagerUtility.DrawControlBox("Scene Setup", null, "Adds the deathmatch components and waypoints to the scene.",
                                            true, "Setup Scene", SetupDeathmatchScene, string.Empty);
        }

        /// <summary>
        /// Updates the Deathmatch AI Kit project.
        /// </summary>
        private void SetupProject()
        {
            // Ensure the Ultimate Character Controller project has been seutp.
            UltimateCharacterController.Editor.Utility.CharacterInputBuilder.UpdateInputManager();
            UltimateCharacterController.Editor.Managers.SetupManager.UpdateLayers();

            // Setup the input.
            var inputManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            var axesProperty = inputManager.FindProperty("m_Axes");
            UnityInputBuilder.AddInputAxis(axesProperty, "Scoreboard", "", "u", "", "", 1000, 0.001f, 1000, false, false, UnityInputBuilder.AxisType.KeyMouseButton, UnityInputBuilder.AxisNumber.X);
            UnityInputBuilder.AddInputAxis(axesProperty, "Toggle Item Wheel", "", "tab", "", "", 1000, 0.001f, 1000, false, false, UnityInputBuilder.AxisType.KeyMouseButton, UnityInputBuilder.AxisNumber.X);
            UnityInputBuilder.AddInputAxis(axesProperty, "End Game", "", "space", "", "", 1000, 0.001f, 1000, false, false, UnityInputBuilder.AxisType.KeyMouseButton, UnityInputBuilder.AxisNumber.X);
            inputManager.ApplyModifiedProperties();

            // Setup the layers.
            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var layersProperty = tagManager.FindProperty("layers");
            UltimateCharacterController.Editor.Managers.SetupManager.AddLayer(layersProperty, 8, "BlueTeam");
            UltimateCharacterController.Editor.Managers.SetupManager.AddLayer(layersProperty, 9, "GreenTeam");
            UltimateCharacterController.Editor.Managers.SetupManager.AddLayer(layersProperty, 10, "BlueTeam");
            UltimateCharacterController.Editor.Managers.SetupManager.AddLayer(layersProperty, 11, "YellowTeam");
            UltimateCharacterController.Editor.Managers.SetupManager.AddLayer(layersProperty, 12, "Ragdoll");
            UltimateCharacterController.Editor.Managers.SetupManager.AddLayer(layersProperty, 13, "Explosive");
            tagManager.ApplyModifiedProperties();
        }

        /// <summary>
        /// Adds the deathmatch startup GameObject.
        /// </summary>
        private void SetupDeathmatchScene()
        {
            var startup = GameObject.FindObjectOfType<Startup>();
            if (startup != null) {
                return;
            }

            UltimateCharacterController.Editor.Managers.SetupManager.AddManagers();
            
            new GameObject("Startup").AddComponent<Startup>();
            var waypointParent = new GameObject("Waypoints");
            var objectIdentifier = waypointParent.AddComponent<ObjectIdentifier>();
            objectIdentifier.ID = 45894; // Magic number from DeathmatchManager.cs.
            for (int i = 0; i < 3; ++i) {
                var waypoint = new GameObject("Waypoint " + i);
                waypoint.transform.position = new Vector3(Random.value * 20 * (Random.value < 0.5f ? -1 : 1), 0, Random.value * 20 * (Random.value < 0.5f ? -1 : 1));
                waypoint.transform.parent = waypointParent.transform;
                waypoint.AddComponent<UltimateCharacterController.Game.SpawnPoint>();
            }
            EditorUtility.SetDirty(objectIdentifier);

            Debug.Log("New waypoints have been added. They can be repositioned under the Waypoints GameObject.");
        }
    }
}