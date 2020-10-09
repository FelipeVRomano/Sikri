using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextoHistoria : MonoBehaviour
{
    Text descricao;
    Camera mainCamera;
    bool estAqui;
    [SerializeField]
    private GameObject image;

    textBaseHistoria selecao;

    ativaEspirito ativa;
    // Start is called before the first frame update
    void Start()
    {
        descricao = GameObject.Find("Texto História").GetComponent<Text>();
        mainCamera = GetComponent<Camera>();

        ativa = GetComponent<ativaEspirito>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(ativa.testando)
        {
            desativa();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("textao"))
        {
            selecao = other.GetComponent<textBaseHistoria>();
            if (selecao != null)
            {
                image.SetActive(true);
                if (selecao.interagivel)
                {
                    if (!estAqui)
                    {
                        descricao.text = "Aperte F para Interagir";
                        estAqui = true;
                    }
                    if (Input.GetButtonDown("Interagir"))
                        descricao.text = selecao.texto;
                }
                else
                {
                    descricao.text = selecao.texto;

                }

            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("textao"))
        {
            if(selecao == null && !ativa.testando)
            selecao = other.GetComponent<textBaseHistoria>();

            if (selecao != null)
            {
                image.SetActive(true);
                if (selecao.interagivel)
                {
                    if (!estAqui)
                    {
                        descricao.text = "Aperte F para Interagir";
                        estAqui = true;
                    }
                }
                else
                {
                    descricao.text = selecao.texto;

                }

            }
        }
    }

    public void desativa()
    {
        selecao = null;
        descricao.text = "";
        image.SetActive(false);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("textao"))
        {
            if (selecao.destrutivel)
            {

                other.gameObject.SetActive(false);
                selecao = null;
            }
            descricao.text = "";
            image.SetActive(false);
        }
    }
}
