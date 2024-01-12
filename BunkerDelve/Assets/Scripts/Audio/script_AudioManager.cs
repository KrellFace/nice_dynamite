using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_AudioManager : MonoBehaviour
{

    public AudioSource[] stepSounds;
    public AudioSource glowStickCrack;

    public AudioSource musicDrumsLayer;
    public float maxDrumsVolume = 0.1f;
    private float drumsIntensity = 0f;

    public AudioSource menuMusic;
    public AudioSource gameMusic;
    public AudioSource gameMusicDrums;

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

    public void MakeDrumsMoreIntense(){
        drumsIntensity+=0.05f;
        Debug.Log("Drums intensity: " + drumsIntensity);
        musicDrumsLayer.volume = Mathf.Min(drumsIntensity, maxDrumsVolume);
    }

    public void StartGameMusic(){
        
    }
}
