/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.UI
{
    using Opsive.DeathmatchAIKit.Demo.Game;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The GameSettings will manage the UI elements for the deathmatch game settings.
    /// </summary>
    public class GameSettings : MonoBehaviour
    {
        /// <summary>
        /// Control which contains the text field representing the count.
        /// </summary>
        [System.Serializable]
        public class AdjustableCountControl
        {
            [Tooltip("A reference to the count text.")]
            [SerializeField] protected Text m_Text;

            private GameObject m_Parent;

            /// <summary>
            /// Activates or deactives the control.
            /// </summary>
            /// <param name="active">Should the control be activated?</param>
            public void SetActive(bool active)
            {
                if (m_Parent == null) {
                    m_Parent = m_Text.transform.parent.parent.gameObject;
                }
                m_Parent.SetActive(active);
            }

            /// <summary>
            /// Sets the count of the text.
            /// </summary>
            /// <param name="count">The count value that should be set.</param>
            public void SetCount(int count)
            {
                m_Text.text = count.ToString();
            }
        }

        [Header("Header")]
        [Tooltip("A reference to the field that allows the player name to be changed.")]
        [SerializeField] protected InputField m_NameField;
        [Tooltip("A reference to the toggle that determines if the player is observing.")]
        [SerializeField] protected Toggle m_ObserveToggle;
        [Tooltip("A reference to the toggle that determines the starting perspective.")]
        [SerializeField] protected Toggle m_PerspectiveToggle;

        [Header("Buttons")]
        [Tooltip("The image sprite when the button is selected.")]
        [SerializeField] protected Sprite m_SelectedSprite;
        [Tooltip("The color of the sprite when the button is selected")]
        [SerializeField] protected Color m_SelectedColor;
        [Tooltip("The image sprite when the button is deselected.")]
        [SerializeField] protected Sprite m_DeselectedSprite;
        [Tooltip("The color of the sprite when the button is deselected")]
        [SerializeField] protected Color m_DeselectedColor;
        [Tooltip("The array of the difficulty levels.")]
        [SerializeField] protected Image[] m_DifficultyImages;
        [Tooltip("The array of the game modes.")]
        [SerializeField] protected Image[] m_ModeImages;

        [Header("Counts")]
        [Tooltip("A reference to the player count control.")]
        [SerializeField] protected AdjustableCountControl m_PlayerCount;
        [Tooltip("A reference to the team count control.")]
        [SerializeField] protected AdjustableCountControl m_TeamCount;
        [Tooltip("A reference to the number of players per team count control.")]
        [SerializeField] protected AdjustableCountControl m_PlayersPerTeam;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        private void Start()
        {
            for (int i = 0; i < m_DifficultyImages.Length; ++i) {
                m_DifficultyImages[i].sprite = m_DeselectedSprite;
            }
            for (int i = 0; i < m_ModeImages.Length; ++i) {
                m_ModeImages[i].sprite = m_DeselectedSprite;
            }

            m_ObserveToggle.isOn = DeathmatchManager.ObserverMode;
#if FIRST_PERSON_CONTROLLER && THIRD_PERSON_CONTROLLER
            m_PerspectiveToggle.isOn = DeathmatchManager.FirstPersonPerspective;
#else
            m_PerspectiveToggle.gameObject.SetActive(false);
#endif
            SelectDifficulty((int)DeathmatchManager.Difficulty);
            SelectTeamGame(DeathmatchManager.TeamGame ? 1 : 0);
            m_PlayerCount.SetCount(DeathmatchManager.PlayerCount);
            m_TeamCount.SetCount(DeathmatchManager.TeamCount);
            m_PlayersPerTeam.SetCount(DeathmatchManager.PlayersPerTeam);
        }

        /// <summary>
        /// Updates the name of the player.
        /// </summary>
        public void UpdateName()
        {
            DeathmatchManager.PlayerName = m_NameField.text;
        }

        /// <summary>
        /// Changes if the player should be in observer mode.
        /// </summary>
        /// <param name="observe">Should the player observe the game?</param>
        public void SetObserveMode(bool observe)
        {
            DeathmatchManager.ObserverMode = observe;
#if FIRST_PERSON_CONTROLLER && THIRD_PERSON_CONTROLLER
            m_PerspectiveToggle.gameObject.SetActive(!observe);
#endif
        }

        /// <summary>
        /// Changes if the game should start in a first person perspective.
        /// </summary>
        /// <param name="firstPerson">Should the game start in a first person perspective?</param>
        public void SetPerspective(bool firstPerson)
        {
            DeathmatchManager.FirstPersonPerspective = firstPerson;
        }

        /// <summary>
        /// Selects the difficulty of the game.
        /// </summary>
        /// <param name="index">An index representation of DeathmatchManager.AIDifficulty.</param>
        public void SelectDifficulty(int index)
        {
            var prevDifficulty = DeathmatchManager.Difficulty;
            DeathmatchManager.Difficulty = (DeathmatchManager.AIDifficulty)index;
            m_DifficultyImages[(int)prevDifficulty].sprite = m_DeselectedSprite;
            m_DifficultyImages[(int)prevDifficulty].color = m_DeselectedColor;
            m_DifficultyImages[(int)DeathmatchManager.Difficulty].sprite = m_SelectedSprite;
            m_DifficultyImages[(int)DeathmatchManager.Difficulty].color = m_SelectedColor;
        }

        /// <summary>
        /// Selects a FFA or team based game.
        /// </summary>
        /// <param name="index">Value indicating if the game is FFA (0) or team based (1).</param>
        public void SelectTeamGame(int index)
        {
            var prevTeamGame = DeathmatchManager.TeamGame;
            DeathmatchManager.TeamGame = index == 1;

            m_PlayerCount.SetActive(!DeathmatchManager.TeamGame);
            m_TeamCount.SetActive(DeathmatchManager.TeamGame);
            m_PlayersPerTeam.SetActive(DeathmatchManager.TeamGame);

            m_ModeImages[prevTeamGame ? 1 : 0].sprite = m_DeselectedSprite;
            m_ModeImages[prevTeamGame ? 1 : 0].color = m_DeselectedColor;
            m_ModeImages[DeathmatchManager.TeamGame ? 1 : 0].sprite = m_SelectedSprite;
            m_ModeImages[DeathmatchManager.TeamGame ? 1 : 0].color = m_SelectedColor;
        }

        /// <summary>
        /// Increases or decreases the player count.
        /// </summary>
        /// <param name="increase">Should the player count increase?</param>
        public void ChangePlayerCount(bool increase)
        {
            DeathmatchManager.PlayerCount += (increase ? 1 : -1);
            m_PlayerCount.SetCount(DeathmatchManager.PlayerCount);
        }

        /// <summary>
        /// Increases or decreases the team count.
        /// </summary>
        /// <param name="increase">Should the team count increase?</param>
        public void ChangeTeamCount(bool increase)
        {
            DeathmatchManager.TeamCount += (increase ? 1 : -1);
            m_TeamCount.SetCount(DeathmatchManager.TeamCount);
        }

        /// <summary>
        /// Increases or decreases the players per team count.
        /// </summary>
        /// <param name="increase">Should the players per team count increase?</param>
        public void ChangePlayersPerTeam(bool increase)
        {
            DeathmatchManager.PlayersPerTeam += (increase ? 1 : -1);
            m_PlayersPerTeam.SetCount(DeathmatchManager.PlayersPerTeam);
        }
    }
}