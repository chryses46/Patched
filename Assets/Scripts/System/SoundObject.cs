using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [SerializeField] AudioClip soundFile;
    [SerializeField] float volume;
    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;
    
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSource.volume = volume;
        audioSource.maxDistance = maxDistance;
        audioSource.minDistance = minDistance;
    }

}