using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class script_GameFlowManager : MonoBehaviour
{
    //GAME OBJECTS
    private script_DialogueManager dialogueManager;
    private script_RoomManager roomManager;

    public GameObject photoPopUpPrefab;
    public Canvas uiCanvas;

    private PlayerDetector enemy;

    private script_Trapdoor trapdoor;
    private script_GameEndedUI gameEndedUI;

    public GameObject glowsticks;
    public GameObject firstPhoto;

    //DYNAMIC VARAIBLES
    enum_GameFlowState currState = enum_GameFlowState.START;
    private bool photoPopUpPresent = false;
    private GameObject photoPopUp = null;

    //INTRO TEXT OBJECTS
    public script_Spotlight introSpotLight;

    //GLOW STICKS 
    public bool glowSticksCollected = false;
    public bool glowSticksDialogueOver;

    public bool firstPhotoCollected;
    public bool firstPhotoDialogueOver;
    public script_Spotlight firstPhotoLight;



    //READY TO DECEND OBJECTS
    public script_Spotlight trapDoorSpotLight;


    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<script_DialogueManager>();
        roomManager = FindObjectOfType<script_RoomManager>();
        enemy = FindObjectOfType<PlayerDetector>();
        trapdoor = FindObjectOfType<script_Trapdoor>();
        gameEndedUI = FindObjectOfType<script_GameEndedUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(glowSticksCollected&&currState == enum_GameFlowState.GLOWSTICKS_READY){
            ChangeState(enum_GameFlowState.GLOWSTICKS_TUTORIAL);
        }

        if(firstPhotoCollected&&firstPhotoDialogueOver&&currState == enum_GameFlowState.PHOTO_PICKUP){
            ChangeState(enum_GameFlowState.FINAL_INTRO_CHAT);
        }
    }

    public void ChangeState(enum_GameFlowState flowState){
        if(currState == enum_GameFlowState.START&&flowState ==enum_GameFlowState.INTRO_TEXT){
            currState = enum_GameFlowState.INTRO_TEXT;
            dialogueManager.playingIntro = true;
            introSpotLight.SwitchLightState(true, 0.5f);
        }
        else if(currState == enum_GameFlowState.INTRO_TEXT&&flowState ==enum_GameFlowState.GLOWSTICKS_READY){
            currState = enum_GameFlowState.GLOWSTICKS_READY;
            glowsticks.SetActive(true);
            introSpotLight.SwitchLightState(false, 0f);

        }
        else if(currState == enum_GameFlowState.GLOWSTICKS_READY&&flowState ==enum_GameFlowState.GLOWSTICKS_TUTORIAL){
            currState = enum_GameFlowState.GLOWSTICKS_TUTORIAL;
            glowsticks.SetActive(true);
            dialogueManager.playingGlowSticks = true;
            Debug.Log("Ready to pick up glowsticks");

        }
        else if(currState == enum_GameFlowState.GLOWSTICKS_TUTORIAL&&flowState ==enum_GameFlowState.PHOTO_PICKUP){
            currState = enum_GameFlowState.PHOTO_PICKUP;
            firstPhoto.SetActive(true);
            firstPhotoLight.SwitchLightState(true, 0.5f);
            dialogueManager.playingPhotoTut = true;
            //dialogueManager.SpawnPopUp("I feel a profound sense of dread, but I guess there is no other choice. Down I go");
            Debug.Log("Ready to First photo");

        }
        else if(currState == enum_GameFlowState.PHOTO_PICKUP&&flowState ==enum_GameFlowState.FINAL_INTRO_CHAT){
            currState = enum_GameFlowState.FINAL_INTRO_CHAT;
            trapDoorSpotLight.SwitchLightState(true, 0.5f);
            firstPhotoLight.SwitchLightState(false, 0f);
            dialogueManager.playingReadyToD = true;
            //dialogueManager.SpawnPopUp("I feel a profound sense of dread, but I guess there is no other choice. Down I go");
            Debug.Log("Ready to Decend");

        }
        else if(currState == enum_GameFlowState.FINAL_INTRO_CHAT&&flowState ==enum_GameFlowState.READY_TO_DECEND){
            currState = enum_GameFlowState.READY_TO_DECEND;
            introSpotLight.SwitchLightState(false, 0f);
            trapDoorSpotLight.SwitchLightState(true, 0.5f);
            trapdoor.OpenTrapdoor();
            //dialogueManager.SpawnPopUp("I feel a profound sense of dread, but I guess there is no other choice. Down I go");
            Debug.Log("Ready to Decend");

        }
        else if(currState == enum_GameFlowState.READY_TO_DECEND&&flowState ==enum_GameFlowState.INITIAL_HUNT){
            currState = enum_GameFlowState.INITIAL_HUNT;
            trapDoorSpotLight.SwitchLightState(false, 0f);
            Debug.Log("Entered dungeon");
        }
        else if(currState == enum_GameFlowState.INITIAL_HUNT&&flowState ==enum_GameFlowState.PERSUED){
            currState = enum_GameFlowState.PERSUED;
            dialogueManager.SpawnPopUp("I think i hear something following me");
            Debug.Log("Now hunted");
            //TO DO
            //START SPAWNING MONSTER HERE
            enemy.SetEnemyActive();
        }
        else if(currState == enum_GameFlowState.PERSUED&&flowState ==enum_GameFlowState.READY_TO_LEAVE){
            currState = enum_GameFlowState.READY_TO_LEAVE;
            roomManager.SetReadyToSpawnGoal();
            Debug.Log("Now ready to leave");
        }
        
        else if(currState == enum_GameFlowState.READY_TO_LEAVE&&flowState ==enum_GameFlowState.COMPLETED ){
            Debug.Log("You are a winner! Comgratulations");
            gameEndedUI.ActivateEndScreen(true);
        }
        
    }

    public void CollectGoalObject(script_GoalObject go){
        photoPopUp = Instantiate(photoPopUpPrefab, uiCanvas.transform);
        photoPopUp.transform.localPosition = new Vector3(0,0,0);
        photoPopUp.GetComponent<script_PhotoPopup>().Bind(go.goalSprite, go.goalText);
        photoPopUpPresent= true;
        Time.timeScale = 0;
        if(go.goalID == 0){
            firstPhotoCollected=true;
        }
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
