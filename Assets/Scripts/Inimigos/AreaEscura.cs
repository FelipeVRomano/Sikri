using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEscura : MonoBehaviour
{
    [SerializeField]
    private Ativavel[] ativaveis;
    [SerializeField]
    private int filhos;

    private BoxCollider box;
    private bool regula;

    //private MeshRenderer mesh;
    [SerializeField]
    private GameObject obj;

    private MeshRenderer mesh;

    [SerializeField]
    private GameObject obj2;
    void Start()
    {
        filhos = ativaveis.Length;
        box = GetComponent<BoxCollider>();
        mesh = GetComponent<MeshRenderer>();
      //  mesh = GetComponent<MeshRenderer>();

        
    }

    // Update is called once per frame
    void Update()
    {

        if (akim(regula))
        {
            box.enabled = false;
            // mesh.enabled = false;
            obj.SetActive(false);
            obj2.SetActive(false);
            mesh.enabled = false;
        }
        
    }
    private bool akim(bool v)
    {
        for (int i = 0; i < ativaveis.Length; i++)
        {
            if(ativaveis[i].ativado == false)
            {
                return false;
            }

        }
        return true;
        
    }
}
