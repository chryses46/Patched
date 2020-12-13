using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public static AudioController instance;


    [SerializeField] AudioClip song;

    AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }


   public void StartMusic()
   {
       audioSource.clip = song;
       if(audioSource.isPlaying) audioSource.Stop();
       audioSource.Play();
   }


}
