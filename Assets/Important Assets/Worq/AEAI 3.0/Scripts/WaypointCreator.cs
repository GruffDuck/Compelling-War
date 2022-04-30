using UnityEngine;

namespace Worq.AEAI.Waypoint
{
    public class WaypointCreator : MonoBehaviour
    {
        public void CreateNewWaypoint(GameObject go)
        {
            var mShader = Shader.Find("Standard");
            var mMat = new Material(mShader) {color = Color.yellow};
            //		rend.material = new Material(Shader.Find("Specular"));

            var waypoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            waypoint.transform.SetParent(go.transform);
            waypoint.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            waypoint.transform.localPosition =
                new Vector3(Random.Range(-5f, 5f), go.transform.localScale.y / 4, Random.Range(-5f, 5f));
            waypoint.name = "waypoint";
            waypoint.GetComponent<Renderer>().material = mMat;
            waypoint.GetComponent<Collider>().isTrigger = true;
            waypoint.AddComponent<WaypointIdentifier>();
        }
    }
}