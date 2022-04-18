using UnityEngine;
using Worq.AEAI.Enemy;

namespace Worq.AEAI.HealthAndDamage
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class PlayerBullet : MonoBehaviour
    {
        private AIData info;

        public string enemyTag;

        private void Awake()
        {
            Destroy(gameObject, 3f);
            if (string.IsNullOrEmpty(enemyTag))
                enemyTag = "";
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(enemyTag))
            {
                //ToDo damage enemy
                Debug.Log("Bullet Hit Enemy");
                info = other.transform.parent.GetComponent<AIData>();
                if (info)
                    other.gameObject.GetComponent<EnemyHealthManager>().TakeDamage(info.enemyTakeDamageValue);
            }
        }
    }
}