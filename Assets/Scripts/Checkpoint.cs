using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Checkpoint : MonoBehaviour
{
  public  string text;
    public int satac;
  public TextMeshProUGUI textMeshProUGUI;


    private void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {

        StartCoroutine(texts());
     
    }
    IEnumerator texts()
    {
        textMeshProUGUI.color = Color.green;
        yield return new WaitForSeconds(1f);
        textMeshProUGUI.color = Color.white;
        textMeshProUGUI.text = text;
    }
  
   
}
