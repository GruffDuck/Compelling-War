using System;
using UnityEngine;

namespace Worq.AEAI.HealthAndDamage
{
    [DisallowMultipleComponent]
    public class PlayerAttack : MonoBehaviour
    {
        public KeyCode shootKey = KeyCode.LeftControl;

        public Rigidbody bulletPrefab;
        public GameObject spawnPoint;
        public string enemyTag;
        public float damageAmmount = 10f;
        public float bulletRange = 10f;

        private RaycastHit hit;
        private GameObject enemyObj;


        private void Awake()
        {
            try
            {
                enemyObj = GameObject.FindWithTag(enemyTag);
            }
            catch (Exception e)
            {
            }

            spawnPoint = transform.Find("BulletSpawnPos").gameObject;
        }

        private void Update()
        {
            if (Input.GetKey(shootKey)) //shootKey
            {
                if (string.IsNullOrEmpty(enemyTag))
                    enemyTag = "";

                var hitPlayer = Instantiate(bulletPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
                
                if (!hitPlayer.GetComponent<PlayerBullet>())
                    hitPlayer.gameObject.AddComponent<PlayerBullet>();
                
                hitPlayer.velocity = transform.TransformDirection(Vector3.forward * 100);
                Physics.IgnoreCollision(bulletPrefab.GetComponent<Collider>(),
                    transform.root.GetComponent<Collider>());

                //ray
                Vector3 fwd = transform.TransformDirection(Vector3.forward);
                Debug.DrawRay(transform.position, fwd * bulletRange, Color.green);

                var startVector = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                if (Physics.Raycast(startVector, fwd, out hit, bulletRange))
                {
                    Debug.Log("Has hit of tag: " + hit.collider.gameObject.tag);
                    if (hit.collider.gameObject.tag.Equals(enemyTag))
                    {
//                        Debug.Log("Enemy hit with Raycast");
                    }
                }
            }
        }

        private void AttackEnemy(GameObject enemy)
        {
            enemy.SendMessage("PlayerTakeDamage", damageAmmount, SendMessageOptions.DontRequireReceiver);
        }
    }
}