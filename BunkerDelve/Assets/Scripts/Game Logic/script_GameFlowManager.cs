using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_GameFlowManager : MonoBehaviour
{
    //GAME OBJECTS
    private script_DialogueManager dialogueManager;

    //DYNAMIC VARAIBLES
    enum_GameFlowState currState = enum_GameFlowState.START;

    //INTRO TEXT OBJECTS
    public GameObject introSpotLight;

    //READY TO DECEND OBJECTS
    public GameObject trapDoorSpotLight;


    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<script_DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeState(enum_GameFlowState flowState){
        if(currState == enum_GameFlowState.START){
            currState = enum_GameFlowState.INTRO_TEXT;
            dialogueManager.playingTutorial = true;
            introSpotLight.SetActive(true);
        }
        else if(currState == enum_GameFlowState.INTRO_TEXT){
            currState = enum_GameFlowState.READY_TO_DECEND;
            introSpotLight.SetActive(false);
            trapDoorSpotLight.SetActive(true);

        }
        else if(flowState ==enum_GameFlowState.COMPLETED ){
            Debug.Log("You are a winner! Comgratulations");
        }
    }


}
