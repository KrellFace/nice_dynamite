using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_AudioManager : MonoBehaviour
{

    public AudioSource[] stepSounds;
    public AudioSource glowStickCrack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayStep(){
        stepSounds[Random.Range(0, stepSounds.Length)].Play(0);
    }

    public void PlayJumpAudio(){
        
    }

    public void PlayGlowstickCrack(){
        glowStickCrack.Play(0);
    }
}
