using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransicao : MonoBehaviour
{
    TerceiraPessoaCam terceira;
    [SerializeField]
    private int numDeSpots;
    bool ativou;
    [SerializeField]
    private int valorMaximo;
    [SerializeField]
    private int valorAtual;
    void Start()
    {
        terceira = GameObject.Find("CameraBase").GetComponent<TerceiraPessoaCam>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            terceira.numeroDeSpots = numDeSpots;
            terceira.indexCam = valorAtual;
      //      terceira.valorMaximo = valorMaximo;
            if (!ativou)
            {
                terceira.camTravel();
                ativou = true;
            }
        }
    }
}
