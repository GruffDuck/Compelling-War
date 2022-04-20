/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.UI
{
    using Opsive.Shared.Events;
    using Opsive.UltimateCharacterController.UI;

    /// <summary>
    /// Inherits DamaageIndicatorMonitor to hide the hit indicators when the game is over.
    /// </summary>
    public class DeathmatchDamageIndicatorMonitor : DamageIndicatorMonitor
    {
        /// <summary>
        /// Initialize the default values.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            EventHandler.RegisterEvent<bool>("OnGameOver", GameOver);
        }

        /// <summary>
        /// The game has ended. Deactivate the GameObject.
        /// </summary>
        /// <param name="winner">Did the local player win?</param>
        private void GameOver(bool winner)
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// The object has been destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            EventHandler.UnregisterEvent<bool>("OnGameOver", GameOver);
        }
    }
}
