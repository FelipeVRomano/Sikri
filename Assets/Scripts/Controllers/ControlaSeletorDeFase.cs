using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ControlaSeletorDeFase : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] fases;

    [SerializeField]
    private bool menu;

    public int nextScene;

    [SerializeField]
    private int hub;

    private SceneLoader load;
    void Start()
    {
        if (menu)
        {
            for (int i = 0; i <= fases.Length - 1; i++)
            {
                if (PlayerPrefs.GetInt("FasesLiberadas") < i)
                    fases[i].material.color = Color.red;
                else
                {
                    fases[i].material.color = Color.green;
                }
            }
        }
        load = GetComponent<SceneLoader>();
       

    }
    public void nextSceneFunction()
    {
        if (PlayerPrefs.GetInt("FasesLiberadas") < nextScene)
            PlayerPrefs.SetInt(("FasesLiberadas"), nextScene);
     //   load.LoadScene(hub);
    }

}
