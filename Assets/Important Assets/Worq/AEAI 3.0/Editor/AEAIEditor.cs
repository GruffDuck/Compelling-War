using UnityEditor;
using UnityEngine;
using Worq.AEAI.HealthAndDamage;
using Worq.AEAI.Waypoint;

namespace Worq.AEAI.Enemy
{
    public class AEAIEditor : EditorWindow
    {
        [MenuItem("Tools/Worq/AEAI/Create AI")]
        public static void AddEnemyAI()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                if (go.GetComponent<AEAIIdentifier>() == null)
                {
                    var enemy = new GameObject("AIEnemy");
                    enemy.transform.position = go.transform.position;

                    enemy.AddComponent<AEAIIdentifier>();
                    enemy.AddComponent<AIData>();

                    go.AddComponent<Animation>();
                    go.AddComponent<EnemyAI>();
//                    go.AddComponent(typeof(EnemyHealthManager));
                    
                    go.transform.SetParent(enemy.transform);
                }
                else
                    Debug.Log("Selected GameObject already has AI");
            }
        }

        [MenuItem("Tools/Worq/AEAI/Add New Waypoint")]
        public static void createWaypoint()
        {
            WaypointCreator wp = new WaypointCreator();
            foreach (GameObject go in Selection.gameObjects)
            {
                if (go.GetComponent<AEAIIdentifier>() != null)
                    wp.CreateNewWaypoint(go);
                else
                    Debug.Log("No AI on selected GameObject. Create AI first");
            }
        }

//        [MenuItem("Tools/Worq/AEAI/Setup Enemy Health System")]
//        public static void SetupHealth()
//        {
//            foreach (var ai in FindObjectsOfType<EnemyAI>())
//            {
//                ai.gameObject.AddComponent(typeof(EnemyHealthManager));
//            }
//        }
    }
}