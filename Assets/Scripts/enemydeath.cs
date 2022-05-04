using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydeath : MonoBehaviour
{
    public Camera camera;
    int health = 100;
    public int sayac;
    public int healt;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseworldPos = Vector3.zero;
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
            {
                Debug.Log(raycastHit.transform.name);
                if (raycastHit.collider.tag == "Enemy")
                {

                    raycastHit.collider.GetComponent<enemyhealths>().healt -= healt;

                    if (raycastHit.collider.GetComponent<enemyhealths>().healt <= 0)
                    {
                        sayac++;
                        raycastHit.collider.GetComponent<Animator>().SetTrigger("isDead");
                        Destroy(raycastHit.collider.gameObject, 1.5f);
                    }
                }
            }


        }
    }
}

