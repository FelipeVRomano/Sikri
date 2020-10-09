using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextosNarrativa : MonoBehaviour
{
    public Image fade;
    public Text fade2;
    public Text fade2a;
    public Image fade3;
    public Image fade4;
    public bool temFrô;
    public bool naoTemTextoAnterior;
    public bool hub;
    public bool planoSequencia;

    bool executando;
    
    public GameObject textoAnterior;
    public GameObject objetoAnterior;
    public GameObject esferaluz;

    BoxCollider me;

    [SerializeField]
    private bool transicao;

    [SerializeField]
    private Transform camera2;

    [SerializeField]
    private Transform cameraOrigin;

    private TerceiraPessoaCam terceira;
    private CameraColision cam;
    private ativaEspirito ativa;
    private movimento movimento;
    Vector3 actualRot;
    Vector3 actualPos;

    [SerializeField]
    private bool temAnim;
    [SerializeField]
    private Animator anim;

     [SerializeField]
     private bool temRepetido;
    [SerializeField]
    private GameObject copia;

    [SerializeField]
    private bool imgs;

    [SerializeField]
    private Text[] txt;

    [SerializeField]
    private GameObject particleMinha;
    [SerializeField]
    private GameObject particle;

    [SerializeField]
    private BoxCollider next;
    void Start()
    {
        fade.canvasRenderer.SetAlpha(0.0f);
        fade2.canvasRenderer.SetAlpha(0.0f);

        if (temFrô)
        {
            fade3.canvasRenderer.SetAlpha(0.0f);
            if(fade4 != null)
                fade4.canvasRenderer.SetAlpha(0.0f);
        }
        
        if (planoSequencia)
        {
            fade2a.canvasRenderer.SetAlpha(0.0f);
        }


        me = GetComponent<BoxCollider>();
        if (transicao)
        {
            cameraOrigin = GameObject.Find("Camera").GetComponent<Transform>();
            terceira = GameObject.Find("CameraBase").GetComponent<TerceiraPessoaCam>();
            cam = GameObject.Find("Camera").GetComponent<CameraColision>();
            ativa = GameObject.Find("Player").GetComponent<ativaEspirito>();
            movimento = GameObject.Find("Player").GetComponent<movimento>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hub)
        {
            if (esferaluz.activeSelf)
            {
               me.enabled = true;
            }

            if (!esferaluz.activeSelf)
            {
                me.enabled = false;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (naoTemTextoAnterior == false)
        {
            if (textoAnterior.activeSelf)
            {
                textoAnterior.SetActive(false);
                objetoAnterior.SetActive(false);
            }
        }
      
        if (planoSequencia == false)
        {
            StartCoroutine(Fade());
        }
        

        if (planoSequencia == true)
        {
            StartCoroutine(PlanoSequencia());
        }
    }

    public void chamaIEnumerator()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        if (!executando)
        {
            executando = true;


            if (temRepetido)
                copia.SetActive(false);
            fade.CrossFadeAlpha(1, 1, false);
            fade2.CrossFadeAlpha(1, 1, false);

            if (temFrô)
            {
                fade3.CrossFadeAlpha(1, 1, false);
                if (fade4 != null)
                    fade4.CrossFadeAlpha(1, 1, false);
            }
            if (transicao)
            {
                terceira.enabled = false;
                movimento.podeSeMovimentar = false;
                cam.enabled = false;
                ativa.enabled = false;
                actualPos = cameraOrigin.position;
                actualRot = cameraOrigin.eulerAngles;
                cameraOrigin.position = camera2.position;
                cameraOrigin.eulerAngles = camera2.eulerAngles;
                if(temAnim && anim != null)
                {
                    anim.SetBool("roda", true);
                }
            }
            yield return new WaitForSeconds(3);
            if (transicao)
            {
                cameraOrigin.position = actualPos;
                cameraOrigin.eulerAngles = actualRot;
                terceira.enabled = true;
                movimento.podeSeMovimentar = true;
                cam.enabled = true;
                ativa.enabled = true;
            }
            if (temFrô)
            {
                fade3.CrossFadeAlpha(0, 1, false);
                if (fade4 != null)
                    fade4.CrossFadeAlpha(0, 1, false);
            }

            fade2.CrossFadeAlpha(0, 1, false);
            fade.CrossFadeAlpha(0, 1, false);
            if(imgs)
            {
                yield return new WaitForSeconds(2f);
                particleMinha.SetActive(false);
                particle.SetActive(true);
                next.enabled = true;
            }
            gameObject.SetActive(false);
            executando = false;

       
        }


    }

    IEnumerator PlanoSequencia()
    {
        if (!executando)
        {
            executando = true;

            fade.CrossFadeAlpha(1, 1, false);
            fade2.CrossFadeAlpha(1, 1, false);

            yield return new WaitForSeconds(2);

            fade2.CrossFadeAlpha(0, 1, false);

            yield return new WaitForSeconds(0.8f);

            fade2a.CrossFadeAlpha(1, 1, false);

            yield return new WaitForSeconds(2);

            fade2a.CrossFadeAlpha(0, 1, false);
            fade.CrossFadeAlpha(0, 1, false);

            

          //  yield return new WaitForSeconds(2);
            gameObject.SetActive(false);
            executando = false;

           
        }
    }


}
