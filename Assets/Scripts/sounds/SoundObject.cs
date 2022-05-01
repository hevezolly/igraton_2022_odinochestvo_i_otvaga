using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    public AudioSource audioSource => source;

    private bool isPlaying = false;

    public void Play()
    {
        isPlaying = true;
        audioSource.Play();
    }

    private void Update()
    {
        if (isPlaying && !audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
