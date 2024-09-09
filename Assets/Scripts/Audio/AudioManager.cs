using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;
    [SerializeField]
    AudioClip[] footstepSounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = 0f;
        }
    }

    public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Som: " + name + " não encontrado!");
            return;
        }
        Debug.Log("Playing sound: " + name);
        s.source.Play();
    }

    public void PlayAtPoint(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Som: " + name + " não encontrado!");
            return;
        }
        Debug.Log("Playing sound: " + name);
        AudioSource.PlayClipAtPoint(s.clip, new Vector3(0f, 0f, 0f), s.volume);
    }   

    public void Stop(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Som: " + name + " não encontrado!");
            return;
        }
        s.source.Stop();
    }

    public void PlayRandomFootstep(Vector3 position)
    {
        if (footstepSounds.Length > 0)
        {
            int index = Random.Range(0, footstepSounds.Length);
            AudioSource.PlayClipAtPoint(footstepSounds[index], position);
        }
    }
}
