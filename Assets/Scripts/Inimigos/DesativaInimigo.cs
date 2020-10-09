using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesativaInimigo : MonoBehaviour
{
    [SerializeField]
    private SphereCollider box;
    [SerializeField]
    private GameObject baseLuz;
    [SerializeField]
    private GameObject baseFogo;
    [SerializeField]
    private InimigoCorrompido enemy;
    [SerializeField]
    private GameObject baseIntera2;

    [SerializeField]
    private bool reativavel = true;

    private bool timer;
    private float tim;

    [SerializeField]
    private float tempoParaVoltar;

    movimentacaoBspirit moveSpirit;
    private void Update()

    {
        if(timer)
        {
            tim += Time.deltaTime;
        }

        if(tim >= tempoParaVoltar)
        {
            reativavel = true;
            box.enabled = true;
            baseFogo.SetActive(false);
            baseLuz.SetActive(true);
            enemy.funciona = true;
            tim = 0;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Purificador"))
        {
            moveSpirit = GameObject.Find("Spirit").GetComponent<movimentacaoBspirit>();            
            box.enabled = false;
            baseLuz.SetActive(false);

            if (baseIntera2 != null)
            {
                baseIntera2.SetActive(false);
            }

            baseFogo.SetActive(true);
            enemy.funciona = false;
            moveSpirit.Velocity();
            print("Ola");

            if(reativavel)
            {
                timer = true;
                reativavel = false;
            }
        }
    }


}
