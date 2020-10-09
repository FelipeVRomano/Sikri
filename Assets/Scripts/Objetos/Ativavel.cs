using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ativavel : MonoBehaviour
{

    public bool ativa;
    [HideInInspector]
    public bool ativado;

    public GameObject objeto;
    public GameObject objeto2;
    public float speed;
    public Vector3[] posicaoPorta;
    bool abrindo;
    public AudioSource portaSom;
    public AudioSource alavancaSom;
    public Animator anim;
    bool ativaSom;
    private void Update()
    {
        
        if (abrindo)
        {
            anim.SetTrigger("Ativada");
            objeto.transform.localPosition = Vector3.Lerp(objeto.transform.localPosition, posicaoPorta[1], speed * Time.deltaTime);
            StartCoroutine(PortaAbrindoSom());
        }
        if (ativaSom)
        {
            Invoke("AlavancaSom", 2.7f);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Purificador"))
        {
            if (!ativa)
            {
                abrindo = true;

                if (!alavancaSom.isPlaying && !ativaSom)
                {
                    alavancaSom.Play();
                    ativaSom = true;
                }

            }
            else if(ativa)
            {             
                //transform.GetChild(2).gameObject.SetActive(true);
                objeto.SetActive(true);
                objeto2.SetActive(false);
                ativado = true;
                
            }
        }
    }

    void AlavancaSom()
    {
        if (alavancaSom.isPlaying)
            alavancaSom.Stop();
    }

    IEnumerator PortaAbrindoSom()
    {
        if(!portaSom.isPlaying)
        portaSom.Play();

        yield return new WaitForSeconds(5f);

        if (portaSom.isPlaying)
            portaSom.Stop();

    }
}
