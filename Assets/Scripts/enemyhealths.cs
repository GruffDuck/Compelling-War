using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemyhealths : MonoBehaviour
{
    public string texxts;
   public int healt = 100;
    public TextMeshProUGUI textMeshProUGUI;
    public void Update()
    {
        if (healt<= 0)
        {
            StartCoroutine(HealtH());
        }
    }
    IEnumerator HealtH()
    {
        textMeshProUGUI.color = Color.green;
        yield return new WaitForSeconds(1f);
        textMeshProUGUI.color = Color.white;
        textMeshProUGUI.text = texxts;
    }
}
