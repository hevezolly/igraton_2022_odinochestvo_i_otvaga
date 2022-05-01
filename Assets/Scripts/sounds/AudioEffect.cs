using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioEffect : ScriptableObject
{
    [SerializeField]
    private SoundObject soundObj;

    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    [Range(0, 1)]
    private float volume = 1f;

    public void PlayAt(Vector3 position)
    {
        var obj = Instantiate(soundObj, position, Quaternion.identity);
        obj.audioSource.volume = volume;
        obj.audioSource.clip = clip;
        obj.Play();
    }
}
