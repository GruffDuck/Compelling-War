using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Checkpoint : MonoBehaviour
{
  public  string text;
  public TextMeshProUGUI textMeshProUGUI;


    private void Update()
    {
        if (FindObjectOfType<enemydeath>().sayac == 2)
        {
            textMeshProUGUI.text = "Görev Tamamlandý.";
        }
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
