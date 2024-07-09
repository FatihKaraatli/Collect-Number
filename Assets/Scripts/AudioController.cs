using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioClip;

    public static AudioController instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    public void StartAudio()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
