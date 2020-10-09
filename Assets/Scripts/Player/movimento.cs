using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movimento : MonoBehaviour
{
    private float horizontalMove;
    private float verticalMove;
    private Vector3 playerInput;

    public CharacterController Player;

    public float PlayerSpeed;
    private Vector3 movePlayer;

    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    public float gravity;
    public float fallVelocity;
    public float jumpForce;

    //extra
    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float slideVelocity;
    public float slopeForceDown;

    [SerializeField]
    private Animator anim;


    private bool pulando;
    private bool correndo;
    private float corrida;
    private float andarNormal;


    //novas
    public bool controlaSePodeMovimentar;
    public bool podeSeMovimentar;

    //GameController
    private GameController gm;
    public ativaEspirito ativandoEspirito;
    private float timer;


    private bool Interagindo;

    public Interagir interagir;

    //Efeitos Sonoros
    [Header("Efeitos Sonoros")]
    public AudioSource andandoAudio;
    public AudioSource pulandoAudio;
    public AudioClip[] andandoArray;
    public AudioSource coletavelAudio;
    public AudioSource arrastaAudio;


    [SerializeField]
    private bool level3;

    [SerializeField]
    private Image fade;
    [SerializeField]
    private GameObject fade2;


    float mortes;
    [SerializeField]
    GameObject[] textos;
    int index;

    public Animator animHud;

    [SerializeField]
    private TextosNarrativa textNar;

    [SerializeField]
    private float corridaMultiplicador = 1.3f;

    [SerializeField]
    private TerceiraPessoaCam third;
    void Start()
    {
        andandoAudio = GetComponent<AudioSource>();


        mortes = 0;
        Player = GetComponent<CharacterController>();
        ativandoEspirito = GetComponent<ativaEspirito>();
      
        podeSeMovimentar = true;

        andarNormal = PlayerSpeed;
        corrida = PlayerSpeed * corridaMultiplicador;

        Physics.IgnoreLayerCollision(9, 10, true);

        timer = 0;

        interagir = GetComponent<Interagir>();

        if(!level3)
        SpawnPos();
    }

    void Update()
    {
        //pega Input
        if (podeSeMovimentar)
        {
            horizontalMove = Input.GetAxis("Horizontal");
            verticalMove = Input.GetAxis("Vertical");
            
        }
        else
        {
            horizontalMove = 0;
            verticalMove = 0;
        }
        animacao();
        //print(verticalMove);
        //joga o input em um Vector3 e limita o valor 
        if (podeSeMovimentar)
        {
            playerInput = new Vector3(horizontalMove, 0, verticalMove);
            //playerInput = new Vector3(0, 0, verticalMove);
            playerInput = Vector3.ClampMagnitude(playerInput, 1);
        }
        else
        {
            playerInput = Vector3.zero;
        }
        //pega o angulo da camera
        CamDirection();

        if (controlaSePodeMovimentar)
            timer += Time.deltaTime;

        if (timer > 0.5f)
        {
            controlaSePodeMovimentar = false;
            podeSeMovimentar = true;
            timer = 0;
        }
        //passa as variaveis de movimento
        if (podeSeMovimentar)
        {
            movePlayer = playerInput.x * camRight + playerInput.z * camForward;
            movePlayer = movePlayer * PlayerSpeed;
            
        }
        else
        {
            movePlayer = Vector3.zero;
        }
        //rotaciona o Player
        if (playerInput.x != 0 || playerInput.z != 0)
        {
            if (!interagir.interagir)
            {
                Player.transform.LookAt(Player.transform.position + movePlayer);
                if (!interagir.interagir && Input.GetButton("Run"))
                {
                    if (!pulando)
                        correndo = true;
                }
                else
                {
                    correndo = false;
                }
            }
            else
            {
                correndo = false;
            }
        }
        else
        {
            correndo = false;
        }

        if (correndo)
            PlayerSpeed = corrida;
        else
        {
            PlayerSpeed = andarNormal;
        }
        //gravidade
        SetGravity();

        //funcao para as habilidades do player
        PlayerSkills();

        // move o Player
        //if (podeSeMovimentar)
        //{
            Player.Move(movePlayer * Time.deltaTime);
        //}

    }

    void animacao()
    {
        if (podeSeMovimentar)
        {
            
            if (playerInput.x == 0 && playerInput.z == 0 && !interagir.interagir && !pulando && !correndo)
            {
                //print("idle");
                anim.SetBool("pulando", false);
                anim.SetBool("andando", false);
                anim.SetBool("push", false);
                anim.SetBool("andandoParaTras", false);
                anim.SetBool("correndo", false);
                anim.SetBool("rezando", false);
            }

            else if (!interagir.interagir && !pulando && !correndo)
            {
                //print("andando");
                anim.SetBool("pulando", false);
                anim.SetBool("andando", true);
                anim.SetBool("push", false);
                anim.SetBool("andandoParaTras", false);
                anim.SetBool("correndo", false);
                if (!andandoAudio.isPlaying)
                {
                    andandoAudio.clip = andandoArray[Random.Range(0, andandoArray.Length)];
                    andandoAudio.Play();
                }
                anim.SetBool("rezando", false);

            }
            
            else if (!interagir.interagir && !pulando && correndo)
            {
                //print("correndo");
                anim.SetBool("pulando", false);
                anim.SetBool("andando", false);
                anim.SetBool("push", false);
                anim.SetBool("andandoParaTras", false);
                anim.SetBool("correndo", true);

                if (!andandoAudio.isPlaying)
                {
                    andandoAudio.clip = andandoArray[Random.Range(0, andandoArray.Length)];
                    andandoAudio.Play();
                }
                anim.SetBool("rezando", false);
            }

            if (interagir.interagir && !pulando)
            {
                //print("pushParado");
                anim.SetBool("pulando", false);
                anim.SetBool("andando", false);
                anim.SetBool("push", true);
                anim.SetBool("andandoParaTras", false);
                anim.SetBool("correndo", false);
                anim.SetBool("rezando", false);

                if (playerInput.x != 0 || playerInput.z != 0)
                {
                    anim.speed = 2f;

                    if (!arrastaAudio.isPlaying)
                    {
                        arrastaAudio.Play();
                    }
                }
                else
                {
                    anim.speed = 0f;
                    if (arrastaAudio.isPlaying)
                    {
                        arrastaAudio.Stop();
                    }
                }

            }
            else
            {
                anim.speed = 1f;
                if (arrastaAudio.isPlaying)
                {
                    arrastaAudio.Stop();
                }
            }
             if (!interagir.interagir && pulando)
            {
                //print("pulandoAndando");
                anim.SetBool("pulando", true);
                anim.SetBool("andando", false);
                anim.SetBool("push", false);
                anim.SetBool("andandoParaTras", false);
                anim.SetBool("correndo", false);

               
                
                 anim.SetBool("rezando", false);
            }
        }
        else
        {
            //print("rezando");
            anim.SetBool("pulando", false);
            anim.SetBool("andando", false);
            anim.SetBool("push", false);
            anim.SetBool("andandoParaTras", false);
            anim.SetBool("correndo", false);
            anim.SetBool("rezando", true);
        }
        //print(podeSeMovimentar);
    }
    void CamDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    void SetGravity()
    {
        if (Player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
            pulando = false;
            if (pulandoAudio.isPlaying)
            {
                pulandoAudio.Stop();
            }
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
          
        
        }

        slideDown();
    }

    public void PlayerSkills()
    {
        if (Player.isGrounded && Input.GetButton("Jump") && podeSeMovimentar && !interagir.interagir)
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
            pulando = true;

            if (!pulandoAudio.isPlaying)
            {
                pulandoAudio.pitch = Random.Range(0.8f, 1.8f);
                pulandoAudio.Play();
                
            }
        }
    }

    public void slideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= Player.slopeLimit;

        if (isOnSlope)
        {
            movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
            movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;

            movePlayer.y += slopeForceDown;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    public void SpawnPos()
    {
        if (mortes == 0)
        {
            Player.enabled = false;
            gm = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>();
            transform.position = gm.lastCheckPoint; //A primeira posição do jogador sendo o primeiro checkpoint

            if (arrastaAudio.isPlaying)
                arrastaAudio.Stop();

            if (interagir.interagir)
            {
                interagir.soltaObj();
                interagir.two = false;
            }
            Player.enabled = true;
        }
        else
        {
            StartCoroutine(Respawn());
        }
        mortes++;
    }
    IEnumerator Respawn()
    {
       

        Player.enabled = false;
        gm = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>();
        if (arrastaAudio.isPlaying)
            arrastaAudio.Stop();

        if (interagir.interagir)
        {
            interagir.soltaObj();
            interagir.two = false;
        }
        
        Color cor = fade.color;


        while (cor.a < 1 )
        {
          cor.a += Time.deltaTime;

          fade.color = cor;
          
          yield return null;
        }
       
        transform.position = gm.lastCheckPoint;
        podeSeMovimentar = false;
        playerInput.x = 0;
        playerInput.z = 0;
        pulando = false;
        correndo = false;
        index++;
        //text.text = textos[index];
        if (index == textos.Length)
        {
            index = 0;
        }
        fade2.SetActive(true);
        textos[index].SetActive(true);        
        // text.SetActive(true);
        yield return new WaitForSeconds(2);
        fade2.SetActive(false);
        textos[index].SetActive(false);
       
        //   playerInput.x == 0 && playerInput.z == 0 && !interagir.interagir && !pulando && !correndo
        // text.SetActive(false);

        Player.enabled = true;
        podeSeMovimentar = true;
        

        while (cor.a > 0 )
         {
          cor.a -= Time.deltaTime;
         
          fade.color = cor;
          yield return null;
         }

       

    }
    public void animacaoChamada()
    {
        anim.SetBool("pulando", false);
        anim.SetBool("andando", false);
        anim.SetBool("push", false);
        anim.SetBool("andandoParaTras", false);
        anim.SetBool("correndo", false);
        anim.SetBool("rezando", false);
    }

    public void animacaoChamadaReza()
    {
      anim.SetBool("pulando", false);
      anim.SetBool("andando", false);
      anim.SetBool("push", false);
      anim.SetBool("andandoParaTras", false);
      anim.SetBool("correndo", false);
      anim.SetBool("rezando", true);
    }
    //colisao com o coletavel
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Recarga2") && podeSeMovimentar)
        {
            if (!coletavelAudio.isPlaying)
            {
                coletavelAudio.pitch = Random.Range(0.8f, 1.1f);
                coletavelAudio.Play();
            }
            animHud.SetTrigger("Coleta");
            if(ativandoEspirito != null)
            ativandoEspirito.fragmentos++;
            ativaEspirito.maisTempo = true;
            other.gameObject.SetActive(false);

            
        }

        if (other.gameObject.CompareTag("TP"))
        {
            third.controle = true;
        }

        if(other.gameObject.CompareTag("AreaEscura"))
        {
            textNar.chamaIEnumerator();
        }
    }
}
