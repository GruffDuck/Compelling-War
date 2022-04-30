using UnityEditor;
using UnityEngine;
using Worq.AEAI.HealthAndDamage;

namespace Worq.TPCharacter
{
    public class PlayerEditor : EditorWindow
    {
        [MenuItem("Tools/Worq/AEAI/Setup Player Health System")]
        public static void SetupHealth()
        {
            foreach (var go in Selection.gameObjects)
            {
                go.AddComponent(typeof(PlayerHealthManager));
                go.AddComponent(typeof(PlayerAttack));
                var bulletSpawnPos = new GameObject("BulletSpawnPos");
                bulletSpawnPos.transform.parent = go.transform;
                bulletSpawnPos.transform.SetPositionAndRotation(Vector3.zero, go.transform.rotation);
            }
        }
    }
}