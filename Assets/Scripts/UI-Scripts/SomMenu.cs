using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomMenu : MonoBehaviour
{
    public GameObject objeto;
    public AudioSource som;

    public void TocaSom()
    {
        StartCoroutine(Som());
    }

    IEnumerator Som()
    {
        som.Play();
        yield return new WaitForSeconds(0.001f);
        objeto.SetActive(false);
    }
}
