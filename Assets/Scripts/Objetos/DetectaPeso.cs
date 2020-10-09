using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectaPeso : MonoBehaviour
{
    [SerializeField]
    private GameObject ativaGameObject;
    bool abrindo;
    float y;

    //para objetos que vão cair
    [SerializeField]
    private bool quebravel;
    private Rigidbody body;
    [SerializeField]
    private float DelayParaSumir;
    [SerializeField]
    private float DelayParaVoltar;
    float x = 3f;
    float y2 = 3f;


    //para a plataforma móvel
    [SerializeField]
    private Transform player;
    [SerializeField]
    private bool estaColidindo;
    [SerializeField]
    private bool controlaCoroutine;
    [SerializeField]
    private bool mudaPos;
    [SerializeField]
    private bool ativaAnim;

    public AudioSource caindoAudio;

  
    void Start()
    {
    }
    void Update()
    {
        if (!quebravel)
        {
            if (abrindo)
                ativaGameObject.SetActive(false);
            //y = -30f;
            else
            {
                ativaGameObject.SetActive(true);
                //y = 30f;
            }
        }
        else if(quebravel)
        {
            if (Morreu.morreu)
            {
                if (estaColidindo || abrindo || controlaCoroutine)
                {
                    estaColidindo = false;
                    abrindo = false;
                    controlaCoroutine = false;
                }
                else
                {
                    Morreu.morreu = false;
                }
            }
            if (abrindo && !controlaCoroutine)
            {
                StartCoroutine(DelayQueda());
            }
        }


    }
    private void FixedUpdate()
    {
        if (mudaPos)
        {
            player.position = new Vector3(player.position.x, transform.position.y + 1.7f, player.position.z);
            mudaPos = false;
        }
        if(ativaAnim)
        {
            x = x * -1;
            y2 = y2 * -1;
            transform.Translate(new Vector3(x, y2, 0) * Time.deltaTime);
        }
    }
    IEnumerator DelayQueda()
    {
        controlaCoroutine = true;
        ativaAnim = true;
        yield return new WaitForSeconds(DelayParaSumir);
        ativaAnim = false;
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(DelayParaVoltar);
        transform.GetChild(0).position = transform.position;
        transform.GetChild(0).gameObject.SetActive(true);

        if (caindoAudio.isPlaying)
        {
            caindoAudio.Stop();
        }

        //faz com que o player nao atravesse a plataforma.
        print(transform.position);
        if (estaColidindo)
        { 
            mudaPos = true;
        }
        controlaCoroutine = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            abrindo = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (quebravel)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                estaColidindo = true;

                if (!caindoAudio.isPlaying)
                {
                    caindoAudio.Play();
                }

                //pega o transform do player.
                if (player == null)
               {
                   player = other.gameObject.GetComponent<Transform>();                             
                                   
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        abrindo = false;

        if (quebravel)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                estaColidindo = false;
            }
        }
    }
}
