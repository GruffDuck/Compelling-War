/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo
{
    using Opsive.DeathmatchAIKit.Demo.Game;
    using UnityEngine;

    /// <summary>
    /// Prepares the scene for a deathmatch game by spawning the Deathmatch Manager prefab.
    /// </summary>
    public class Startup : MonoBehaviour
    {
        [Tooltip("The prefab to spawn upon startup")]
        [SerializeField] protected GameObject m_DeathmatchManagerPrefab;

        /// <summary>
        /// Spawn the DeathmatchManager prefab if there is no DeathmatchManager.
        /// </summary>
        private void Awake()
        {
            if (!DeathmatchManager.IsInstantiated) {
                GameObject.Instantiate(m_DeathmatchManagerPrefab);
            }
        }

        /// <summary>
        /// If a game scene is loaded then notify the DeathmatchManager that the scene was loaded.
        /// </summary>
        private void Start()
        {
            Destroy(gameObject);
        }
    }
}