using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerLookingAt
{
    NONE,
    TALKABLE_NPC,
    GOAL_OBJECT
}
public class script_PlayerLookingAtChecker : MonoBehaviour
{
    //GAME OBJECTS
    //private script_InteractPopUp interactPopUp;

    //FIXED VARIABLES
    public LayerMask interactableLayer;

    //DYNAMIC VARIABLES
    private PlayerLookingAt lookingAt;
    private GameObject lookedAtObject;
    private script_GoalObject lookedAtGoalObj;

    // Start is called before the first frame update
    void Start()
    {
        //interactPopUp = FindObjectOfType<script_InteractPopUp>();
        lookingAt = PlayerLookingAt.NONE;
    }

    void FixedUpdate()
    {
        RaycastHit hit;

        if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, 3f, interactableLayer)){
            if(hit.collider.gameObject.CompareTag("NPC")){
                lookingAt = PlayerLookingAt.TALKABLE_NPC;
                lookedAtObject = hit.collider.gameObject;
                Debug.Log("Looking at NPC");
            }else if(hit.collider.gameObject.CompareTag("GoalObject")){
                lookingAt = PlayerLookingAt.GOAL_OBJECT;
                lookedAtObject = hit.collider.gameObject;
                lookedAtGoalObj = lookedAtObject.GetComponent<script_GoalObject>();
                //lookedAtGoalObj.HighlightObj(true);

                Debug.Log("Looking at Goal Object");
            }else{
                Debug.Log("Looking at unidentified interactable");
                lookingAt = PlayerLookingAt.NONE;
                lookedAtObject= null;
            }

        }else{
            lookingAt = PlayerLookingAt.NONE;
                lookedAtObject= null;
                //if(lookedAtGoalObj!=null){
                //    lookedAtGoalObj.HighlightObj(false);
                //    lookedAtGoalObj=null;
                //}
        }

        //interactPopUp.UpdateLookingAt(lookingAt);
    }

    public PlayerLookingAt GetLookingAt(){
        return lookingAt;
    }

    public GameObject GetLookedAtObject(){
        return lookedAtObject;
    }
}
