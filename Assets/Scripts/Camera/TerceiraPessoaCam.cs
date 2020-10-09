using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TerceiraPessoaCam : MonoBehaviour
{

    [SerializeField]
    private float mouseSensitivity;
    
    public float mouseX, mouseY;
    

    [SerializeField]
    private Transform[] target;
    [SerializeField]
    private float distanciaDoAlvo;
    [SerializeField]
    private Vector2 mouseYMinMan;


    [SerializeField]
    private Vector2 mouseYMinSpirit; 
    private float eps;
    //dados da rotação
    [SerializeField]
    private float rotationSmoothTime;
    Vector3 rotationSmoothVelocity;
    
    public Vector3 currentRotation;

    [SerializeField]
    private GameObject objCam;
    public static int current;

    public Vector3 vectorSave;
    public Vector2 vector2;

    public bool nfazpf;

    public bool fazIsso;

    [SerializeField]
    private movimento moviment;
    [SerializeField]
    private ativaEspirito ativa;
    [SerializeField]
    private CameraColision camerAtion;
    [SerializeField]
    private Transform trans;
    [SerializeField]
    private float tempoCam;
    [SerializeField]
    private bool noHub;

    public Vector3 inicial;
    public Vector3 rotInicial;

    public Vector3 final;
    public Vector3 rotFinal;
    public Vector3 posicao;
    ControlaSeletorDeFase controller;

    int index; 

    float timer;
    [SerializeField]
    private float tempoDaAnim;
    bool travel;
    public bool travelBack;
    bool travellingBack;

    bool teste;

    public string nomeCod;

    public Interagir interagir;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Slider slido;

    public bool testeTransicao;
    float index2;

    public int indexCam;
    public Vector3[] posCam;
    public Vector3[] rotCam;
    Vector3 posatual;
    Vector3 rotatual;
    bool travelMidGame;
    public int numeroDeSpots;
    bool voltaPerso;
    float tempoDePause;
    public int valorMaximo;
    [SerializeField]
    private GameObject end;

    [SerializeField]
    private Image fade;
    [SerializeField]
    private Image fade2;
    [SerializeField]
    private Text text;

    [SerializeField]
    private TextosNarrativa terceira;

    [SerializeField]
    private CharacterController chara;

    [SerializeField]
    float valorNormal = 1.25f;

    public Quaternion eulerAngles;

    [SerializeField]
    private Transform cam;

    [SerializeField]
    private bool focaNaDiva;

    [SerializeField]
    private Transform pos2cam;

    [SerializeField]
    private GameObject esferinha;

    private bool moveEsferinha;

    [SerializeField]
    private float tempoEsfera;

    [SerializeField]
    private Vector3 inicioEsfera;

    [SerializeField]
    private Transform ola;

    [SerializeField]
    private movimentacaoBspirit mSpirit;

    [SerializeField]
    private CharacterController chara2;

    [SerializeField]
    private GameObject HUD;

    public AudioSource somVitoria;
    public bool controle;
    public GameObject particulaTP;

    public SceneLoader scene;

 
    public Transform meio;

    public Material skyBox;
    public Material materialArvLevel2;
    public GameObject[] eliminar;

    public Material agua;
    public Renderer[] aguaR;
    public bool Miguel;
    public bool eliminarBool;
    public bool cancelaTransicao;
    public bool mudaAgua;
    public bool arvore;


    private void Awake()
    {
      //  slido = GameObject.Find("GameSensi").GetComponent<Slider>();
        mouseSensitivity = slido.value;
    }
    private void Start()
    {
        current = 0;
        indexCam = 1;
        if (!noHub)
        {
            StartCoroutine(caraio());
        }

        currentRotation = new Vector3(mouseY, mouseX, 0);

        controller = GameObject.Find(nomeCod).GetComponent<ControlaSeletorDeFase>();

        index2 = current;

        mouseSensitivity = PlayerPrefs.GetFloat(("Sensibilidade"));
      //  PlayerPrefs.SetFloat(("Sensibilidade"), slido.value);
        slido.value = PlayerPrefs.GetFloat(("Sensibilidade"));
        scene = GameObject.Find(nomeCod).GetComponent<SceneLoader>();
    //    indexCam = posCam.Length;
    }
    float timer22;
    private void Update()
    {
        if (current == 0)
            mouseSensitivity = slido.value;
        else if (current == 1)
            mouseSensitivity = slido.value / valorNormal;

        Vector3 news3 = target[current].position - transform.forward * distanciaDoAlvo;

        if (!teste)
            transform.position = (target[current].position - transform.forward * distanciaDoAlvo);
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, news3, 20 * Time.deltaTime);
            if (transform.position == news3)
            {
                index = 1;
                teste = false;
            }
        }

        if (moveEsferinha)
        {
            esferinha.transform.localPosition = Vector3.MoveTowards(esferinha.transform.localPosition, inicioEsfera, 20 * Time.deltaTime);
            trans.LookAt(esferinha.transform);
            timer22 += Time.deltaTime;
            print(timer22);
        }

    }
    private void FixedUpdate()
    {
     //   print(trans.eulerAngles);
        if (travel)
        {
                trans.localPosition = Vector3.Lerp(trans.localPosition, final, 2 * Time.deltaTime);
                //     trans.Rotate(Vector3.up * Time.deltaTime);
                trans.eulerAngles = Vector3.Lerp(trans.eulerAngles, rotFinal, Time.deltaTime);
        }
        if(travelBack)
        {
            StartCoroutine(fechamento());
            travelBack = false;
        }
        if(travellingBack && !Miguel)
        {
            trans.position = Vector3.Lerp(trans.position, inicial, 2 * Time.deltaTime);
        //    trans.eulerAngles = Vector3.Lerp(trans.eulerAngles, rotInicial, Time.deltaTime);
        }
        else if(travellingBack && Miguel)
        {
            trans.position = Vector3.Lerp(trans.position, meio.position, 2 * Time.deltaTime);
        }
        if(travelMidGame)
        {
            trans.position = Vector3.MoveTowards(trans.position, posCam[indexCam], 9 * Time.deltaTime);
            trans.eulerAngles = Vector3.MoveTowards(trans.eulerAngles, rotCam[indexCam], 30 * Time.deltaTime);
            
            if (trans.position == posCam[indexCam] && trans.eulerAngles == rotCam[indexCam])
            {
                numeroDeSpots--;
                if (numeroDeSpots > 0)
                    indexCam++;            
            }
        }

        if(voltaPerso)
        {
            trans.localPosition = Vector3.MoveTowards(trans.localPosition, posatual, 3 * Time.deltaTime);
            print(trans.localPosition);
            print(posatual + " ola");
           
            if (trans.localPosition == posatual)
            {
                print("cheguei");
                trans.localEulerAngles = rotatual;
                voltaPerso = false;
            }
        }
    }
    void LateUpdate()
    {
        //  print(target[current].position);
        //  current = Mathf.Clamp(current,0, target.Length);
        if (fazIsso)
        {
            mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            if (current == 0)
            {
                mouseY = Mathf.Clamp(mouseY, mouseYMinMan.x, mouseYMinMan.y);
                index = 0;
            }
            else if (current == 1)
            {
                mouseY = Mathf.Clamp(mouseY, mouseYMinSpirit.x, mouseYMinSpirit.y);

            }

            if (current == 1 && index == 0)
                teste = true;
            //permite a rotação ficar mais suave
            if (!nfazpf)
             currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(mouseY, mouseX), ref rotationSmoothVelocity, rotationSmoothTime);
            else
            {
                if(interagir.interagir)
                currentRotation = interagir.saveRot;
            }
            transform.eulerAngles = currentRotation;

         
           
        }
    }

    public void Sensibilidade()
    {
      //  if(PlayerPrefs.GetFloat("Senbilidade") != slido.value)
        PlayerPrefs.SetFloat(("Sensibilidade"), slido.value);
        mouseSensitivity = PlayerPrefs.GetFloat(("Sensibilidade"));
    }
    IEnumerator caraio()
    {
        trans.position = inicial;
        trans.eulerAngles = rotInicial;
      //  moviment.enabled = false;
        yield return new WaitForSeconds(tempoCam);
        //  transform.localPosition = Vector3.MoveTowards(transform.localPosition, final, 2 * Time.deltaTime);
         travel = true;
         yield return new WaitForSeconds(tempoDaAnim);
         travel = false;
        if (!focaNaDiva)
        {
            HUD.SetActive(true);
            camerAtion.enabled = true;
            moviment.enabled = true;
            fazIsso = true;
            if (ativa != null)
            {
                ativa.enabled = true;

            }
            if (terceira != null)
                terceira.chamaIEnumerator();
        }
        else
        {
            trans.position = pos2cam.position;
            trans.eulerAngles = pos2cam.eulerAngles;
            moveEsferinha = true;
            yield return new WaitForSeconds(tempoEsfera);
            moveEsferinha = false;
            HUD.SetActive(true);
            trans.position = ola.position;
            trans.eulerAngles = ola.eulerAngles;
            chara2.enabled = true;
            mSpirit.enabled = true;
            esferinha.SetActive(false);
            camerAtion.enabled = true;
            moviment.enabled = true;
            fazIsso = true;
            if (ativa != null)
            {
                ativa.enabled = true;

            }
            if (terceira != null)
                terceira.chamaIEnumerator();

        }
    }
    public void camTravel()
    {
        StartCoroutine(camTravelMidGame());
    }

    IEnumerator camTravelMidGame()
    {
        print("primeiraParte");
        yield return new WaitForSeconds(0.1f);
        print("segundaParte");
        fazIsso = false;
        camerAtion.enabled = false;
        moviment.enabled = false;
        anim.enabled = false;
        ativa.enabled = false;
        posatual = trans.localPosition;
        rotatual = trans.localEulerAngles;
        trans.localPosition = posCam[0];
        trans.eulerAngles = rotCam[0];
        
        yield return new WaitForSeconds(1.5f);
        print("terceiraParte");
        indexCam = Mathf.Clamp(indexCam, indexCam, indexCam + numeroDeSpots);
        travelMidGame = true;       
        yield return new WaitForSeconds(8f);
        print("quartaParte");
        travelMidGame = false;
        voltaPerso = true;
        while(voltaPerso)
            yield return null;
        print("quintaParte");
        fazIsso = true;
        camerAtion.enabled = true;
        moviment.enabled = true;
        anim.enabled = true;
        ativa.enabled = true;
    }

    public void introducao()
    {
        StartCoroutine(Introducao());
    }

    public void liberAcoes()
    {
        fazIsso = true;
        chara.enabled = true;
        camerAtion.enabled = true;
        moviment.enabled = true;

    }
    public void cancelaAcoes()
    {
        fazIsso = false;
        chara.enabled = false; 
        camerAtion.enabled = false;
        moviment.enabled = false;
    }
    IEnumerator Introducao()
    {
        Color cor = fade.color;
        Color cor2 = text.color;
        Color cor3 = fade2.color;
        yield return new WaitForSeconds(5f);
        while (cor.a > 0)
        {
            cor.a -= Time.deltaTime;
            cor2.a -= Time.deltaTime;
            cor3.a -= Time.deltaTime;
            fade.color = cor;
            fade2.color = cor3;
            text.color = cor2;
            yield return null;
        }

        fazIsso = true;
        nfazpf = false;
        camerAtion.enabled = true;
        moviment.enabled = true;
        PlayerPrefs.SetInt(("HUB"), PlayerPrefs.GetInt("FasesLiberadas"));

    }
    IEnumerator fechamento()
    {
        if(particulaTP != null)
        particulaTP.SetActive(true);
        while(!controle)
            yield return null;
        
        moviment.podeSeMovimentar = false;
        yield return new WaitForSeconds(1f);
        fazIsso = false;
        camerAtion.enabled = false;
        moviment.animacaoChamada();
       // moviment.enabled = false;
        anim.enabled = false;
        ativa.enabled = false;
        if (!cancelaTransicao)
        {
            trans.localPosition = posicao;
            trans.eulerAngles = new Vector3(90, 0, -90);
        }
        else if(cancelaTransicao)
        {
            trans.position = meio.position;
            trans.eulerAngles = meio.eulerAngles;
        }
        if(arvore)
        {
            materialArvLevel2.SetFloat("_Cutoff", 0.3f);
        }
        if(mudaAgua)
        {
            for(int i = 0; i < aguaR.Length; i++)
            {
                aguaR[i].material = agua;
            }
        }
        if (Miguel)
        {
            RenderSettings.skybox = skyBox;
        }
        if (eliminarBool)
        {
            for (int i = 0; i < eliminar.Length; i++)
            {
                eliminar[i].SetActive(false);
            }
        }
        yield return new WaitForSeconds(2f);
        if (!cancelaTransicao)
        {
            travellingBack = true;
        }
        controller.nextSceneFunction();
        print("eu");
        yield return new WaitForSeconds(tempoDaAnim + 1f);
        end.SetActive(true);
        if (!somVitoria.isPlaying)
        {
            somVitoria.Play();
        }
        
        yield return new WaitForSeconds(2.5f);
        //SceneManager.LoadScene(1);
        scene.chamaFunction(1);
      
    }
}
