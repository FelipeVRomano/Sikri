using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class menuPause : MonoBehaviour
{
    [SerializeField]
    private GameObject pMenu;
 
  
    private void Start()
    {
        Cursor.visible = false;
       Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Pause")&& !pMenu.activeSelf)
        {
           
            pauseMenu();

        }
        else if (Input.GetButtonDown("Pause") && pMenu.activeSelf)
        {
            
            saiMenuPause();

        }
        
    }
    public void pauseMenu()
    {
        AudioListener.volume = 0f;
        Time.timeScale = 0;
        pMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void saiMenuPause()
    {
        pMenu.SetActive(false);
        AudioListener.volume = 1f;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void SetQualityLevel(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }
  
}
