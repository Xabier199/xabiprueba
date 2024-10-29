using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance = null;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // M�todo para cambiar la m�sica
    public void ChangeMusic(AudioClip newClip)
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio.clip != newClip)
        {
            audio.clip = newClip;
            audio.Play();
        }
    }
}
