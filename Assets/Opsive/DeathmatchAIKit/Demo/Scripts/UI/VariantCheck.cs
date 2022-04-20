/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateCharacterController.Demo
{
    using UnityEngine;

    /// <summary>
    /// Ensures the correct controller variant is being used.
    /// </summary>
    public class VariantCheck : MonoBehaviour
    {
        [Tooltip("A reference to the notice Text object.")]
        [SerializeField] protected UnityEngine.UI.Text m_Notice;

        /// <summary>
        /// Determines if a valid variant is being used.
        /// </summary>
        private void Awake()
        {
            var isValid = false;

#if FIRST_PERSON_CONTROLLER && THIRD_PERSON_CONTROLLER && ULTIMATE_CHARACTER_CONTROLLER_SHOOTER && ULTIMATE_CHARACTER_CONTROLLER_MELEE
            isValid = true;
#elif FIRST_PERSON_CONTROLLER && ULTIMATE_CHARACTER_CONTROLLER_SHOOTER && ULTIMATE_CHARACTER_CONTROLLER_MELEE
            isValid = true;
#elif THIRD_PERSON_CONTROLLER && ULTIMATE_CHARACTER_CONTROLLER_SHOOTER && ULTIMATE_CHARACTER_CONTROLLER_MELEE
            isValid = true;
#endif
            if (isValid) {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// A valid variant is not being used. Show the notice.
        /// </summary>
        private void Start()
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            m_Notice.text = string.Format(m_Notice.text, Utility.AssetInfo.Name);
        }

        /// <summary>
        /// Closes the notice.
        /// </summary>
        public void Close()
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Destroy(gameObject);
        }
    }
}