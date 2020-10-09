using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public AudioMixer audi;
    public AudioMixer audiTrilha;
    public Slider slidoMus;
    public Slider slidoEfei;
    void Start()
    {
        slidoMus.value = PlayerPrefs.GetFloat(("Musicas"));
       // PlayerPrefs.SetFloat(("Musicas"), slidoMus.value);
        audi.SetFloat("volume", PlayerPrefs.GetFloat(("Musicas")));

        //

        slidoEfei.value = PlayerPrefs.GetFloat(("TrilhaSonora"));
      //  PlayerPrefs.SetFloat(("TrilhaSonora"), slidoEfei.value);
        audiTrilha.SetFloat("volumeTrilha", PlayerPrefs.GetFloat(("TrilhaSonora")));

    }
    public void VolumeSound( float volume)
    {
        audi.SetFloat("volume", volume);
        PlayerPrefs.SetFloat(("Musicas"), volume);
    }

    public void VolumeSound2(float volume2)
    {
        audiTrilha.SetFloat("volumeTrilha", volume2);
        PlayerPrefs.SetFloat(("TrilhaSonora"), volume2);
    }
}
