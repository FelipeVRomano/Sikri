using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscuridaoDensa : MonoBehaviour
{
    private ativaEspirito controleDeBarra;
   // [SerializeField]
   // private float dano;

    private bool timerSpirit;
    private float tempo;
    private bool timerPlayer;
    public AudioSource danoSom;

    private void Start()
    {
        tempo = 0;
    }
    private void Update()
    {
        tempo = Mathf.Clamp(tempo, -0.1f, 2);
        if (controleDeBarra != null)
        {
            if (!controleDeBarra.testando && timerSpirit)
            {
                timerSpirit = false;
            }
            if(controleDeBarra.testando && timerPlayer)
            {
                timerPlayer = false;
            }
            if (timerPlayer || timerSpirit)
            {
                tempo -= Time.deltaTime;
                if (tempo <= 0)
                {
                    controleDeBarra.RetiraVida();
                    tempo = 2;
                }
            }
            else
            {
                tempo = 0;
            }
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Purificador"))
        {
            if(controleDeBarra == null)
            controleDeBarra = other.gameObject.GetComponentInParent<ativaEspirito>();
            timerSpirit = true;

            if (!danoSom.isPlaying)
            {
                danoSom.Play();
            }

        }
        if(other.gameObject.CompareTag("Player"))
        {
            if(controleDeBarra == null)
            controleDeBarra = other.gameObject.GetComponent<ativaEspirito>();
            timerPlayer = true;

            if (!danoSom.isPlaying)
            {
                danoSom.Play();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Purificador"))
        {
            timerSpirit = true;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            timerPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Purificador"))
        {
            timerSpirit = false;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            timerPlayer = false;
        }
    }
}
