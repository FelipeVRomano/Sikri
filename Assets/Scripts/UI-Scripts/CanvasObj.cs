using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasObj : MonoBehaviour
{

    private RectTransform canvas;
    [SerializeField]
    Camera mainCamera;
    void Start()
    {
        canvas = GetComponent<RectTransform>();
        mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
    }

    //camera tem q ser jogada na Unity
    void Update()
    {
        //faz o texto seguir o player
        canvas.gameObject.transform.LookAt(mainCamera.transform);
        //
    }
}
