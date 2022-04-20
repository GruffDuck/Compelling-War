using System;
using UnityEngine;
using UnityEngine.UI;

namespace Worq.AEAI.HealthAndDamage
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class EnemyHealthManager : MonoBehaviour
    {
        public float startingHealth = 100;
        public static float startingH = 100;
        public static float currentEnemyHealth;
        public float currentHealth;
        public Slider healthSlider;
        public Image damageImage;
        public AudioClip deathAudio;
        public float flashSpeed = 5f;
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

        Animator anim;
        AudioSource playerAudio;
        bool isDead;
        bool damaged;

        void Awake()
        {
            anim = GetComponent<Animator>();
            playerAudio = GetComponent<AudioSource>();
            currentEnemyHealth = startingHealth;
            startingH = startingHealth;
        }

        void Update()
        {
            try
            {
                if (damaged)
                {
                    damageImage.color = flashColour;
                }
                else
                {
                    damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
                }

                damaged = false;
            }
            catch (Exception e)
            {
            }

            currentHealth = currentEnemyHealth;
        }

        public void TakeDamage(float amount)
        {
            damaged = true;
            currentEnemyHealth -= amount;

            if (healthSlider != null)
                healthSlider.value = currentEnemyHealth;

            if (deathAudio != null)
                playerAudio.Play();

            if (currentEnemyHealth <= 0 && !isDead)
            {
                Die();
            }
        }

        void Die()
        {
            // Set the death flag so this function won't be called again.
            isDead = true;
//            anim.SetTrigger("Die");

            if (deathAudio != null)
            {
                playerAudio.clip = deathAudio;
                playerAudio.Play();
            }
        }

        public static void ResetHealth()
        {
            currentEnemyHealth = startingH;
        }
    }
}