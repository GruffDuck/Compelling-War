/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Editor.Managers
{
    /// <summary>
    /// The Manager is an abstract class which allows for various categories to the drawn to the MainManagerWindow pane.
    /// </summary>
    [System.Serializable]
    public abstract class Manager
    {
        protected MainManagerWindow m_MainManagerWindow;

        /// <summary>
        /// Initialize the manager after deserialization.
        /// </summary>
        public virtual void Initialize(MainManagerWindow mainManagerWindow) { m_MainManagerWindow = mainManagerWindow; }

        /// <summary>
        /// Draws the Manager.
        /// </summary>
        public abstract void OnGUI();
    }
}