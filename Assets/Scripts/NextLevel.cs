using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{

    public string text;
    public TextMeshProUGUI textMeshProUGUI;
    public GameObject gameObject;
    public int scneIndex;
    private void OnTriggerEnter(Collider other)
    {

       
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(texts());
            
            gameObject.SetActive(true);
            StartCoroutine(finishLine());

        }
    }
    IEnumerator texts()
    {
        textMeshProUGUI.color = Color.green;
        yield return new WaitForSeconds(1f);
        textMeshProUGUI.color = Color.white;
        textMeshProUGUI.text = text;
    }
    IEnumerator finishLine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scneIndex);
    }
}
