using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{
  public  string text;
  public TextMeshProUGUI textMeshProUGUI;
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
