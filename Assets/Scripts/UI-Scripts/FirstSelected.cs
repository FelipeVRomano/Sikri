using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FirstSelected : MonoBehaviour
{
    [SerializeField]
    private EventSystem first;
    private Button botao;

    public string nameEvent;
    
  
    void Awake()
    {
        first = GameObject.Find(nameEvent).GetComponent<EventSystem>();
        first.enabled = false;
        botao = GetComponent<Button>();
    }

    private void OnEnable()
    {
        first.firstSelectedGameObject = gameObject;
        first.enabled = true;
     //   botao.Select();
    }
    private void OnDisable()
    {
        if (first != null)
        {
            first.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
