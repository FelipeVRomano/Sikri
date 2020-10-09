using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class movimentacaoBspirit : MonoBehaviour
{
    private CharacterController controller;

    private float horizontalMove;
    private float verticalMove;

   
    [SerializeField]
    private float sensitivity;


    [SerializeField]
    private Transform cameraT;
    private float turnSmoothTime;
    private float speedBase;

    DetectaPesoPlaca placa;
    List<InimigoCorrompido> corrompidos;
   
    public ativaEspirito ativandoEspirito;

    [Header("Efeitos Sonoros")]
    public AudioSource coletavelAudio;

    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    public float PlayerSpeed;
    private Vector3 movePlayer;
    private Vector3 playerInput;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        corrompidos = new List<InimigoCorrompido>();
        //faz ignorar a colisão ser ignorada (foi feito desse jeito para não ter mexer com as layers por enquanto
        Physics.IgnoreCollision(transform.parent.GetComponent<Collider>(), gameObject.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(transform.parent.GetComponent<CharacterController>(), gameObject.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(transform.parent.GetComponent<Collider>(), gameObject.GetComponent<CharacterController>(), true);
        Physics.IgnoreCollision(transform.parent.GetComponent<CharacterController>(), gameObject.GetComponent<CharacterController>(), true);

        speedBase = PlayerSpeed;
    }
    void Update()
    {
    // horizontalMove = Input.GetAxis("Horizontal");
     verticalMove = Input.GetAxis("Vertical");

      CamDirection();

      //playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = new Vector3(0, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer = movePlayer * PlayerSpeed;

        controller.Move(movePlayer * Time.deltaTime);


    }

    void CamDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }
    //colisão com o coletável
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Recarga2"))
        {
            if (!coletavelAudio.isPlaying)
            {
                coletavelAudio.pitch = Random.Range(0.8f, 1.1f);
                coletavelAudio.Play();
            }

            ativandoEspirito.fragmentos++;
            ativaEspirito.maisTempo = true;
            other.gameObject.SetActive(false);

           
        }
        if (other.gameObject.CompareTag("Corrompido"))
        {
            PlayerSpeed = PlayerSpeed / 2;
            corrompidos.Add(other.GetComponent<InimigoCorrompido>());
        }

        if (other.gameObject.CompareTag("Peso"))
        {
            placa = other.GetComponent<DetectaPesoPlaca>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Corrompido"))
        {
            PlayerSpeed = speedBase;
        }
    }

    public void Velocity()
    {
        PlayerSpeed = speedBase;
    }

    void OnDisable()
    {
        PlayerSpeed = speedBase;
        if(placa != null)
        placa.abrindo = false;
        placa = null;
        if (corrompidos != null)
        {
            for (int i = 0; i < corrompidos.Count; i++)
            {
                corrompidos[i].estaColidindoSpirit = false;
                corrompidos[i].paraSom();
                corrompidos[i].encontrado = false;
                corrompidos[i].playerControl = null;
            }
        }
        if(corrompidos != null)
        corrompidos.Clear();
    }
}
