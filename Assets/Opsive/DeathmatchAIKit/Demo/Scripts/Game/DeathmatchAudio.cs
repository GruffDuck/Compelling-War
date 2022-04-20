/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.Game
{
    using UnityEngine;
    using Opsive.Shared.Events;

    /// <summary>
    /// Plays any deathmatch game specific audio.
    /// </summary>
    public class DeathmatchAudio : MonoBehaviour
    {
        [Tooltip("Audio reference when the game starts.")]
        [SerializeField] protected AudioClip m_StartGame;
        [Tooltip("Audio reference when the player wins the game.")]
        [SerializeField] protected AudioClip m_GameOverWinner;
        [Tooltip("Audio reference when the player loses the game.")]
        [SerializeField] protected AudioClip m_GameOverLoser;
        [Tooltip("The number of seconds of delay before the game over audio is played.")]
        [SerializeField] protected float m_GameOverDelay;

        private AudioSource m_CameraAudioSource;

        /// <summary>
        /// Register for any interested events.
        /// </summary>
        private void Awake()
        {
            EventHandler.RegisterEvent("OnStartGame", StartGame);
            EventHandler.RegisterEvent<bool>("OnGameOver", GameOver);
        }

        /// <summary>
        /// The game has started.
        /// </summary>
        private void StartGame()
        {
            // StartGame will be called before Awake.
            if (m_CameraAudioSource == null) {
                m_CameraAudioSource = Camera.main.GetComponent<AudioSource>();
                if (m_CameraAudioSource == null) {
                    m_CameraAudioSource = Camera.main.gameObject.AddComponent<AudioSource>();
                }
            }

            if (m_StartGame != null) {
                m_CameraAudioSource.clip = m_StartGame;
                m_CameraAudioSource.Play();
            }
        }

        /// <summary>
        /// The game has ended.
        /// </summary>
        /// <param name="winner">Did the local player win?</param>
        private void GameOver(bool winner)
        {
            var audioClip = winner ? m_GameOverWinner : m_GameOverLoser;
            if (audioClip != null) {
                m_CameraAudioSource.clip = audioClip;
                if (m_GameOverDelay > 0) {
                    m_CameraAudioSource.PlayDelayed(m_GameOverDelay);
                } else {
                    m_CameraAudioSource.Play();
                }
            }
        }
    }
}