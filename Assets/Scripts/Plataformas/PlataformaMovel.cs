using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovel : MonoBehaviour
{
    public Vector3[] pontos;
    public int pontoIndex;
    private Vector3 alvoAtual;

    //chegar lentamente ao alvo
    public float tolerance;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float delayMovimento;

    private float delayStart;
    public bool automatic;

   
    private Vector3 movimentaObj;
    [SerializeField]
    private CharacterController player;
    private movimentacaoB playerCod;
    [SerializeField]
    private bool ignoraPlayer;

    [SerializeField]
    private bool moVertical;

    public float teste;

    public Interagir Interagir;

    [SerializeField]
    private float speedPlayer;

    private GameObject Player;

    [SerializeField]
    private GameObject filho;
    public AudioSource platMovelInico;
    public AudioSource platMovelFinal;
    void Start()
    {
        if(pontos.Length > 0)
        {
            alvoAtual = pontos[0];
        }
        tolerance = speed * Time.deltaTime;

        Interagir = GameObject.Find("Player").GetComponent<Interagir>();
        Player = GameObject.Find("Player").GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position != alvoAtual)
        {
            MovePlataforma();
        }
        else
        {
            TrocaAlvo();
        }

        if(automatic)
        {
            if(filho != null)
            filho.SetActive(false);
        }
        
    }
    // move a plataforma (e o player se ele estiver na plataforma) e a velocidade vai diminuindo até chegar no ponto final, quando o ponto final é alcançado o tempo de delay é iniciado
    void MovePlataforma()
    {
        if (platMovelInico != null)
        {
            if (!platMovelInico.isPlaying)
            {
                platMovelInico.Play();
                platMovelFinal.Stop();
            }
        }
        Vector3 direcao = alvoAtual - transform.position;
        // Vector3.magnitude = Retorna o comprimento desse vetor(x* x+y * y + z * z)
        transform.position += (direcao / direcao.magnitude) * speed * Time.deltaTime;
        movimentaObj = (direcao / direcao.magnitude) * speed * Time.deltaTime;
        if (player != null)
        {
          //  player.Move(movimentaObj);
          //  player.isGrounded = true;
        }
        if (direcao.magnitude < tolerance)
        {
            transform.position = alvoAtual;
            delayStart = Time.time;

            if (platMovelFinal != null)
            {
                if (!platMovelFinal.isPlaying)
                {
                    platMovelFinal.Play();
                    platMovelInico.Stop();
                }
            }
        }

    }
    // o void automatic deve ser só ligado, se a plataforma for fazer mais de uma viagem 
    void TrocaAlvo()
    {
        if(automatic)
        {
            if(Time.time - delayStart > delayMovimento)
            {
                novoAlvo();
            }
        }
    }
    // troca o alvo
    void novoAlvo()
    {
        pontoIndex++;
        if(pontoIndex >= pontos.Length)
        {
            pontoIndex = 0;
        }

        alvoAtual = pontos[pontoIndex];
    }
    //qdo colide com o player, ele pega o character controller para movimentar o player 
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           // if (!ignoraPlayer)
                player = other.GetComponent<CharacterController>();
            
        }
        //ativa a plataforma móvel com a colisão
        if(other.gameObject.CompareTag("Purificador"))
        {
            automatic = true;
        }
       
    }

    //serve como controle, quem manda nos objetos deixados na plataforma. Se o player soltar a caixa, ela gruda na plataforma.
    private void OnTriggerStay(Collider other)
    {
     //   if (other.transform.paren && other.CompareTag("Move") && !Interagir.interagir)
     //   {
     //       other.transform.parent = transform;
     //   }
    }
    private void OnTriggerExit(Collider other)
    {
       if(other.CompareTag("Player"))
        { 
            playerCod = null;
            player = null;
        }
     //  if(other.CompareTag("Move"))
      //  {
      //      other.transform.parent = null;
      //  }
       
    }

}
