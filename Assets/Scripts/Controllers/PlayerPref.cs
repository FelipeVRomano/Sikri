using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPref : MonoBehaviour
{
    //Agua
    [SerializeField]
    private Transform agua;

    [SerializeField]
    private Transform[] aguAlt;



    //Coluna
    public Material materialCol;

    [SerializeField]
    private float[] valMaterial;


    //Grama
    public Renderer[] Grama;
    public Material[] materialGrama;


    //Arvore
    public Material materialArvore;

    [SerializeField]
    private float[] alphaCuteoff;

    public float speed;
    public Color startColor;
    public Color endColor;
    float startTime;
    public Material arvoreMaterial;
    //esferaDeLuz
    public GameObject[] esfera;

    public GameObject falas;

    //camera
    [SerializeField]
    private Transform cameraPos;

    //posicoesDaCamera
    [SerializeField]
    private Transform[] posLevel1;


    [SerializeField]
    private TerceiraPessoaCam third;

    //feedbackScene
    [SerializeField]
    private GameObject[] feedbackScenes;
    [SerializeField]
    private GameObject[] textos;
    [SerializeField]
    private SpriteRenderer[] sprite;
   // [SerializeField]
   // private GameObject[] particles;

    [SerializeField]
    private int[] indices;
    private int indiceSprite;
    private bool boolando;
    //player
    [SerializeField]
    private Transform[] spawns;
    [SerializeField]
    private Transform Player;

    //bool
    private bool aguaMove;
    private bool coluna;
    private bool arvore;

    //intIndex
    private int index;


    //floats
    private float moveAgua;
    private float mudaColuna;
    private float mudaArvoreCut;


   // public int teste;

    Vector3 pos;
    Vector3 rot;

    [SerializeField]
    private GameObject inicio;

    private bool altera;

    [SerializeField]
    private GameObject endgame;
    [SerializeField]
    private GameObject creditos;

    [SerializeField]
    private GameObject somChuva;

    [SerializeField]
    private GameObject chuva;

    [SerializeField]
    Material[] sky;

    [SerializeField]
    private GameObject inicioFase0;

    [SerializeField]
    private GameObject fundo;

    [HideInInspector]
    public bool ativaste;

    public GameObject luz1;
    public GameObject luz2;

    void Start()
    {
        
        //PlayerPrefs.SetInt(("FasesLiberadas"), 1);
        //PlayerPrefs.SetInt(("HUB"), 0);
        if(PlayerPrefs.GetInt("HUB") == 0)
        {
            inicioFase0.SetActive(true);
        }
        if(PlayerPrefs.GetInt("HUB") < 3)
        {
            RenderSettings.skybox = sky[0];
            luz1.SetActive(true);
            luz2.SetActive(false);
        }
        else
        {
            RenderSettings.skybox = sky[1];
            luz1.SetActive(false);
            luz2.SetActive(true);
        }
        if (PlayerPrefs.GetInt("HUB") < PlayerPrefs.GetInt("FasesLiberadas"))
        {
            if (PlayerPrefs.GetInt("HUB") == 0)
            {
                inicio.SetActive(true);
                agua.position = aguAlt[0].position;
                materialCol.SetFloat("_BumpScale", valMaterial[0]);
                materialArvore.SetFloat("_Cutoff", alphaCuteoff[0]);
            }
            else
            {
                agua.position = aguAlt[PlayerPrefs.GetInt("HUB") - 1].position;
                materialCol.SetFloat("_BumpScale", valMaterial[PlayerPrefs.GetInt("HUB") - 1]);
                materialArvore.SetFloat("_Cutoff", alphaCuteoff[PlayerPrefs.GetInt("HUB") - 1]);
            }
            if (PlayerPrefs.GetInt("HUB") == 0)
            {
                StartCoroutine(fase0());
            }
            else if (PlayerPrefs.GetInt("HUB") > 0)
            {
                StartCoroutine(fase1());
            }
        }
        else
        {
            agua.position = aguAlt[PlayerPrefs.GetInt("HUB")-1].position;
            materialCol.SetFloat("_BumpScale", valMaterial[PlayerPrefs.GetInt("HUB")-1]);
            materialArvore.SetFloat("_Cutoff", alphaCuteoff[PlayerPrefs.GetInt("HUB")-1]);
            indiceSprite = indices[PlayerPrefs.GetInt("HUB") - 1];
            for (int i = 0; i <= indiceSprite ; i++)
            {
              //  particles[i].SetActive(true);
                sprite[i].color = Color.white;
            }
            fundo.SetActive(false);
            for (int i = 0; i < PlayerPrefs.GetInt("HUB"); i++)
            {
                feedbackScenes[i].SetActive(true);
            }
            if (PlayerPrefs.GetInt("HUB") < 4)
            {
                arvoreMaterial.color = startColor;
                for (int i = 0; i < Grama.Length; i++)
                {
                    Grama[i].material = materialGrama[0];
                }
            }
            else
            {
                arvoreMaterial.SetFloat("_Cutoff", alphaCuteoff[PlayerPrefs.GetInt("HUB") - 1]);
                arvoreMaterial.color = endColor;
                for (int i = 0; i < Grama.Length; i++)
                {
                    Grama[i].material = materialGrama[1];
                }
            }
            third.cancelaAcoes();
            Player.position = spawns[PlayerPrefs.GetInt("HUB")-1].position;
            if (PlayerPrefs.GetInt("FasesLiberadas") == 4)
            {
                Player.eulerAngles = new Vector3(0, 0, 0);
                third.mouseX = 0;
            }
            third.liberAcoes();
        }
        startTime = Time.time;

    }
    private void Update()
    {
        if (altera)
        {
            print("ola");

            print(alphaCuteoff[PlayerPrefs.GetInt("HUB") - 1]);
           // altera = false;
        }
        if (aguaMove)
        {
            agua.position = Vector3.MoveTowards(agua.position, new Vector3(agua.position.x, aguAlt[index].position.y, agua.position.z), 1 * Time.deltaTime);
        }
        if (coluna)
        {
            materialCol.SetFloat("_BumpScale", mudaColuna);
            if (mudaColuna > valMaterial[index])
            {
                mudaColuna -= Time.deltaTime / 5;
            }

        }
        if(boolando)
        {
            for(int i = 0; i <= indiceSprite; i++)
            {
              //  particles[i].SetActive(true);
                float t = (Time.time - startTime) * speed;
                sprite[i].color = Color.Lerp(sprite[i].color, Color.white , t);
            }
        }
        if (arvore)
        {
            materialArvore.SetFloat("_Cutoff", mudaArvoreCut);
            if (mudaArvoreCut > alphaCuteoff[index])
                mudaArvoreCut -= Time.deltaTime / 7;
            if(PlayerPrefs.GetInt("FasesLiberadas") == 4)
            {
                float t = (Time.time - startTime) * speed;
                arvoreMaterial.color = Color.Lerp(startColor, endColor, t);

            }

        }
    }
    IEnumerator fase0()
    {
        esfera[0].SetActive(true);
        textos[0].SetActive(true);
        textos[1].SetActive(true);
        third.introducao();
        yield return null;
    }
    IEnumerator fase1()
    {
        index = (PlayerPrefs.GetInt("HUB"));
        if(index >= 3)
        {
            for (int i = 0; i < Grama.Length; i++)
            {
                Grama[i].material = materialGrama[1];
            }
        }
 //       feedbackScenes[index - 1].SetActive(true);
        for (int i = 0; i <= index; i++)
        {
            feedbackScenes[i].SetActive(true);
        }
        esfera[index].SetActive(true);
        if(index == 0)
        {
            textos[0].SetActive(true);
            textos[1].SetActive(true);
        }
        else if(index == 1)
        {
            textos[2].SetActive(true);
            textos[3].SetActive(true);
        }
        else if(index == 2)
        {
            textos[4].SetActive(true);
            textos[5].SetActive(true);
        }
        else if(index == 3)
        {
            textos[6].SetActive(true);
            textos[7].SetActive(true);
            //somChuva.SetActive(true);
            //chuva.SetActive(true);
        }
    //    txt[index].SetActive(true);
        third.cancelaAcoes();
        Player.position = spawns[index].position;
        if (PlayerPrefs.GetInt("FasesLiberadas") == 4)
            Player.eulerAngles = new Vector3(0, 0, 0);
        pos = cameraPos.localPosition;
        rot = cameraPos.localEulerAngles;
        cameraPos.position = posLevel1[0].position;
        cameraPos.eulerAngles = posLevel1[0].eulerAngles;
        aguaMove = true;
        yield return new WaitForSeconds(3f);
        if (index == 3)
            chuva.SetActive(false);
        aguaMove = false;
        cameraPos.position = posLevel1[1].position;
        cameraPos.eulerAngles = posLevel1[1].eulerAngles;
        if (index == 0)
            mudaColuna = valMaterial[0];
        else
        {
            mudaColuna = valMaterial[index - 1];
        }
        coluna = true;
        yield return new WaitForSeconds(3f);
        coluna = false;
        indiceSprite = indices[index];
        cameraPos.position = posLevel1[3].position;
        cameraPos.eulerAngles = posLevel1[3].eulerAngles;
        boolando = true; 
        yield return new WaitForSeconds(3f);
        boolando = false;
        //if(index == 3)
            //chuva.SetActive(true);
        cameraPos.position = posLevel1[2].position;
        cameraPos.eulerAngles = posLevel1[2].eulerAngles;
        if (index == 0)
            mudaArvoreCut = alphaCuteoff[0];
        else
        {
            mudaArvoreCut = alphaCuteoff[index - 1];
        }
        arvore = true;
        yield return new WaitForSeconds(4f);

        PlayerPrefs.SetInt(("HUB"), PlayerPrefs.GetInt("FasesLiberadas"));
        if (PlayerPrefs.GetInt("HUB") < 4)
        {
            cameraPos.localPosition = pos;
            cameraPos.localEulerAngles = rot;
            third.liberAcoes();
        }
        else
        {
            cameraPos.localPosition = pos;
            cameraPos.localEulerAngles = rot;
            third.liberAcoes();
            somChuva.SetActive(false);
            chuva.SetActive(false);
            do
            {
                yield return null;
            }
            while (!ativaste);
                third.cancelaAcoes();
                endgame.SetActive(true);
                somChuva.SetActive(true);
                cameraPos.position = posLevel1[2].position;
                yield return new WaitForSeconds(3f);
                chuva.SetActive(true);
                cameraPos.localEulerAngles = new Vector3(rot.x, 0, rot.z);
                endgame.SetActive(false);
                creditos.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
        }
    }

    public void crebiro()
    {
        third.mouseX = 0;
        chuva.SetActive(false);
        cameraPos.localPosition = pos;
        cameraPos.localEulerAngles = rot;
        third.liberAcoes();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
 
}
