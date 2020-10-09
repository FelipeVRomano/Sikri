using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ativaEspirito : MonoBehaviour
{
    [SerializeField]
    private TerceiraPessoaCam terceira;
    [SerializeField]
    private GameObject objCam;

    [SerializeField]
    private GameObject espirito;
    [SerializeField]
    private Transform espiritoTrans;
    [SerializeField]
    private Transform spawnPos;
    [HideInInspector]
    public bool ativouEsfera;
    private GameObject Player;

    //pega o valor da rotacao do player
    private Transform playerRot;
    //relacionadas a interface e controle do tempo ativo da habilidade
    [SerializeField]
    private Image timeBar;

    [HideInInspector]
    public float timeVar;
    public float timeFixo;
    private bool tiraEnergia;


    //Contador de Fragmentos de Fé 
    public  int fragmentos;
    public Text txtFragmentos;

    //controladores da energia 
    public static bool maisTempo;
    public static bool podeMaisTempo;
    public static bool adcTempo;

    private CharacterController character;

    [SerializeField]
    private Transform spawn2;
    private bool colide;
    public int index;

    private movimento controlaB;
    [HideInInspector]
    public bool retiradorDeEnergia;


    //controle
    [HideInInspector]
    public bool testando;

    [SerializeField]
    private int cena;

    public SceneLoader scene;

    public bool level3;

    Interagir interagir;

    public Animator animHud;
    bool estaCheckpoint;
    bool estaLevaDano;


    
    TextosNarrativa texto;
    public string numFala;
    [SerializeField]
    bool controleText;

 
    [Header("Leva Dano")]
    public float danoEscuridao = 7;

    void Start()
    {
        
        ativouEsfera = true;
        Player = this.gameObject;
        playerRot = GetComponent<Transform>();
        timeVar = timeFixo/ 2f;
        character = GetComponent<CharacterController>();
        controlaB = GetComponent<movimento>();
        fragmentos = 0;

        scene = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        interagir = GetComponent<Interagir>();
        if(controleText)
        texto = GameObject.Find(numFala).GetComponent<TextosNarrativa>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!estaCheckpoint)
        {
            animHud.SetBool("Checkpoint", false);
        }

        txtFragmentos.text = "" + fragmentos;

        if(fragmentos > 0 && controleText)
        {
            texto.chamaIEnumerator();
            controleText = false;
        }
            timeBar.fillAmount = timeVar / timeFixo;
        timeVar = Mathf.Clamp(timeVar, 0, timeFixo);

        //ativa a habilidade
        if (character.isGrounded || !ativouEsfera)
        {
            if (Input.GetButtonDown("Spirit") && timeVar > 0)
            {
                if (ativouEsfera && !testando)
                {
                    ativado();                 
                }
                else if (!ativouEsfera && testando && !level3)
                {
                    desativado();
                }
            }
        }
        if (testando)
            controlaB.podeSeMovimentar = false;
        //controla se acabar a energia, desativa a habilidade
        if (tiraEnergia && !adcTempo)
            timeVar -= Time.deltaTime;
        if (timeVar <= 0 && testando)
        {           
            if (level3)
            {
                timeVar = timeFixo;
                controlaB.SpawnPos();
            }
            desativado();
        }

        //para adicionar o tempo pelo spirit e personagem - COLETAVEL
        if (timeFixo != timeVar)
            podeMaisTempo = true;
        if (maisTempo)
        {
            // depois do / é possível mudar a velocidade, quanto mais alto o valor menor a velocidade para adicionar
            timeVar += timeFixo / 7f;
            maisTempo = false;
        }
        if (timeFixo == timeVar)
        {
           // maisTempo = false;
            podeMaisTempo = false;
            adcTempo = false;
        }
        //para adicionar o tempo só pelo personagem gradualmente
        if (adcTempo)
            timeVar += Time.deltaTime * 3f;

        if (Physics.Raycast(transform.position, transform.right, 3))
            colide = true;
        else
        {
            colide = false;
        }

        if (retiradorDeEnergia)
        {
            timeVar -= Time.deltaTime;
            if (!estaLevaDano)
            {
                animHud.SetBool("LevaDano2", true);
                print(estaLevaDano);
                animHud.SetTrigger("LevaDano");
                estaLevaDano = true;
            }
        }
        else
        {
            if (estaLevaDano)
            {
                animHud.SetBool("LevaDano2", false);
                estaLevaDano = false;
                
            }
            
        }
      
    }
    public void RetiraVida()
    {
        timeVar -= timeFixo / danoEscuridao;
        if (!estaLevaDano)
        {
            animHud.SetBool("LevaDano2", true);
            print(estaLevaDano);
            animHud.SetTrigger("LevaDano");
            estaLevaDano = true;
        }
        //print("fiz");
    }
    void ativado()
    {
        animHud.SetTrigger("AtivaFe");
        animHud.SetBool("FormaEspirito", true);

        //desativa a camera do player, o script de movimentacao do jogador e ativar o espito e as booleans para controle de tempo.
        terceira.nfazpf = false;
       // terceira.eulerAngles = objCam.transform.rotation;
        controlaB.podeSeMovimentar = false;
        terceira.vectorSave = objCam.transform.position;
        espirito.SetActive(true);
        espiritoTrans.eulerAngles = playerRot.eulerAngles;
        terceira.vector2 = new Vector2(terceira.mouseX, terceira.mouseY);
        if (!colide) espiritoTrans.position = spawnPos.position;
        else
        {
            espiritoTrans.position = spawn2.position;
        }
        espirito.SetActive(true);
        TerceiraPessoaCam.current = 1;
        tiraEnergia = true;
        ativouEsfera = false;

        testando = true;
    }
   
    //ativa a camera do player, o script de movimentacao do jogador e desativa o espito e as booleans para controle de tempo.
    public void desativado()
    {
        print("desativado");
        animHud.SetTrigger("DesativaFe");
        animHud.SetBool("FormaEspirito", false);
        retiradorDeEnergia = false;
        controlaB.controlaSePodeMovimentar = true;
        espirito.SetActive(false);
        espiritoTrans.position = spawnPos.position;
        espiritoTrans.eulerAngles = spawnPos.eulerAngles;
        terceira.nfazpf = true;
        objCam.transform.position = terceira.vectorSave;
        //  objCam.transform.rotation = terceira.eulerAngles;
      //  terceira.fazIsso = false;
       // terceira.mouseX = terceira.vector2.x;
       // terceira.mouseY = terceira.vector2.y;
       // terceira.currentRotation = new Vector3(terceira.mouseY, terceira.mouseX, 0);
        TerceiraPessoaCam.current = 0;
      //  terceira.fazIsso = true;
        if (!interagir.interagir)
        {
            terceira.nfazpf = false;
        }
        else
        {
            interagir.cam.transform.eulerAngles = interagir.saveRot;
            terceira.nfazpf = true;
        }
        tiraEnergia = false;
        ativouEsfera = true;

        //
        testando = false;
    }

    
    //luz fixa
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Recarga") && podeMaisTempo)
        {
            adcTempo = true;        
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(ativouEsfera)
        {
            if (other.gameObject.CompareTag("Recarga") )
            {

                if (!estaCheckpoint)
                {
                                      
                    estaCheckpoint = true;
                    animHud.SetTrigger("Checkpoint2");
                }

                if (podeMaisTempo)
                {
                    adcTempo = true;
                }                
            }
        }
        else
        {
            adcTempo = false;
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Recarga"))
        {
            if (estaCheckpoint)
            {
               animHud.SetBool("Checkpoint", true);
                Invoke("CheckpointDelay", 0.5f);
            }
            adcTempo = false;
        }              
    }

    void CheckpointDelay()
    {
        estaCheckpoint = false;
    }

    public void CheatEspirito()
    {
        timeVar = 100000000;
        timeFixo = 100000000;
    }
}
