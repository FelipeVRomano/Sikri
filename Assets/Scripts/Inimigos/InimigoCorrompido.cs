using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoCorrompido : MonoBehaviour
{
    private Transform Enemy;
    [HideInInspector]
    public bool encontrado;
   // [SerializeField]
   // private float y;
    [SerializeField]
    private Transform player;

    [SerializeField]
    private ativaEspirito control;

    [SerializeField]
    private float velocidadeEstatua;
    [HideInInspector]
    public bool estaColidindoSpirit;
    private BoxCollider trigger;

    public bool funciona;

    private bool controlCoroutine;
    [SerializeField]
    private float anguloDaEstatua;

    [SerializeField]
    private float dano;

    public CharacterController playerControl;
    Vector3 firstPosition;
    [SerializeField]
    float speedBaseMovEmp;
    [SerializeField]
    float empurraVel;

    private bool playerTaAqui;

    private bool timer;
    private float tim;

    [SerializeField]
    private float tempoParaVoltar;

    public AudioSource somHit;
    public bool controlaSom;
    void Start()
    {
        Enemy = GetComponent<Transform>();
        trigger = GetComponent<BoxCollider>();
        funciona = true;
        if (funciona)
            StartCoroutine(controlEnemy());
        firstPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       

        if (Morreu.morreu)
        {
            MorreuPlayer();
        }
        if (!funciona)
        {
            StopCoroutine(controlEnemy());
            controlCoroutine = true;
        }
  //      Mathf.Clamp(y, 0, 360);
        if (funciona)
        {

            if (encontrado && control != null)
            {
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                if (player != null)
                {
                     transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), speedBaseMovEmp * Time.deltaTime);
                    //Mais apelão, necessita de outros acertos.
                    // transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), speedBaseMovEmp * Time.deltaTime);
                    playerControl.Move(empurraVel * speedBaseMovEmp* transform.forward * Time.deltaTime);
                }
            }
        }
        if(!encontrado && controlCoroutine && funciona)
        {
            StartCoroutine(controlEnemy());
            controlCoroutine = false;
        }
    }

    IEnumerator controlEnemy()
    {
            if (!encontrado && funciona)
            yield return null;
            controlCoroutine = true;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (funciona)
        {
            if (other.gameObject.CompareTag("Purificador"))
            {
                
                if (!playerTaAqui)
                {
                    if (control == null)
                        control = other.transform.GetComponentInParent<ativaEspirito>();
                    player = other.gameObject.GetComponent<Transform>();
                    playerControl = other.gameObject.GetComponent<CharacterController>();
                    encontrado = true;
                    control.retiradorDeEnergia = true;
                    controlaSom = true;
                    StartCoroutine(TocaSom());
                }
               
            }
            if (control == null)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    control = other.gameObject.GetComponent<ativaEspirito>();
                    playerTaAqui = true;
                    player = other.gameObject.GetComponent<Transform>();
                    playerControl = other.gameObject.GetComponent<CharacterController>();
                    encontrado = true;
                    control.retiradorDeEnergia = true;
                    controlaSom = true;
                    StartCoroutine(TocaSom());
                }
                
            }
            else
            {
                if (other.gameObject.CompareTag("Player") && (control.ativouEsfera || !estaColidindoSpirit))
                {
                    control = other.gameObject.GetComponent<ativaEspirito>();
                    playerTaAqui = true;
                    player = other.gameObject.GetComponent<Transform>();
                    playerControl = other.gameObject.GetComponent<CharacterController>();
                    encontrado = true;
                    control.retiradorDeEnergia = true;
                    
                }
            }
        }
    }
    public void MorreuPlayer()
    {                
        control.retiradorDeEnergia = false;        
        player = null;
        encontrado = false;
        playerControl = null;
        if (controlCoroutine && funciona)
        {
            StartCoroutine(controlEnemy());
            controlCoroutine = false;
        }
        playerTaAqui = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (funciona)
        {
            if (other.gameObject.CompareTag("Purificador"))
            {
                if (!playerTaAqui)
                {
                    estaColidindoSpirit = true;
                    control.retiradorDeEnergia = true;
                }
            }
            if(other.gameObject.CompareTag("Player"))
            {               
                control.retiradorDeEnergia = true;
                playerTaAqui = true;
                if (!estaColidindoSpirit && !encontrado)
                {
                    playerControl = other.gameObject.GetComponent<CharacterController>();             
                    encontrado = true;
                }
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Purificador"))
        {
            controlaSom = false;
            StopCoroutine(TocaSom());
        }

        if (funciona)
        {
            if (control != null)
            {
                if (other.gameObject.CompareTag("Player") && control.ativouEsfera)
                {
                    
                    control.retiradorDeEnergia = false;
                    player = null;
                    encontrado = false;
                    playerControl = null;
                    if (controlCoroutine && funciona)
                    {
                        StartCoroutine(controlEnemy());
                        controlCoroutine = false;
                    }
                    playerTaAqui = false;
                    
                }
                
            }
            if (other.gameObject.CompareTag("Purificador") && !playerTaAqui)
            {
                
                control.retiradorDeEnergia = false;
                player = null;
                encontrado = false;
                playerControl = null;
                if (controlCoroutine && funciona)
                {
                    StartCoroutine(controlEnemy());
                    controlCoroutine = false;
                }
                estaColidindoSpirit = false;
                
            }

        }
    }

    public void paraSom()
    {
        controlaSom = false;
        StopCoroutine(TocaSom());
    }
    IEnumerator TocaSom()
    {
        while (controlaSom)
        {
            yield return new WaitForSeconds(0.5f);
            if (!somHit.isPlaying)
            {
                somHit.Play();
            }           
        }        
    }
}
