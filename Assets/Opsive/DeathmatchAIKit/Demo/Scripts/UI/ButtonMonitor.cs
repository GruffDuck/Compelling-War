/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.UI
{
    using Opsive.DeathmatchAIKit.Demo.Game;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Receives the button events.
    /// </summary>
    public class ButtonMonitor : MonoBehaviour
    {
        /// <summary>
        /// Starts the game by loading the first scene.
        /// </summary>
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        /// <summary>
        /// Pauses or unpauses the game.
        /// </summary>
        /// <param name="paused">True if the game is paused.</param>
        public void PauseGame(bool paused)
        {
            DeathmatchManager.Paused = paused;
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        public void EndGame()
        {
            DeathmatchManager.EndGame();
        }
    }
}