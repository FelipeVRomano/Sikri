using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venceu : MonoBehaviour
{
    public ControlaSeletorDeFase control;

    public bool win;

    public int indiceFase;

    public SceneLoader scene;

    [SerializeField]
    private int sceneint;
    TerceiraPessoaCam cam;

    movimento moviment;
    [SerializeField]
    private GameObject game;

    void Start()
    {
        control = GameObject.Find("SceneLoader").GetComponent<ControlaSeletorDeFase>();
        cam = GameObject.Find("CameraBase").GetComponent<TerceiraPessoaCam>();
        if(!win)
        {
            scene = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
       
        }
      
    }
    public void Scene()
    {
        control.nextSceneFunction();
    }

    public void fimFase()
    {
        cam.travelBack = true;
    }

    IEnumerator escolheCena()
    {
        moviment.podeSeMovimentar = false;
        yield return new WaitForSeconds(1f);
        moviment.animacaoChamadaReza();
        game.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        // scene.LoadScene(sceneint);
        scene.chamaFunction(sceneint);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && win)
        {
            // control.nextSceneFunction();
            cam.travelBack = true;
        }
        else if(other.CompareTag("Player") && !win)
        {
            if(indiceFase < PlayerPrefs.GetInt("FasesLiberadas")) // + 2
            {
                moviment = other.GetComponent<movimento>();
                StartCoroutine(escolheCena());
            }
        }
    }
}
