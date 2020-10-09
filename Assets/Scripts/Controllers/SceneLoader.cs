using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject telaDeLoading;
    [SerializeField]
    private Image loading;

    public Animator transition;

    [SerializeField]
    private bool onMenu;

    [SerializeField]
    private GameObject cud;
    private void Start()
    {
        if (onMenu)
            Time.timeScale = 1;


    }
    public void LoadScene(int scene)
    {
          StartCoroutine(LoadGameProg(scene));
       // transition.SetTrigger("Start");
       // SceneManager.LoadScene(scene);
    }

    public void Update()
    {
        if (onMenu)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            onMenu = false;
        }
    }

    public void chamaFunction(int scene)
    {
        StartCoroutine(LoadGameProg(scene));
    }
    IEnumerator LoadGameProg(int scene)
    {
        Time.timeScale = 1;
        transition.SetTrigger("Start");
        cud.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(scene);
        /* AsyncOperation async = SceneManager.LoadSceneAsync(scene);
           while(!async.isDone)
           {
            telaDeLoading.SetActive(true);
            //  transition.SetTrigger("Start");
            // cud.SetActive(true);
             loading.fillAmount += async.progress;

               yield return null;
           }
           */
    }
    public void Sair()
    {
        Application.Quit();
    }
}
