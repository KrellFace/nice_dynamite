using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class script_RoomDressingManager : MonoBehaviour

{

    //GAME OBJECTS

    private script_DialogueManager dialogueManager;
    private script_GameFlowManager gameFlowManager;
    private script_AudioManager audioManager;

    [Header("Set Dressing Prefabs")]
    public GameObject[] basicBoxDressings;
    public GameObject[] tJunctDressings;

    
    [Header("Other Room Prefabs")]
    public GameObject[] doorBlocks;
    public GameObject[] goalObjects;

    private int currGoalID;

    //FIXED VARIABLES
    public float goalObjectChance = 0.2f;

    void Start()
    {
        dialogueManager = FindObjectOfType<script_DialogueManager>();
        gameFlowManager = FindObjectOfType<script_GameFlowManager>();
        audioManager  = FindObjectOfType<script_AudioManager>();
    }
    
    public GameObject GetDressingForRoomType(enum_RoomTypes type){
        GameObject ret = null;
        if(type==enum_RoomTypes.BASICBOX){
            ret = Instantiate(basicBoxDressings[Random.Range(0,basicBoxDressings.Length)]);
        }
        else if(type==enum_RoomTypes.TJUNCTION){
            ret = Instantiate(tJunctDressings[Random.Range(0,tJunctDressings.Length)]);
        }
        return ret;
    }

    public GameObject GetBlockedDoorPrefab(){
        return Instantiate(doorBlocks[Random.Range(0,doorBlocks.Length)]);
    }

    public void SpawnGoalObject(Transform parentLoc){
        if(Random.Range(0f, 1f)<goalObjectChance){
            GameObject obj = Instantiate(goalObjects[currGoalID]);
            obj.GetComponent<script_GoalObject>().Bind(this);
            obj.transform.parent = parentLoc;
            obj.transform.localPosition = new Vector3(0,0,0);
            obj.transform.localEulerAngles = new Vector3(0,0,0);
        }
    }

    public void CollectGoalObject(int id){
        if(id == currGoalID){
            currGoalID+=1;
            Debug.Log("Collected goal object " + id + ". Advancing in game");
            if(currGoalID >1){
                audioManager.MakeDrumsMoreIntense();
            }
            if(currGoalID==2){
                dialogueManager.SpawnPopUp("Good... onto the next one.");
            }
            if(currGoalID==3){
                dialogueManager.SpawnPopUp("Gotcha.");
            }
            if(currGoalID==4){
                dialogueManager.SpawnPopUp("Moving on... I must hurry...");
            }
            if(currGoalID==5){
                dialogueManager.SpawnPopUp("One more to go! Please let me make it");
            }
            if(currGoalID ==6){
                dialogueManager.SpawnPopUp("I got them all! The exit - I need to find the exit!");
                gameFlowManager.ChangeState(enum_GameFlowState.READY_TO_LEAVE);


            }
        }
        else{
            Debug.Log("Goal already collected, soz");
            if(currGoalID >1){
                dialogueManager.SpawnPopUp("I've already found this photo. Got to keep looking");
            }
        }
    }
}
