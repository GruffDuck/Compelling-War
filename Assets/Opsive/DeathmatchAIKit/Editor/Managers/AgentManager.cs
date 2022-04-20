/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Editor.Managers
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Editor;
    using Opsive.DeathmatchAIKit.AI;
    using Opsive.DeathmatchAIKit.Character.Abilities;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Items;
    using Opsive.UltimateCharacterController.Items.Actions;
    using Opsive.UltimateCharacterController.Utility.Builders;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Builds the Deathmatch AI Kit AI Agent.
    /// </summary>
    [OrderedEditorItem("Agent", 2)]
    public class AgentManager : Manager
    {
        private const string c_SoloTreeGUID = "9030953f942ffae458439438ec8a598d";
        private const string c_TeamTreeGUID = "daa8dc40e24911648a5c704b9c898a49";

        [SerializeField] private GameObject m_Agent;
        [SerializeField] private bool m_AddToTeam;

        /// <summary>
        /// Draws the Manager.
        /// </summary>
        public override void OnGUI()
        {
            var canBuild = true;
            m_Agent = EditorGUILayout.ObjectField("Character", m_Agent, typeof(GameObject), true) as GameObject;
            if (m_Agent == null) {
                EditorGUILayout.HelpBox("Select the GameObject which will be used as the agent. This GameObject must be setup as an AI agent with the Ultimate Character Controller's Character Manager.",
                                    MessageType.Error);
                canBuild = false;
            } else if (m_Agent.GetComponent<UltimateCharacterLocomotion>() == null || m_Agent.GetComponent<LocalLookSource>() == null) {
                // The agent must already be setup with the Ultimate Character Controller.
                EditorGUILayout.HelpBox("The agent must be setup as an AI agent with the Ultimate Character Controller's Character Manager.",
                                    MessageType.Error);
                canBuild = false;
            }
            GUI.enabled = canBuild;
            m_AddToTeam = EditorGUILayout.Toggle("Add To Team", m_AddToTeam);

            if (GUILayout.Button("Build")) {
                BuildAgent();
            }
            GUI.enabled = true;
        }

        /// <summary>
        /// Builds the Deathmatch AI Kit agent.
        /// </summary>
        private void BuildAgent()
        {
            // Add the Deathmatch Agent component.
            var deathmatchAgent = m_Agent.AddComponent<DeathmatchAgent>();
            if (m_AddToTeam) {
                deathmatchAgent.AddToTeam = true;
            }

            // The Deathmatch Agent needs to know where to look. If the character is a humanoid then the look transform will be the head.
            var lookTransform = m_Agent.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head);
            if (lookTransform == null) {
                lookTransform = new GameObject("Look Position").transform;
                lookTransform.parent = deathmatchAgent.transform;
                lookTransform.localPosition = new Vector3(0.3f, 1.6f, 0.35f);
            }
            deathmatchAgent.LookTransform = lookTransform;

            // Add any existing items to the Weapon List.
            var items = m_Agent.GetComponentsInChildren<Item>();
            deathmatchAgent.AvailableWeapons = new DeathmatchAgent.WeaponStat[items.Length];
            for (int i = 0; i < items.Length; ++i) {
                var weaponStat = new DeathmatchAgent.WeaponStat();
                weaponStat.ItemDefinition = items[i].ItemDefinition;
#if ULTIMATE_CHARACTER_CONTROLLER_MELEE
                if (typeof(MeleeWeapon).IsAssignableFrom(items[i].GetType())) {
                    weaponStat.Class = DeathmatchAgent.WeaponStat.WeaponClass.Melee;
                } else
#endif
                if (typeof(ThrowableItem).IsAssignableFrom(items[i].GetType())) {
                    weaponStat.Class = DeathmatchAgent.WeaponStat.WeaponClass.Grenade;
                }
                deathmatchAgent.AvailableWeapons[i] = weaponStat;
            }

            // Add the Cover ability.
            var characterLocomotion = m_Agent.GetComponent<UltimateCharacterLocomotion>();
            characterLocomotion.DeserializeAbilities();
            if (characterLocomotion.GetAbility<Cover>() == null) {
                AbilityBuilder.AddAbility(characterLocomotion, typeof(Cover));
            }

            // Add the behavior tree component.
            var behaviorTree = m_Agent.AddComponent<BehaviorTree>();
            var assetPath = AssetDatabase.GUIDToAssetPath(m_AddToTeam ? c_TeamTreeGUID : c_SoloTreeGUID);
            if (string.IsNullOrEmpty(assetPath)) {
                Debug.LogError("Warning: Unable to find the solo or team behavior tree. Ensure the behavior tree has been imported.");
                return;
            } else {
                behaviorTree.ExternalBehavior = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ExternalBehaviorTree)) as ExternalBehaviorTree;
            }
            var coverVariable = behaviorTree.GetVariable("Cover") as SharedCoverPoint;
            coverVariable.PropertyMapping = "Opsive.DeathmatchAIKit.AI.DeathmatchAgent/CoverPoint";
            coverVariable.PropertyMappingOwner = m_Agent;
            // Save the cover point variable mapping.
            if (BehaviorDesignerPreferences.GetBool(BDPreferences.BinarySerialization)) {
                BinarySerialization.Save(behaviorTree.GetBehaviorSource());
            } else {
                JSONSerialization.Save(behaviorTree.GetBehaviorSource());
            }

            // The animator should always update to keep track of the agent's position.
            var animator = m_Agent.GetComponent<Animator>();
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;

            Selection.activeGameObject = m_Agent;
        }
    }
}