using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class finishline : MonoBehaviour
{
    public TextMeshProUGUI TextMeshProUGUI;
    public Animator animator;
    public GameObject panel;
    public int index;
    private void OnTriggerEnter(Collider other)
    {
        TextMeshProUGUI.text = "F";
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("stand");
            StartCoroutine(fin());
        }
    }
    IEnumerator fin()
    {
        yield return new WaitForSeconds(4f);
        panel.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(index);
    }
}
