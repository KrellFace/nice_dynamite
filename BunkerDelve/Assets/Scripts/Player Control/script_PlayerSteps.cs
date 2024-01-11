using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_PlayerSteps : MonoBehaviour
{
    //Game Objects
    //private script_GameManager gameManager;
    private script_AudioManager audioManager;
    public Transform playerCamera;

    //Fixed Variables
    public float stepsDelay;

    public float xHeadBob;
    //public float maxXHeadBob;
    public float yHeadSway;
    //public float maxYHeadBob;

    //Dynamic Variables
    private float currStepsDelayMod = 1f;
    private float stepCurrDelay = 0f;
    //Half frequency of normal steps
    private float swayCurrDelay = 0f;
    private bool playerIsWalking = false;
    private float currXHeadTilt=0;
    private float currYHeadTilt=0;
    // Start is called before the first frame update
    void Start()
    {
        //gameManager = FindObjectOfType<script_GameManager>();
        audioManager = FindObjectOfType<script_AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsWalking /*&& !gameManager.GetIsInConversation()*/){
            StepsLogic();
            HeadBobLogic();
        }
    }

    private void StepsLogic(){
        stepCurrDelay+=Time.deltaTime;
        swayCurrDelay+=Time.deltaTime;
        if(stepCurrDelay>(stepsDelay*currStepsDelayMod)){
            stepCurrDelay = 0f;
            audioManager.PlayStep();
            //prevStepClipIndex = SelectNotPreviousClip(stepsClips.Length, prevStepClipIndex);
            //stepsClips[prevStepClipIndex].Play(0);
        }
        if(swayCurrDelay>((stepsDelay*currStepsDelayMod)*2f)){
            swayCurrDelay = 0f;
        }
    }

    private void HeadBobLogic(){
        float stepProgress = stepCurrDelay/(stepsDelay*currStepsDelayMod);
        float bobVar = Mathf.Sin(Mathf.Abs(stepProgress-.5f)*2f);
        
        float swayStepProgress = swayCurrDelay/((stepsDelay*currStepsDelayMod)*2);
        float swayVar = Mathf.Sin(Mathf.Abs(swayStepProgress-.5f)*2f);
        
        float aboveMin = Mathf.Lerp(0,xHeadBob*2,bobVar);
        float beyondMin = Mathf.Lerp(0,yHeadSway*2,swayVar);
        
        //Debug.Log("Above min: " + aboveMin);
        currXHeadTilt = aboveMin-xHeadBob;
        currYHeadTilt = beyondMin-yHeadSway;
        Vector3 currCameraEul = playerCamera.localEulerAngles;
        playerCamera.localEulerAngles = new Vector3(currXHeadTilt, currYHeadTilt, currCameraEul.z);
    }
    public void SetIsWalking(bool b){
        playerIsWalking = b;
    }
    public void SetStepsSpeedMod(float newMod){
        currStepsDelayMod = newMod;
    }
}
