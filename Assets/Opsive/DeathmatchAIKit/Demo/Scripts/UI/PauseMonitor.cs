/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.UI
{
    using Opsive.Shared.Events;
    using UnityEngine;

    /// <summary>
    /// Shows or hides the pause menu.
    /// </summary>
    public class PauseMonitor : MonoBehaviour
    {
        /// <summary>
        /// Initialize the default values.
        /// </summary>
        private void Awake()
        {
            ActivateChildren(false);
            EventHandler.RegisterEvent<bool>("OnPauseGame", PauseGame);
        }

        /// <summary>
        /// The game has been paused. Activator or deactivate the children.
        /// </summary>
        /// <param name="pause">Was the game paused?</param>
        private void PauseGame(bool pause)
        {
            ActivateChildren(pause);
        }

        /// <summary>
        /// Loops through the children and sets the active state.
        /// </summary>
        /// <param name="activate">Should the child GameObject be activated?</param>
        private void ActivateChildren(bool activate)
        {
            for (int i = 0; i < transform.childCount; ++i) {
                var child = transform.GetChild(i);
                child.gameObject.SetActive(activate);
            }
        }

        /// <summary>
        /// The object has been destroyed - unregister for all events.
        /// </summary>
        private void OnDestroy()
        {
            EventHandler.UnregisterEvent<bool>("OnPauseGame", PauseGame);
        }
    }
}