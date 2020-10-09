using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Morreu : MonoBehaviour
{
    private GameController gm;
    public movimento playerPos;
    private ativaEspirito vidas;
    private Interagir interagir;
    public static bool morreu;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<movimento>();
        vidas = GameObject.FindGameObjectWithTag("Player").GetComponent<ativaEspirito>();
        interagir = GameObject.FindGameObjectWithTag("Player").GetComponent<Interagir>();
        
    } 

        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
          
            morreu = true;
            vidas.desativado();
            interagir.soltaObj();
            interagir.two = false;
            playerPos.SpawnPos();
            vidas.timeVar = vidas.timeFixo;          
            //SceneManager.LoadScene(1);           
        }
    }
}
