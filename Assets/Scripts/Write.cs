using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Write : MonoBehaviour
{
  public TextMeshProUGUI textMeshProUGUI;



    private void Start()
    {
        StartCoroutine(Health());
    }
    IEnumerator Health()
    {
        yield return new WaitForSeconds(2f);
        textMeshProUGUI.text = "Ýlk düþmaný öldürmelisin. Daha sonrasýnda diðer düþmanlar seni duyacak.";
    }
}
