using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

public class TPSC : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimCam;
    private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private float aimSens;
    [SerializeField] private float normalSens;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform spawnBullet;
    private ThirdPersonController thirdPersonController;
    private Animator animator;
    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }
    private void Update()
    { Vector3 mouseworldPos = Vector3.zero;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
          //  debugTransform.position = raycastHit.point;
            mouseworldPos = raycastHit.point;
            hitTransform = raycastHit.transform;
        }
        if (starterAssetsInputs.aim)
        {
            aimCam.gameObject.SetActive(true);
            thirdPersonController.SetSens(aimSens);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1),1f,Time.deltaTime*10f));
            Vector3 worldAimTarget = mouseworldPos;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDir=(worldAimTarget-transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 20f);
        }
        else
        {
           
            aimCam.gameObject.SetActive(false);
            thirdPersonController.SetSens(normalSens);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
        if (starterAssetsInputs.shoot)
        {
            
            Vector3 aimDr = (mouseworldPos - spawnBullet.position).normalized;
            Instantiate(bulletPrefab, spawnBullet.position, Quaternion.LookRotation(aimDr, Vector3.up));
            starterAssetsInputs.shoot = false;
        }
    }
}
