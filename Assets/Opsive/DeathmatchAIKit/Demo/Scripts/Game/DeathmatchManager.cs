/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.Game
{
    using BehaviorDesigner.Runtime;
    using Opsive.DeathmatchAIKit.AI;
    using Opsive.DeathmatchAIKit.Demo.UI;
    using Opsive.Shared.Events;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Camera;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Game;
    using Opsive.UltimateCharacterController.Items.Actions;
    using Opsive.UltimateCharacterController.Objects;
    using Opsive.UltimateCharacterController.Traits;
    using Opsive.UltimateCharacterController.UI;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// The DeathmatchManager sets up the deathmatch game and acts as a coordinator between the various components.
    /// </summary>
    public class DeathmatchManager : MonoBehaviour
    {
        private static DeathmatchManager s_Instance;
        private static DeathmatchManager Instance {
            get {
                if (s_Instance == null) {
                    s_Instance = new GameObject("Deathmatch Manager").AddComponent<DeathmatchManager>();
                }
                return s_Instance;
            }
        }

        private const int c_MaxPlayerCount = 8;
        private const int c_MaxTeamPlayerCount = 4;
        private const int c_MaxTeamCount = 4;

        /// <summary>
        /// Specifies how difficult the AI is to kill.
        /// </summary>
        public enum AIDifficulty {
            Easy,   // AI has reduced health and reduced accuracy, player has increased health.
            Medium, // AI has reduced accuracy, player has increased health.
            Hard    // No changes.
        }

        [Tooltip("Specifies the number of players a FFA deathmatch game.")]
        [SerializeField] protected int m_PlayerCount = 8;
        [Tooltip("Specifies the number of players on a team in the deathmatch game.")]
        [SerializeField] protected int m_PlayersPerTeam = 4;
        [Tooltip("Specifies the number of teams in the deathmatch game.")]
        [SerializeField] protected int m_TeamCount = 2;
        [Tooltip("Should a team deathmath game be started?")]
        [SerializeField] protected bool m_TeamGame;
        [Tooltip("Should the observer mode be started? Player will not spawn but merely observe.")]
        [SerializeField] protected bool m_ObserverMode;
        [Tooltip("Should the character start in a first person perspective?")]
        [SerializeField] protected bool m_FirstPersonPerspective;
        [Tooltip("Specifies how hard the AI agents are to beat.")]
        [SerializeField] protected AIDifficulty m_Difficulty = AIDifficulty.Medium;
        [Tooltip("A reference to the player controlled character.")]
        [SerializeField] protected GameObject m_PlayerPrefab;
        [Tooltip("A reference to the AI controlled character.")]
        [SerializeField] protected GameObject m_AgentPrefab;
        [Tooltip("A reference to the camera.")]
        [SerializeField] protected GameObject m_CameraPrefab;
        [Tooltip("A reference to the observer.")]
        [SerializeField] protected GameObject m_ObserverPrefab;
        [Tooltip("The names to use within in the deathmatch game.")]
        [SerializeField] protected string[] m_AgentNames;
        [Tooltip("The team names to use within in the deathmatch game.")]
        [SerializeField] protected string[] m_TeamNames;
        [Tooltip("The name of the object to parent the players to.")]
        [SerializeField] protected string m_ParentName = "Players";
        [Tooltip("The primary player colors to use when in a FFA game.")]
        [SerializeField] protected Color[] m_PrimaryFFAColors;
        [Tooltip("The secondary player colors to use when in a FFA game.")]
        [SerializeField] protected Color[] m_SecondaryFFAColors;
        [Tooltip("The primary player colors to use for the when in a team game.")]
        [SerializeField] protected Color[] m_PrimaryTeamColors;
        [Tooltip("The secondary player colors to use for the when in a team game.")]
        [SerializeField] protected Color[] m_SecondaryTeamColors;
        [Tooltip("The LayerMask name to use for a FFA and team game.")]
        [SerializeField] protected string[] m_Layers;
        [Tooltip("The behavior tree to use when in a FFA game.")]
        [SerializeField] protected ExternalBehavior m_SoloTree;
        [Tooltip("The behavior tree to use when in a team game.")]
        [SerializeField] protected ExternalBehavior m_TeamTree;
        [Tooltip("The Object Identifier ID of the Waypoints parent Transform.")]
        [SerializeField] protected int m_WaypointParentID = 45894;

        public static bool IsInstantiated { get { return s_Instance != null; } }
        public static int PlayerCount { set { Instance.PlayerCountInternal = value; } get { return Instance.PlayerCountInternal; } }
        private int PlayerCountInternal { set { m_PlayerCount = Mathf.Clamp(value, 2, c_MaxPlayerCount); } get { return m_PlayerCount; } }
        private static int MaxPlayerCount { get { return c_MaxPlayerCount; } }
        public static int PlayersPerTeam { set { Instance.PlayersPerTeamInternal = value; } get { return Instance.PlayersPerTeamInternal; } }
        private int PlayersPerTeamInternal { set { m_PlayersPerTeam = Mathf.Clamp(value, 2, c_MaxTeamPlayerCount); } get { return m_PlayersPerTeam; } }
        private static int MaxTeamPlayerCount { get { return c_MaxTeamPlayerCount; } }
        public static int TeamCount { set { Instance.TeamCountInternal = value; } get { return Instance.TeamCountInternal; } }
        private int TeamCountInternal { set { m_TeamCount = Mathf.Clamp(value, 2, c_MaxTeamCount); } get { return m_TeamCount; } }
        private static int MaxTeamCount { get { return c_MaxTeamCount; } }
        public static string PlayerName { set { Instance.PlayerNameInternal = value; } }
        private string PlayerNameInternal { set { m_PlayerName = value; } }
        public static string[] TeamNames { get { return Instance.TeamNamesInternal; } }
        private string[] TeamNamesInternal { get { return m_TeamNames; } }
        public static bool TeamGame { get { return Instance.TeamGameInternal; } set { Instance.TeamGameInternal = value; } }
        private bool TeamGameInternal { get { return m_TeamGame; } set { m_TeamGame = value; } }
        public static bool ObserverMode { get { return Instance.ObserverModeInternal; } set { Instance.ObserverModeInternal = value; } }
        private bool ObserverModeInternal { get { return m_ObserverMode; } set { m_ObserverMode = value; } }
        public static bool FirstPersonPerspective { get { return Instance.FirstPersonPerspectiveInternal; } set { Instance.FirstPersonPerspectiveInternal = value; } }
        private bool FirstPersonPerspectiveInternal { get { return m_FirstPersonPerspective; } set { m_FirstPersonPerspective = value; } }
        public static AIDifficulty Difficulty { get { return Instance.DifficultyInternal; } set { Instance.DifficultyInternal = value; } }
        private AIDifficulty DifficultyInternal { get { return m_Difficulty; } set { m_Difficulty = value; } }
        public static Color[] PrimaryFFAColors { get { return Instance.PrimaryFFAColorsInternal; } }
        private Color[] PrimaryFFAColorsInternal { get { return m_PrimaryFFAColors; } }
        public static Color[] PrimaryTeamColors { get { return Instance.PrimaryTeamColorsInternal; } }
        private Color[] PrimaryTeamColorsInternal { get { return m_PrimaryTeamColors; } }
        public static bool Paused { set { Instance.PausedInternal = value;  } }
        private bool PausedInternal
        {
            set
            {
                // Don't pause if the game is over.
                if (m_LocalPlayer == null) {
                    return;
                }
                m_Paused = value;
                Time.timeScale = m_Paused ? 0 : 1;
                EventHandler.ExecuteEvent("OnPauseGame", m_Paused);
                EventHandler.ExecuteEvent<bool>(m_LocalPlayer, "OnShowUI", !m_Paused);
                BehaviorManager.instance.enabled = !m_Paused;
                Cursor.visible = m_Paused;
                Cursor.lockState = m_Paused ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }

        private string m_PlayerName = "Aydan";
        private bool m_Paused;
        private bool m_GameOver;

        private Transform m_PlayerParent;
        private CoverPoint[] m_CoverPoints;
        private TeamManager m_TeamManager;
        private GameObject m_LocalPlayer;

        public static CoverPoint[] CoverPoints { get { return Instance.CoverPointsInternal; } }
        private CoverPoint[] CoverPointsInternal { get { return m_CoverPoints; } }
        public static GameObject LocalPlayer { get { return Instance.LocalPlayerInternal; } }
        private GameObject LocalPlayerInternal { get { return m_LocalPlayer; } }

        /// <summary>
        /// Initializes the default values.
        /// </summary>
        private void Awake()
        {
            s_Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += SceneLoaded;

            // Ensure the arrays are the correct size.
            if (m_TeamGame) {
                if (m_PrimaryTeamColors.Length < m_TeamCount || m_SecondaryTeamColors.Length < m_TeamCount) {
                    Debug.LogError("Error: The team count is greater than the number of colors available.");
                }
                if (m_Layers.Length < m_TeamCount) {
                    Debug.LogError("Error: The team count is greater than the number of layers available.");
                }
            } else {
                if (m_PrimaryFFAColors.Length < m_PlayerCount || m_SecondaryFFAColors.Length < m_PlayerCount) {
                    Debug.LogError("Error: The player count is greater than the number of colors available.");
                }
            }

            EventHandler.RegisterEvent<bool>("OnGameOver", GameOver);
        }

        /// <summary>
        /// Pause the game if the escape key is pressed.
        /// </summary>
        private void Update()
        {
            // Pause the game when escape is pressed.
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Paused = true;
            }
        }

        /// <summary>
        /// Callback when a new scene is loaded. 
        /// </summary>
        /// <param name="scene">The scene that was loaded.</param>
        /// <param name="mode">Specifies how the scene was loaded.</param>
        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneLoaded();
        }

        /// <summary>
        /// A new scene was loaded. Spawn the players if the level isn't the main menu.
        /// </summary>
        public void SceneLoaded()
        {
            // No action for the main menu scene.
            if (FindObjectOfType<KinematicObjectManager>() == null) {
                enabled = false;
                return;
            }
            enabled = true;
            m_GameOver = false;

            // Find the parent object of all of the players.
            var parentGameObject = GameObject.Find(m_ParentName);
            if (parentGameObject != null) {
                m_PlayerParent = parentGameObject.transform;
            }

            // Reset the player list.
            Scoreboard.Reinitialize();

            // AI Agents need to know where to take cover.
            m_CoverPoints = GameObject.FindObjectsOfType<CoverPoint>();
            
            // Add the local player/camera. Wait to attach the character until after all of the scripts have been created. If in observer mode then only spawn the
            // observer prefab.
            GameObject camera = null;
            if (m_ObserverMode) {
                m_LocalPlayer = GameObject.Instantiate(m_ObserverPrefab) as GameObject;
            } else {
                m_LocalPlayer = InstantiatePlayer(m_PlayerPrefab, m_PlayerName, 0);
                camera = GameObject.Instantiate(m_CameraPrefab) as GameObject;
            }

            // Spawn the AI agents.
            var startIndex = m_ObserverMode ? 0 : 1; // Start at index 1 in non-observer mode since the player will be on the first team.
            if (m_TeamGame) {
                // Create the TeamManager.
                m_TeamManager = gameObject.AddComponent<TeamManager>();

                // The local player should target players on the other team.
                var enemyLayer = 0;
                for (int i = 1; i < m_TeamCount; ++i) {
                    enemyLayer |= 1 << LayerMask.NameToLayer(m_Layers[i]);
                }
                if (!m_ObserverMode) {
                    // Setup the local player on the first team.
                    SetupPlayer(m_LocalPlayer, 0, 0, LayerMask.NameToLayer(m_Layers[0]), enemyLayer, m_PrimaryTeamColors[0], m_SecondaryTeamColors[0]);
                    TeamManager.AddTeamMember(m_LocalPlayer, 0);

                    // The player should recognize the enemy layers.
                    var characterLayerManager = m_LocalPlayer.GetComponent<CharacterLayerManager>();
                    characterLayerManager.EnemyLayers = enemyLayer;

                    // The player should have increased health.
                    if (m_Difficulty != AIDifficulty.Hard) {
                        var health = m_LocalPlayer.GetComponent<Health>();
                        var attributeManager = m_LocalPlayer.GetComponent<AttributeManager>();
                        var healthAttribute = attributeManager.GetAttribute(health.HealthAttributeName);
                        if (healthAttribute != null) {
                            healthAttribute.Value = healthAttribute.MaxValue = healthAttribute.Value * (m_Difficulty == AIDifficulty.Easy ? 5 : 2);
                        }
                    }
                }

                // Setup the team AI agents.
                for (int i = 0; i < m_TeamCount; ++i) {
                    // The AI agent should target players on the other team.
                    var teamLayer = LayerMask.NameToLayer(m_Layers[i]);
                    enemyLayer = 0;
                    for (int j = 0; j < m_TeamCount; ++j) {
                        // Do not allow friendly fire.
                        if (i == j) {
                            continue;
                        }
                        enemyLayer |= 1 << LayerMask.NameToLayer(m_Layers[j]);
                    }

                    // Setup the AI agents.
                    for (int j = startIndex; j < m_PlayersPerTeam; ++j) {
                        var agentNameIndex = m_ObserverMode ? ((i * m_PlayersPerTeam) + j ) : ((i * m_PlayersPerTeam) + j - 1);
                        var aiAgent = InstantiatePlayer(m_AgentPrefab, m_AgentNames[agentNameIndex], i);
                        SetupPlayer(aiAgent, (i * m_PlayersPerTeam) + j, i, teamLayer, enemyLayer, m_PrimaryTeamColors[i], m_SecondaryTeamColors[i]);

                        TeamManager.AddTeamMember(aiAgent, i);
                    }

                    startIndex = 0;
                }
            } else {
                if (!m_ObserverMode) {
                    // Setup the local player.
                    SetupPlayer(m_LocalPlayer, 0, 0, LayerManager.Character, 1 << LayerManager.Enemy, m_PrimaryFFAColors[0], m_SecondaryTeamColors[0]);
                }
                // Setup the AI agents.
                for (int i = startIndex; i < m_PlayerCount; ++i) {
                    var aiAgent = InstantiatePlayer(m_AgentPrefab, m_AgentNames[i - (m_ObserverMode ? 0 : 1)], i);
                    SetupPlayer(aiAgent, i, i, LayerManager.Enemy, 1 << LayerManager.Character | 1 << LayerManager.Enemy, m_PrimaryFFAColors[i], m_SecondaryFFAColors[i]);
                }
            }

            EventHandler.ExecuteEvent("OnStartGame");
            // The camera will be null in observer mode.
            if (camera != null) {
                var cameraController = camera.GetComponent<CameraController>();
                cameraController.Character = m_LocalPlayer;
#if FIRST_PERSON_CONTROLLER && THIRD_PERSON_CONTROLLER
                cameraController.SetPerspective(m_FirstPersonPerspective, true);
#endif
            }
            // The UI should be made aware of the character assignment.
            var characterMonitors = FindObjectsOfType<CharacterMonitor>();
            for (int i = 0; i < characterMonitors.Length; ++i) {
                if (m_ObserverMode) {
                    if (!(characterMonitors[i] is MiniScoreboardMonitor)) {
                        characterMonitors[i].Visible = false;
                    }
                } else {
                    characterMonitors[i].Character = m_LocalPlayer;
                }
            }
            enabled = true;
            // The characters updates within Update. Limit the update rate.
            Application.targetFrameRate = 60;
        }

        /// <summary>
        /// Instantiates a new deathmatch player.
        /// </summary>
        /// <param name="prefab">The prefab that can spawn.</param>
        /// <param name="name">The name of the player.</param>
        /// <param name"teamIndex">The index of the team that the player is on.</param>
        /// <returns>The instantiated object.</returns>
        private GameObject InstantiatePlayer(GameObject prefab, string name, int teamIndex)
        {
            // Determine a unique spawn point.
            var position = Vector3.zero;
            var rotation = Quaternion.identity;
            SpawnPointManager.GetPlacement(prefab, (m_TeamGame ? teamIndex : -1), ref position, ref rotation);

            // Instantiate the player and notify the scoreboard.
            var player = GameObject.Instantiate(prefab, position, rotation) as GameObject;
            player.transform.parent = m_PlayerParent;
            player.name = name;
            Scoreboard.AddPlayer(player, teamIndex);
            return player;
        }

        /// <summary>
        /// Setup the player for a deathmatch game.
        /// </summary>
        /// <param name="player">The player to set the layers on.</param>
        /// <param name="playerIndex">The index of the player.</param>
        /// <param name="teamIndex">The index of the team.</param>
        /// <param name="friendlyLayer">The friendly layer.</param>
        /// <param name="enemyLayer">The enemy layer.</param>
        /// <param name="primaryColor">The primary color of the player.</param>
        /// <param name="secondaryColor">The secondary color of the player.</param>
        private void SetupPlayer(GameObject player, int playerIndex, int teamIndex, int friendlyLayer, int enemyLayer, Color primaryColor, Color secondaryColor)
        {
            // The 0th index corresponds to the local player while not in observer mode.
            if (m_ObserverMode || playerIndex != 0) {
                var behaviorTree = player.GetComponent<BehaviorTree>();
                behaviorTree.ExternalBehavior = (m_TeamGame ? m_TeamTree : m_SoloTree);
                Transform waypointsParent = null;
                var objectLocations = FindObjectsOfType<ObjectIdentifier>();
                for (int i = 0; i < objectLocations.Length; ++i) {
                    if (objectLocations[i].ID == m_WaypointParentID) {
                        waypointsParent = objectLocations[i].transform;
                        break;
                    }
                }
                if (waypointsParent == null) {
                    Debug.LogError($"Error: Unable to find the Waypoints GameObject. Ensure this object has the Object Location component with ID {m_WaypointParentID}.");
                    return;
                }
                var waypoints = behaviorTree.GetVariable("Waypoints") as SharedGameObjectList;
                for (int i = 0; i < waypointsParent.childCount; ++i) {
                    waypoints.Value.Add(waypointsParent.GetChild(i).gameObject);
                }

                if (teamIndex > 0) {
                    var deathmatchAgent = player.GetCachedComponent<DeathmatchAgent>();
                    // The following adjustments are applied to the enemy agents:
                    // Easy: Reduced health and less accuracy.
                    // Medium: Less accuracy.
                    // Hard: No change.
                    ShootableWeapon[] shootableWeapons;
                    switch (m_Difficulty) {
                        case AIDifficulty.Easy:
                            var health = deathmatchAgent.GetComponent<Health>();
                            var attributeManager = deathmatchAgent.GetComponent<AttributeManager>();
                            var healthAttribute = attributeManager.GetAttribute(health.HealthAttributeName);
                            if (healthAttribute != null) {
                                healthAttribute.Value = healthAttribute.MaxValue = healthAttribute.Value * 0.5f;
                            }
                            shootableWeapons = deathmatchAgent.GetComponentsInChildren<ShootableWeapon>();
                            for (int i = 0; i < shootableWeapons.Length; ++i) {
                                shootableWeapons[i].Spread = 1f;
                            }
                            break;
                        case AIDifficulty.Medium:
                            shootableWeapons = deathmatchAgent.GetComponentsInChildren<ShootableWeapon>();
                            for (int i = 0; i < shootableWeapons.Length; ++i) {
                                shootableWeapons[i].Spread = 0.5f;
                            }
                            break;
                    }
                }
            }

            // Set the material color.
            var renderers = player.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            for (int i = 0; i < renderers.Length; ++i) {
                var materials = renderers[i].materials;
                for (int j = 0; j < materials.Length; ++j) {
                    // Do not compare the material directly because the player may be using an instance material.
                    if (materials[j].name.Contains("Primary")) {
                        materials[j].color = primaryColor;
                    } else if (materials[j].name.Contains("Secondary")) {
                        materials[j].color = secondaryColor;
                    }
                }
            }

            var characterLayerManager = player.GetCachedComponent<CharacterLayerManager>();
            var characterLocomotion = player.GetCachedComponent<UltimateCharacterLocomotion>();

            // Set the layer of the player collider.
            player.layer = friendlyLayer;
            characterLayerManager.CharacterLayer = 1 << friendlyLayer;
            for (int i = 0; i < characterLocomotion.ColliderCount; ++i) {
                characterLocomotion.Colliders[i].gameObject.layer = friendlyLayer;
            }

            // The player should recognize the enemy layers.
            characterLayerManager.EnemyLayers = enemyLayer;
        }

        /// <summary>
        /// The game has ended.
        /// </summary>
        /// <param name="winner">Did the local player win?</param>
        private void GameOver(bool winner)
        {
            if (m_GameOver) {
                return;
            }
            m_GameOver = true;

            if (!m_ObserverMode) {
                EventHandler.ExecuteEvent(m_LocalPlayer, "OnEnableGameplayInput", false);
            }
            m_LocalPlayer = null;
            var behaviorTrees = Object.FindObjectsOfType<BehaviorTree>();
            for (int i = behaviorTrees.Length - 1; i > -1; --i) {
                behaviorTrees[i].DisableBehavior();
            }
            // Stop the characters from respawning after a delay to allow the spawn events to be created.
            Scheduler.Schedule(0.1f, StopRespawns);
        }

        /// <summary>
        /// Stop all of the characters from respawning.
        /// </summary>
        private void StopRespawns()
        {
            var respawners = Object.FindObjectsOfType<CharacterRespawner>();
            for (int i = 0; i < respawners.Length; ++i) {
                respawners[i].CancelRespawn();
            }
        }

        /// <summary>
        /// The game has ended. Load the main menu.
        /// </summary>
        public static void EndGame()
        {
            Instance.EndGameInternal();
        }

        /// <summary>
        /// Internal method called when the game has ended. Load the main menu.
        /// </summary>
        private void EndGameInternal()
        {
            GameOver(false);
            Time.timeScale = 1;
            if (m_TeamManager != null) {
                Destroy(m_TeamManager);
                m_TeamManager = null;
            }

            SceneManager.LoadScene(0);
        }
    }
}