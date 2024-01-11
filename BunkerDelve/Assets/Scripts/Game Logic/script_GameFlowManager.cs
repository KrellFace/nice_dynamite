using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_GameFlowManager : MonoBehaviour
{
    //GAME OBJECTS
    private script_DialogueManager dialogueManager;
    private script_RoomManager roomManager;

    public GameObject photoPopUpPrefab;
    public Canvas uiCanvas;

    //DYNAMIC VARAIBLES
    enum_GameFlowState currState = enum_GameFlowState.START;
    private bool photoPopUpPresent = false;
    private GameObject photoPopUp = null;

    //INTRO TEXT OBJECTS
    public GameObject introSpotLight;

    //READY TO DECEND OBJECTS
    public GameObject trapDoorSpotLight;


    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<script_DialogueManager>();
        roomManager = FindObjectOfType<script_RoomManager>();
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
            dialogueManager.SpawnPopUp("I feel a profound sense of dread, but I guess there is no other choice. Down I go");
            Debug.Log("Ready to Decend");

        }
        else if(currState == enum_GameFlowState.READY_TO_DECEND){
            currState = enum_GameFlowState.INITIAL_HUNT;
            dialogueManager.SpawnPopUp("Lets find some photos");
            Debug.Log("Entered dungeon");
        }
        else if(currState == enum_GameFlowState.INITIAL_HUNT){
            currState = enum_GameFlowState.PERSUED;
            dialogueManager.SpawnPopUp("I think i hear something following me");
            Debug.Log("Now hunted");
            //TO DO
            //START SPAWNING MONSTER HERE
        }
        else if(currState == enum_GameFlowState.PERSUED){
            currState = enum_GameFlowState.READY_TO_LEAVE;
            roomManager.SetReadyToSpawnGoal();
            Debug.Log("Now ready to leave");
        }
        else if(flowState ==enum_GameFlowState.COMPLETED ){
            Debug.Log("You are a winner! Comgratulations");
        }
    }

    public void CollectGoalObject(script_GoalObject go){
        photoPopUp = Instantiate(photoPopUpPrefab, uiCanvas.transform);
        photoPopUp.transform.localPosition = new Vector3(0,0,0);
        photoPopUp.GetComponent<script_PhotoPopup>().Bind(go.goalSprite, go.goalText);
        photoPopUpPresent= true;
        Time.timeScale = 0;
    }

    public void ClosePopUp(){
        Destroy(photoPopUp);
        photoPopUp=null;
        photoPopUpPresent=false;
        Time.timeScale = 1;


    }

    public bool GetPhotoPopupPresent(){
        return photoPopUpPresent;
    }


}
