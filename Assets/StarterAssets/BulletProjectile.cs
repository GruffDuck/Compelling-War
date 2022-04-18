using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb=GetComponent<Rigidbody>();
    }
    private void Start()
    {
        float speed = 20f;
        rb.velocity = transform.forward * speed ;
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
