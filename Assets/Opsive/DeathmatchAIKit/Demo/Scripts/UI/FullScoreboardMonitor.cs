/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.UI
{
    using Opsive.DeathmatchAIKit.Demo.Game;
    using Opsive.Shared.Events;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Displays the score of all of the players.
    /// </summary>
    public class FullScoreboardMonitor : MonoBehaviour
    {
        /// <summary>
        /// Contains the objects for showing the player score.
        /// </summary>
        [System.Serializable]
        protected class ScoreContainer
        {
            [Tooltip("The parent of the player's score row.")]
            [SerializeField] protected GameObject m_Parent;
            [Tooltip("The name of the player.")]
            [SerializeField] protected Text m_Name;
            [Tooltip("The number of player kills.")]
            [SerializeField] protected Text m_Kills;
            [Tooltip("The number of player deaths.")]
            [SerializeField] protected Text m_Deaths;
            [Tooltip("A reference to the background whose color should change.")]
            [SerializeField] protected Image m_Background;

            public GameObject Parent { get { return m_Parent; } }
            public Text Name { get { return m_Name; } }
            public Text Kills { get { return m_Kills; } }
            public Text Deaths { get { return m_Deaths; } }
            public Image Background { get { return m_Background; } }
        }

        /// <summary>
        /// Contains the objects for showing the team score.
        /// </summary>
        [System.Serializable]
        protected class TeamContainer
        {
            [Tooltip("The parent of the team's score row.")]
            [SerializeField] protected GameObject m_Parent;
            [Tooltip("The name of the team.")]
            [SerializeField] protected Text m_Name;
            [Tooltip("The number of team kills.")]
            [SerializeField] protected Text m_Kills;
            [Tooltip("A reference to the background whose color should change.")]
            [SerializeField] protected Image m_Background;

            public GameObject Parent { get { return m_Parent; } }
            public Text Name { get { return m_Name; } }
            public Text Kills { get { return m_Kills; } }
            public Image Background { get { return m_Background; } }
        }
        
        [Tooltip("A reference to the GameObject indicating how to exit the game.")]
        [SerializeField] protected GameObject m_FFAEndGameInstructions;
        [Tooltip("A reference to the GameObject indicating how to exit the game.")]
        [SerializeField] protected GameObject m_TeamEndGameInstructions;
        [Tooltip("A reference to the GameObject which is the parent of the FFA scoreboard objects.")]
        [SerializeField] protected GameObject m_FFAParentScoreboard;
        [Tooltip("A reference to the GameObject which is the parent of the team scoreboard objects.")]
        [SerializeField] protected GameObject m_TeamParentScoreboard;
        [Tooltip("An array which can show all of the FFA player's scores.")]
        [SerializeField] protected ScoreContainer[] m_FFAPlayers;
        [Tooltip("An array which can show all of the team's scores.")]
        [SerializeField] protected TeamContainer[] m_Teams;
        [Tooltip("An array which can show all of the team player's scores.")]
        [SerializeField] protected ScoreContainer[] m_TeamPlayers;

        private Scoreboard m_Scoreboard;

        private bool m_GameOver;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        protected void Awake()
        {
            m_FFAParentScoreboard.SetActive(false);
            m_TeamParentScoreboard.SetActive(false);
            EventHandler.RegisterEvent("OnScoreChange", ScoreChange);
            EventHandler.RegisterEvent<bool>("OnGameOver", GameOver);
        }

        /// <summary>
        /// Enable or disable the text objects based on the number of players in the game.
        /// </summary>
        protected void Start()
        {
            if (DeathmatchManager.TeamGame) {
                // Initialize the team arrays for a new game.
                for (int i = 0; i < m_Teams.Length; ++i) {
                    m_Teams[i].Parent.SetActive(i < DeathmatchManager.TeamCount);
                }

                // Initialize the team player arrays for a new game.
                var playerCount = DeathmatchManager.TeamCount * DeathmatchManager.PlayersPerTeam;
                for (int i = 0; i < (DeathmatchManager.ObserverMode ? m_TeamPlayers.Length - 1 : m_TeamPlayers.Length); ++i) {
                    m_TeamPlayers[i].Parent.SetActive(i < playerCount);
                }
            } else {
                // Initialize the FFA player arrays for a new game.
                for (int i = 0; i < (DeathmatchManager.ObserverMode ? m_FFAPlayers.Length - 1 : m_FFAPlayers.Length) ; ++i) {
                    m_FFAPlayers[i].Parent.SetActive(i < DeathmatchManager.PlayerCount);
                }
            }
            m_GameOver = false;
        }

        /// <summary>
        /// Show or hide the scoreboard.
        /// </summary>
        private void Update()
        {
            var scoreboardParent = DeathmatchManager.TeamGame ? m_TeamParentScoreboard : m_FFAParentScoreboard;
            // Don't allow the scoreboard to be shown if the game is paused.
            if (Time.timeScale == 0) {
                // Hide the scoreboard if visible.
                if (scoreboardParent.activeSelf) {
                    ShowHideScore(false, true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                return;
            }

            if (!m_GameOver) {
                if (!scoreboardParent.activeSelf && Input.GetButtonDown("Scoreboard")) {
                    ShowHideScore(true, false);
                } else if (scoreboardParent.activeSelf && Input.GetButtonDown("Scoreboard")) {
                    ShowHideScore(false, false);
                }
            }

            // Press the space bar to exit the game.
            var endGameInstructions = DeathmatchManager.TeamGame ? m_TeamEndGameInstructions : m_FFAEndGameInstructions;
            if (endGameInstructions.activeInHierarchy && Input.GetButtonDown("End Game")) {
                DeathmatchManager.EndGame();
            }
        }
        
        /// <summary>
        /// The score has changed. Update the scoreboard.
        /// </summary>
        private void ScoreChange()
        {
            // Update the scoreboard if it is visible.
            var scoreboardParent = DeathmatchManager.TeamGame ? m_TeamParentScoreboard : m_FFAParentScoreboard;
            if (scoreboardParent.activeSelf) {
                ShowHideScore(true, false);
            }
        }

        /// <summary>
        /// The score has changed - update the scoreboard UI.
        /// </summary>
        /// <param name="show">Should the scoreboard be shown?</param>
        /// <param name="endScreenVisible">Is the ending screen visible?</param>
        private void ShowHideScore(bool show, bool endScreenVisible)
        {
            if (m_Scoreboard == null) {
                m_Scoreboard = Scoreboard.Instance;
            }

            // Update the score values.
            if (show) {
                var teamStats = m_Scoreboard.SortedStats;
                for (int i = 0; i < teamStats.Count; ++i) {
                    var playerCount = i;
                    // Only team games need to update the team score.
                    if (DeathmatchManager.TeamGame) {
                        m_Teams[i].Name.text = teamStats[i].Name;
                        m_Teams[i].Kills.text = teamStats[i].Kills.ToString();
                        m_Teams[i].Background.color = DeathmatchManager.PrimaryTeamColors[teamStats[i].TeamIndex];
                        playerCount = i * DeathmatchManager.PlayersPerTeam;
                    }

                    // Update the individual player scores. If on a team the PlayerStats will already be sorted.
                    var playerText = DeathmatchManager.TeamGame ? m_TeamPlayers : m_FFAPlayers;
                    var colors = DeathmatchManager.TeamGame ? DeathmatchManager.PrimaryTeamColors : DeathmatchManager.PrimaryFFAColors;
                    var playerStats = teamStats[i].PlayerStats;
                    for (int j = 0; j < playerStats.Count; ++j) {
                        playerText[playerCount + j].Name.text = playerStats[j].Player.name;
                        playerText[playerCount + j].Kills.text = playerStats[j].Kills.ToString();
                        playerText[playerCount + j].Deaths.text = playerStats[j].Deaths.ToString();
                        playerText[playerCount + j].Background.color = colors[teamStats[i].TeamIndex];
                    }
                }
                var endGameInstructions = DeathmatchManager.TeamGame ? m_TeamEndGameInstructions : m_FFAEndGameInstructions;
                endGameInstructions.SetActive(endScreenVisible);
            }

            // Toggle the scoreboard visibility.
            var scoreboardParent = DeathmatchManager.TeamGame ? m_TeamParentScoreboard : m_FFAParentScoreboard;
            if ((show && !scoreboardParent.activeSelf) || (!show && scoreboardParent.activeSelf)) {
                scoreboardParent.SetActive(show);
                if (show || !endScreenVisible) {
                    var cursorLockState = Cursor.lockState;
                    var cursorVisible = Cursor.visible;
                    // When the game is over the player can still move around.
                    if (!endScreenVisible && DeathmatchManager.LocalPlayer != null) {
                        EventHandler.ExecuteEvent(DeathmatchManager.LocalPlayer, "OnEnableGameplayInput", !show);
                    }
                    EventHandler.ExecuteEvent(DeathmatchManager.LocalPlayer, "OnShowUI", !show);
                    // Keep the cursor disabled.
                    Cursor.lockState = cursorLockState;
                    Cursor.visible = cursorVisible;
                }
            }
        }

        /// <summary>
        /// The game is over. Show the scoreboard for the specified duration and then end the game.
        /// </summary>
        /// <param name="winner">Did the local player win?</param>
        private void GameOver(bool winner)
        {
            ShowHideScore(true, true);
            m_GameOver = true;
        }

        /// <summary>
        /// The object has been destroyed.
        /// </summary>
        protected void OnDestroy()
        {
            EventHandler.UnregisterEvent("OnScoreChange", ScoreChange);
            EventHandler.UnregisterEvent<bool>("OnGameOver", GameOver);
        }
    }
}