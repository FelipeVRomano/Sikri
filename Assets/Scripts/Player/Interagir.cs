using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interagir : MonoBehaviour
{

    public GameObject objMovel;
    private bool controlaObj;
    [SerializeField]
    private GameObject posFixa2;
    [SerializeField]
    private GameObject visao;
    private bool colidindo;
    private Rigidbody objMovelFis;
    public bool interagir;

    private CharacterController box;
    [SerializeField]
    private BoxCollider boxObj;
    public movimento moveplayer;

    private bool one;
    public bool two;

    private GameObject childBolinhaBranca;
   // public GameObject child2Arrastando;

    private MoveWithFloor move;
    [SerializeField]
    private Vector3 triggerBox;
    private Vector3 triggerBoxRot;

    [SerializeField]
    private TerceiraPessoaCam terceira;
    public Transform cam;
    public Vector3 saveRot;

    Vector3 pos;
    bool muda;
    void Start()
    {
        box = GetComponent<CharacterController>();
        moveplayer = FindObjectOfType<movimento>();
    }

   
    void Update()
    {
        if(muda)
        {
            box.enabled = false;
            transform.position = pos;
            transform.eulerAngles = saveRot;
            objMovel.transform.eulerAngles = saveRot;
            box.enabled = true;
            muda = false;
        }
        
        //solta o objeto
        if (one)
        {
            if(Input.GetButtonUp("Interagir"))
            {
                two = true;
                one = false;
            }
        }
        if (two)
        {
            if (objMovel != null && Input.GetButtonDown("Interagir"))
            {
                soltaObj();
                two = false;
            }
        }
        //defini se ele está vendo o objeto ou não
        if (Physics.Raycast(visao.transform.position, transform.forward, 3))
            colidindo = true;
        else
        {
            colidindo = false;
        }
     //   print(interagir);
    }

    public void soltaObj()
    {
        if (objMovel != null)
        {
            terceira.nfazpf = false;
            terceira.mouseX = saveRot.y;
            terceira.mouseY = saveRot.x;
            //ativaTexto
            //       objMovel.transform.GetChild(0).gameObject.SetActive(true);
            interagir = false;
            //mudaTamanhoDoCharacterController
            box.radius = 0.4f;
            box.center = new Vector3(0f, 1.51f, 0f);
            box.height = 2.9f;
            //ativaBoxCollider, desativa Kinematic e passa um valor null para elas não interferirem mais
            move.enabled = true;
            move = null;
            boxObj.enabled = true;
            boxObj = null;
            objMovelFis.isKinematic = false;
            objMovelFis = null;
            //obj não é mais filho e tira o objeto do código para abrir espaço para outro
            objMovel.transform.parent = null;
            childBolinhaBranca.SetActive(true);
            childBolinhaBranca = null;
           // child2Arrastando = null;
            objMovel = null;
        }
    }
    public void soltaObjDeVez()
    {
        if (objMovel != null)
        {
            terceira.nfazpf = false;
            terceira.mouseX = saveRot.y;
            terceira.mouseY = saveRot.x;
            interagir = false;
            box.radius = 0.4f;
            box.center = new Vector3(0f, 1.51f, 0f);
            box.height = 2.9f;
            //ativaBoxCollider, desativa Kinematic e passa um valor null para elas não interferirem mais
            move = null;
            boxObj = null;
            //obj não é mais filho e tira o objeto do código para abrir espaço para outro
            
            childBolinhaBranca.SetActive(true);
            childBolinhaBranca = null;
         //   child2Arrastando = null;        
            two = false;
            objMovel.tag = "Untagged";
            objMovel.transform.parent = null;
            objMovel = null;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        //colisão só com o interagível
        if (collision.gameObject.CompareTag("Interagir"))
        {
            if(Input.GetButtonDown("Interagir"))
            {
                collision.gameObject.SetActive(false);
            }
        }
        //colisão com o móvel
        if (collision.gameObject.CompareTag("Move"))
        {
            triggerBox = collision.transform.position;
            if (Input.GetButtonDown("Interagir") && objMovel == null && colidindo)
            {
                if (moveplayer.arrastaAudio.isPlaying)
                {
                    moveplayer.arrastaAudio.Stop();
                }
                //   transform.position = new Vector3(triggerBox.x, transform.position.y, triggerBox.z);
                //pega o objeto da colisão e passa o valor dele para uma variavel do código
                pos = new Vector3(triggerBox.x, transform.position.y, triggerBox.z);
                objMovel = collision.transform.parent.gameObject;
                terceira.nfazpf = true;
                cam.transform.eulerAngles = collision.transform.eulerAngles;
                saveRot = collision.transform.eulerAngles;
                muda = true;
                childBolinhaBranca = objMovel.transform.GetChild(0).gameObject;
                move = objMovel.GetComponent<MoveWithFloor>();
                move.enabled = false;
                childBolinhaBranca.SetActive(false);
              //  child2Arrastando = objMovel.transform.GetChild(1).gameObject;
                objMovel.transform.parent = gameObject.transform;
                objMovel.transform.position = posFixa2.transform.position;
                // pega e tira a box collider e altera o tamanho do Character Controller
                boxObj = objMovel.gameObject.GetComponent<BoxCollider>();
                boxObj.enabled = false;
                box.radius = 0.9f;
                box.center = new Vector3(0f, 1.4f, 1.25f);
                box.height = 2.5f;
                // pega o Rigidbody do objeto e deixa ele em kinematic
                objMovelFis = objMovel.gameObject.GetComponent<Rigidbody>(); 
                objMovelFis.isKinematic = true;    
                interagir = true;
                // desativa o texto com o nome
            //    objMovel.transform.GetChild(0).gameObject.SetActive(false);
                one = true;
            }
        }

    }
    private void OnTriggerStay(Collider collision)
    {
        //colisão só com o interagível
        if (collision.gameObject.CompareTag("Interagir"))
        {
            if (Input.GetButtonDown("Interagir"))
            {
                collision.gameObject.SetActive(false);
            }
        }
        //colisão com o móvel
        if (collision.gameObject.CompareTag("Move"))
        {
            if (Input.GetButtonDown("Interagir") && objMovel == null && colidindo)
            {
                //pega o objeto da colisão e passa o valor dele para uma variavel do código
                pos = new Vector3(triggerBox.x, transform.position.y, triggerBox.z);
                objMovel = collision.transform.parent.gameObject;
                terceira.nfazpf = true;
                cam.transform.eulerAngles = collision.transform.eulerAngles;
                saveRot = collision.transform.eulerAngles;
                muda = true;
                move = objMovel.GetComponent<MoveWithFloor>();
                move.enabled = false;
                childBolinhaBranca = objMovel.transform.GetChild(0).gameObject;
                childBolinhaBranca.SetActive(false);
             //   child2Arrastando = objMovel.transform.GetChild(1).gameObject;
                objMovel.transform.parent = gameObject.transform;
                objMovel.transform.position = posFixa2.transform.position;
                // pega e tira a box collider e altera o tamanho do Character Controller
                boxObj = objMovel.gameObject.GetComponent<BoxCollider>();
                box.radius = 0.9f;
                box.center = new Vector3(0f, 1.4f, 1.25f);
                box.height = 2.5f;
                // pega o Rigidbody do objeto e deixa ele em kinematic
                objMovelFis = objMovel.gameObject.GetComponent<Rigidbody>();  
               objMovelFis.isKinematic = true;
                interagir = true;
                // desativa o texto com o nome
             //   objMovel.transform.GetChild(0).gameObject.SetActive(false);
                one = true;
            }         
        }
    }
}
