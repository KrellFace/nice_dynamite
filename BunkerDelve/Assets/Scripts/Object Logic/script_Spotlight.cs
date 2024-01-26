using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_Spotlight : MonoBehaviour
{
    private Light light;
    private script_AudioManager audioManager;

    private float timer = 0f;
    private float currDelay = 0f;

    public bool targetState =false;
    public bool currState;

    private void Start() {
        audioManager = FindObjectOfType<script_AudioManager>();
        light = GetComponent<Light>();
        currState= targetState;
    }
    
    private void Update() {
        if(currState!=targetState){
            timer+=Time.deltaTime;
            if(timer>currDelay){
                if(targetState){
                    light.enabled = true;
                    audioManager.PlaySpotlightOn();
                }
                else{
                    light.enabled = false;
                }
                timer=0f;
                currState = targetState;
            }
        }
    }
    public void SwitchLightState(bool on, float delay){
        targetState = on;
        currDelay = delay;
    }


}
