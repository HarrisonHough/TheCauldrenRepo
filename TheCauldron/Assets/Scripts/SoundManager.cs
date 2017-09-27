using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    //private static SoundManager instance;
    //public static SoundManager Instance { get { return instance; } }

    [SerializeField]
    private AudioSource music;
    [SerializeField]
    private AudioSource sfx;
    [SerializeField]
    private AudioClip[] musicClips;

    [SerializeField]
    private AudioClip[] SFXClips;


    public bool musicMuted = false;
    public bool soundEffectsMuted = false;

    
    // Use this for initialization
    void Start () {
        if(music == null)
            music = transform.GetChild(0).GetComponent<AudioSource>();
        if(sfx = null)
            sfx = transform.GetChild(1).GetComponent<AudioSource>();
    }

    private void Update()
    {
        // TODO remove/improve this
        //if (GameObject.Find("Cauldron").GetComponent<AudioSource>().mute != soundEffectsMuted)
        //{
            //ToggleSoundEffects(soundEffectsMuted);
        //}
    }

    public void PlayTableDrop()
    {

    }

    public void PlayRandomFloorDrop()
    {

    }

    public void PlayRandomWitchDeath()
    {

    }

    public void PlayRandomAddItem()
    {

    }

    public void PlayOpenScrollSfx()
    {

    }

    public void PlayRandomWitchCackle()
    {

    }

    public void PlayLevelComplete()
    {

    }

    public void ToggleSoundEffects(bool muted)
    {

    }
}
