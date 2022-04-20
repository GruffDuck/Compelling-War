/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.UI
{
    using Opsive.DeathmatchAIKit.Demo.Game;
    using Opsive.Shared.Events;
    using Opsive.UltimateCharacterController.UI;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Displays the local player's score as well as the high score.
    /// </summary>
    public class MiniScoreboardMonitor : CharacterMonitor
    {
        /// <summary>
        /// Contains the objects for showing the player score.
        /// </summary>
        [System.Serializable]
        protected class ScoreContainer
        {
            [Tooltip("A reference to the text which shows the player's name.")]
            [SerializeField] protected Text m_Name;
            [Tooltip("A reference to the text which shows the player's score.")]
            [SerializeField] protected Text m_Score;
            [Tooltip("A reference to the background whose color should change.")]
            [SerializeField] protected Image m_Background;

            public Text Name { get { return m_Name; } }
            public Text Score { get { return m_Score; } }
            public Image Background { get { return m_Background; } }
        }

        [Tooltip("References the local player's UI elements.")]
        [SerializeField] protected ScoreContainer m_LocalPlayer;
        [Tooltip("References the leaders player's UI elements.")]
        [SerializeField] protected ScoreContainer m_Leader;

        private Scoreboard m_Scoreboard;

        /// <summary>
        /// Register for any interested events.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            EventHandler.RegisterEvent("OnScoreChange", ScoreChange);
        }

        /// <summary>
        /// Determine the activation state.
        /// </summary>
        protected override void Start()
        {
            base.Start();

            if (DeathmatchManager.ObserverMode) {
                ScoreChange();
                gameObject.SetActive(true);

                // There is no local player in observer mode.
                m_LocalPlayer.Name.transform.parent.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Attaches the monitor to the specified character.
        /// </summary>
        /// <param name="character">The character to attach the monitor to.</param>
        protected override void OnAttachCharacter(GameObject character)
        {
            base.OnAttachCharacter(character);

            if (character == null) {
                // The object may be destroyed when Unity is ending.
                if (this != null) {
                    gameObject.SetActive(false);
                }
                return;
            }

            ScoreChange();
            gameObject.SetActive(true);
        }

        /// <summary>
        /// The score has changed - update the scoreboard UI.
        /// </summary>
        private void ScoreChange()
        {
            if (m_Scoreboard == null) {
                m_Scoreboard = Scoreboard.Instance;
            }

            // Update the score.
            var colors = DeathmatchManager.TeamGame ? DeathmatchManager.PrimaryTeamColors : DeathmatchManager.PrimaryFFAColors;
            // The character will be null in observer mode.
            if (m_Character != null) {
                var localPlayerStats = m_Scoreboard.StatsForPlayer(m_Character);
                m_LocalPlayer.Name.text = localPlayerStats.Name;
                m_LocalPlayer.Score.text = localPlayerStats.Kills.ToString();
                m_LocalPlayer.Background.color = colors[localPlayerStats.TeamIndex];
            }
            m_Leader.Name.text = m_Scoreboard.NonLocalPlayerLeader.Name;
            m_Leader.Score.text = m_Scoreboard.NonLocalPlayerLeader.Kills.ToString();
            m_Leader.Background.color = colors[m_Scoreboard.NonLocalPlayerLeader.TeamIndex];
        }

        /// <summary>
        /// The object has been destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            EventHandler.UnregisterEvent("OnScoreChange", ScoreChange);
        }
    }
}