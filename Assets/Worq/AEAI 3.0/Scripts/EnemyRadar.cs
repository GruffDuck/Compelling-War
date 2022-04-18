using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Worq.AEAI.Enemy
{
    [DisallowMultipleComponent]
    public class EnemyRadar : MonoBehaviour
    {
        //classes
        private GameObject lookAtPos;
        private GameObject targetObject;
        private GameObject returnedObject;
        private List<GameObject> seenObjects;
        private Vector3 offset = Vector3.zero;

        private RaycastHit hit;

        //vars
        private string targetTag;
        private float hearingRadius;
        private float audibilityThreshold;
        private float viewAngle;
        private float sightDistance;
        private bool isClearing;

        private bool hearingIsOn;

        //debug
        private bool drawRays;
        private bool drawVisionCone;
        private bool drawHearingRadius;
        private bool drawOverlapSphere;

        private bool playerSeen;

        //scripts
        private AIData info;

        void Start()
        {
            if (lookAtPos == null)
                lookAtPos = GameObject.Find("playerLookAt");

            seenObjects = new List<GameObject>();
            info = transform.parent.parent.GetComponent<AIData>();
            targetObject = info.player;
            transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        }

        void Update()
        {
            hearingRadius = info.hearingRadius;
            viewAngle = info.viewAngle;
            sightDistance = info.sightDistance;
            audibilityThreshold = info.audibilityThreshold;
            hearingIsOn = info.canHear;

            drawRays = info.drawRays;
            drawVisionCone = info.drawVisionCone;
            drawHearingRadius = info.drawHearingRadius;
            drawOverlapSphere = info.drawOverlapSphere;

            whatIsInSight(transform, viewAngle, sightDistance);
            foreach (GameObject go in seenObjects)
            {
//			Debug.Log ("has seen: " + go.name);
                if (go == this.targetObject)
                {
//				Debug.Log ("PLAYER IS IN SIGHT...");
                    transform.parent.GetComponent<EnemyAI>().playerDetected = true;
                }
            }

            if (hearingIsOn)
            {
                canHear(transform, audibilityThreshold, hearingRadius);
            }
        }

        void OnEnable()
        {
            if (seenObjects != null)
                seenObjects.Clear();
            isClearing = false;
            transform.parent.GetComponent<EnemyAI>().playerDetected = false;
        }

        public void whatIsInSight(Transform transform, float fieldOfViewAngle, float viewDistance)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, viewDistance);
            for (int i = 0; i < hitColliders.Length; i += 1)
            {
                GameObject target = hitColliders[i].gameObject;
                Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < fieldOfViewAngle / 2)
                {
                    //float dstToTarget = Vector3.Distance (transform.position, target.transform.position);
                    if (drawRays)
                    {
                        Debug.DrawRay(transform.position, dirToTarget * sightDistance, Color.blue);
                    }

                    if (Physics.Raycast(transform.position, dirToTarget, out hit, sightDistance))
                    {
                        seenObjects.Add(hit.collider.gameObject);
                        if (!isClearing)
                            StartCoroutine(clearSeenList());
                    }
                }
            }
        }

        public void canHear(Transform transform, float audibilityThreshold, float hearingRadius)
        {
            GameObject objectHeard = null;
            var hitColliders = Physics.OverlapSphere(transform.position, hearingRadius);
            if (hitColliders != null)
            {
                float maxAudibility = 0;
                for (int i = 0; i < hitColliders.Length; ++i)
                {
                    float audibility = 0.5f;
                    AudioSource[] colliderAudioSource;
                    if (hitColliders[i].gameObject == this.targetObject)
                    {
//					Debug.Log ("Player is in audibility radius");
                        if ((colliderAudioSource = targetObject.GetComponents<AudioSource>()) != null)
                        {
                            for (int j = 0; j < colliderAudioSource.Length; ++j)
                            {
                                if (colliderAudioSource[j].isPlaying)
                                {
                                    var distance = Vector3.Distance(transform.position,
                                        targetObject.transform.position);
                                    //////
//								if (colliderAudioSource[j].rolloffMode == AudioRolloffMode.Logarithmic) {
//									audibility = colliderAudioSource[j].volume / Mathf.Max(colliderAudioSource[j].minDistance, distance - colliderAudioSource[j].minDistance);
//								} else {
//									audibility = colliderAudioSource[j].volume * Mathf.Clamp01((distance - colliderAudioSource[j].minDistance) / (colliderAudioSource[j].maxDistance - colliderAudioSource[j].minDistance)); 
//								}
//								if (audibility > audibilityThreshold) {
//									Debug.Log ("Can hear");
//								} 	else
//									Debug.Log ("No can hear");
                                    ///
                                    if (colliderAudioSource[j].volume > audibilityThreshold)
                                    {
                                        transform.parent.GetComponent<EnemyAI>().isSeeking = true;
                                    }
                                    else
                                    {
                                        transform.parent.GetComponent<EnemyAI>().isSeeking = false;
                                    }
                                }
                                else
                                {
                                    transform.parent.GetComponent<EnemyAI>().isSeeking = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        transform.parent.GetComponent<EnemyAI>().isSeeking = false;
                    }
                }
            }
        }

        public static GameObject withinHearingRange(Transform transform, float audibilityThreshold,
            GameObject targetObject)
        {
            AudioSource[] colliderAudioSource;
            float audibility;
            if ((colliderAudioSource = targetObject.GetComponents<AudioSource>()) != null)
            {
                for (int i = 0; i < colliderAudioSource.Length; ++i)
                {
                    if (colliderAudioSource[i].isPlaying)
                    {
                        var distance = Vector3.Distance(transform.position, targetObject.transform.position);
                        if (colliderAudioSource[i].rolloffMode == AudioRolloffMode.Logarithmic)
                        {
                            audibility = colliderAudioSource[i].volume / Mathf.Max(colliderAudioSource[i].minDistance,
                                             distance - colliderAudioSource[i].minDistance);
                        }
                        else
                        {
                            audibility = colliderAudioSource[i].volume *
                                         Mathf.Clamp01((distance - colliderAudioSource[i].minDistance) /
                                                       (colliderAudioSource[i].maxDistance -
                                                        colliderAudioSource[i].minDistance));
                        }

                        if (audibility > audibilityThreshold)
                        {
                            return targetObject;
//						Debug.Log ("Can hear");
                        } //else

//						Debug.Log ("No can hear");
                    }
                }
            }

            return null;
        }

        public bool canHearPlayer()
        {
            return false;
        }

        public void OnDrawGizmos()
        {
            if (drawOverlapSphere)
            {
                Gizmos.DrawWireSphere(transform.position, sightDistance);
            }

            if (drawHearingRadius && hearingIsOn)
            {
                drawHearngGizmo();
            }

            if (drawVisionCone)
            {
                drawLineOfSight(transform, offset, viewAngle, sightDistance);
            }
        }

        public void drawLineOfSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle,
            float viewDistance)
        {
#if UNITY_EDITOR
            var mColor = Handles.color;
            var color = Color.blue;
            color.a = 0.1f;
            Handles.color = color;

            var halfFOV = fieldOfViewAngle * 0.5f;
            var beginDirection = Quaternion.AngleAxis(-halfFOV, (Vector3.up)) * (transform.forward);
            Handles.DrawSolidArc(transform.TransformPoint(positionOffset), transform.up, beginDirection,
                fieldOfViewAngle, viewDistance);
            Handles.color = mColor;
#endif
        }

        public void drawHearngGizmo()
        {
#if UNITY_EDITOR
            if (transform == null)
            {
                return;
            }

            var mColor = Handles.color;
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, transform.up, hearingRadius);
            Handles.color = mColor;
#endif
        }

        IEnumerator clearSeenList()
        {
            isClearing = true;
            yield return new WaitForSecondsRealtime(0.5f);
            seenObjects.Clear();
            isClearing = false;
        }
    }
}