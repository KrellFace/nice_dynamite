using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class script_PlayerController : MonoBehaviour
{
    //GAME OBJECTS
    //private script_GoalObjectManager goalObjectManager;
    private script_AudioManager audioManager;
    //private script_CollectionLog collectionLog;
    private script_PlayerSteps playerSteps;
    //private script_GameManager gameManager;

    //PLAYER BODY
    public GameObject playerHead;
    private CharacterController controller;
    private script_PlayerLookingAtChecker playerLookingAtChecker;
    public Transform playerHands;
    //MOVEMENT
    public float moveSpeed = 0.1f;
    public float sprintMod = 1.5f;
    private Vector2 currMoveInput;
    public float currVertSpeed = 0f;
    public float frictionAmount = 0.1f;
    public float clampHorizontalSpeedVal = 3f;
    private bool isSprinting = false;

    //PLAYER STATE
    private bool playerMovementAllowed = false;



    //LOOKING AROUND
    public float yawSpeed = 50f;
    public float pitchSpeed = 40f;
    private float lookYaw = 0f;
    private float lookPitch = 0f;

    private float currLookSpeedSensitivity = 1f;

    [SerializeField] private float pitchlimUP = 45f;
    [SerializeField] private float pitchlimDOWN = -45f;
    private Vector2 currLookDirr;
    //GRAVITY AND GROUNDING
    //public float floorCheckDist = 0.4f;
    public float maxFallSpeed = 10f;
    public float gravityAcceleration = 5f;
    //JUMP
    public float jumpSpeed = 50f;

    //CROUCHING STUFF

    private bool crouching = false;
    private Vector3 localHeadPos;
    public float crouchAmount = 0.6f;
    public float crouchSpeedReduction = 0.5f;

    //GLOWSTICK THROWING
    public GameObject glowstickPrefab;
    private bool glowStickThrown = false;
    private float glowStickCooldown = 2f;
    private float currGlowstickTimer = 0f;
    public float glowStickThrowForce = 5f;


    private void Start() {

        controller = GetComponent<CharacterController>();
        playerLookingAtChecker = GetComponentInChildren<script_PlayerLookingAtChecker>();
        playerSteps = GetComponent<script_PlayerSteps>();
    
        //Get initialisation variables
        localHeadPos = playerHead.transform.localPosition;
        lookYaw = this.transform.eulerAngles.y;

        //goalObjectManager = FindObjectOfType<script_GoalObjectManager>();
        audioManager = FindObjectOfType<script_AudioManager>();
        //collectionLog = FindObjectOfType<script_CollectionLog>();
        //gameManager = FindObjectOfType<script_GameManager>();

        Cursor.lockState = CursorLockMode.Locked;

        playerMovementAllowed=true;

    }
    void Update()
    {
        //Debug.Log("Controller is grounded " + controller.isGrounded);
        if(currMoveInput.magnitude>0.1f){
            playerSteps.SetIsWalking(true);
        }
        else{
            playerSteps.SetIsWalking(false);
        }

        if(glowStickThrown){
            currGlowstickTimer+=Time.deltaTime;
            if(currGlowstickTimer>glowStickCooldown){
                glowStickThrown=false;
                currGlowstickTimer = 0f;
            }
        }
        
    }

    private void FixedUpdate() {
        if(playerMovementAllowed){
            //Debug.Log("Player Movement allowed");
            PlayerMovement();
            UpdatePlayerFacing();
            CrouchingLogic();
        }
        //GroundedCheck();
        //FellThroughTerrainCheck();
        
    }

    public void OnLookAction(InputAction.CallbackContext context){
        currLookDirr = context.ReadValue<Vector2>();
        //Debug.Log("Look action: " + context.ReadValue<Vector2>());
    }
    
    public void OnMoveAction(InputAction.CallbackContext context){
        currMoveInput = context.ReadValue<Vector2>();
    }
    public void OnJumpAction(InputAction.CallbackContext context){
        //if(isgrounded&&context.performed){
        if(controller.isGrounded&&context.performed&&playerMovementAllowed){
            currVertSpeed = jumpSpeed;
            //isgrounded = false;
            audioManager.PlayJumpAudio();

        }
    }
    public void OnCrouchAction(InputAction.CallbackContext context){
        if(context.performed&&playerMovementAllowed){
            crouching = true;
            playerSteps.SetStepsSpeedMod(1f/crouchSpeedReduction);
        }
        else if(context.canceled){
            crouching = false;
            playerSteps.SetStepsSpeedMod(1f);
        }
    }
    public void OnSprintAction(InputAction.CallbackContext context){
        if(context.performed&&playerMovementAllowed){
            isSprinting = true;
            playerSteps.SetStepsSpeedMod(1f/sprintMod);
        }
        else if(context.canceled){
            isSprinting = false;
            playerSteps.SetStepsSpeedMod(1f);
        }
    }
    public void OnInteractAction(InputAction.CallbackContext context){
        if(context.performed&&playerMovementAllowed){
            if(playerLookingAtChecker.GetLookingAt() == PlayerLookingAt.TALKABLE_NPC){
                //playerLookingAtChecker.GetLookedAtObject().GetComponent<script_npc>().TriggerDialogue();
            }
            else if (playerLookingAtChecker.GetLookingAt() == PlayerLookingAt.GOAL_OBJECT){
                //script_GoalObject gObj = playerLookingAtChecker.GetLookedAtObject().GetComponent<script_GoalObject>();
                //goalObjectManager.AddToCollected(gObj);
                //collectionLog.CollectGoalObject(gObj.GetGoalObjectType());
            }
        }
    }
   
    public void OnPauseAction(InputAction.CallbackContext context){
        if(context.performed){
            //gameManager.PauseAction();
        }
    } 
    
    public void OnFireAction(InputAction.CallbackContext context){
        if(context.performed){
            ThrowGlowstick();
        }
    } 

    private void ThrowGlowstick(){
        GameObject glowstick = Instantiate(glowstickPrefab,playerHands.transform.position, this.transform.rotation);
        Rigidbody rgb = glowstick.GetComponent<Rigidbody>();
        //Debug.Log(this.transform.forward);
        rgb.AddForce(this.transform.forward*glowStickThrowForce);
        audioManager.PlayGlowstickCrack();

    }

    private void UpdatePlayerFacing(){
        lookYaw += currLookDirr.x * yawSpeed * currLookSpeedSensitivity * Time.deltaTime;
        lookPitch -= currLookDirr.y * pitchSpeed * currLookSpeedSensitivity * Time.deltaTime;

        lookPitch = Mathf.Clamp(lookPitch, pitchlimDOWN, pitchlimUP);

        this.transform.eulerAngles = new Vector3(0, lookYaw, 0);
        playerHead.transform.localEulerAngles = new Vector3(lookPitch, 0.0f, 0.0f);

    }

    private void PlayerMovement(){

        //Handle Movement
        Vector3 frameMoveVector = new Vector3(0,0,0);

        //HORIZONTAL

        //Add move input to horizontal movement
        float xMove = currMoveInput.x * moveSpeed * Time.deltaTime;
        float zMove = currMoveInput.y * moveSpeed * Time.deltaTime;

        if(crouching){
            xMove *= crouchSpeedReduction;
            zMove *= crouchSpeedReduction;
        }
        else if(isSprinting){

            xMove *= sprintMod;
            zMove *= sprintMod;
        }

        frameMoveVector.x = xMove;
        frameMoveVector.z = zMove;
        

        //VERTICAL

        if(controller.isGrounded && currVertSpeed<0){
            currVertSpeed = 0;
        }
       
        //Steadily increase the current fall speed
        currVertSpeed-= gravityAcceleration*Time.deltaTime;

        //Cap falling speed
        currVertSpeed = Mathf.Clamp(currVertSpeed, -maxFallSpeed, maxFallSpeed);
        
        //currMoveVector.y = Mathf.Lerp(currMoveVector.y, targetVertSpeed, Time.deltaTime);
        frameMoveVector.y += currVertSpeed*Time.deltaTime;

        

        //Convert to global move
        Vector3 globalisedMove = this.transform.rotation * frameMoveVector;

        //Move player
        //this.transform.Translate(frameMoveVector);
        //Debug.Log("Frame move vector: " + frameMoveVector);
        //.Log("Globalised move: " + globalisedMove);
        controller.Move(globalisedMove);

    }

    public void MovePlayerController( Vector3 move){
        controller.Move(move);
    }

    private void CrouchingLogic(){
        if(crouching){
            //Debug.Log("Crouching!");
            playerHead.transform.localPosition = new Vector3(localHeadPos.x, localHeadPos.y-crouchAmount, localHeadPos.z);
        }
        else{
            playerHead.transform.localPosition = localHeadPos; 
        }
    }
    public Transform GetPlayerHeadTransform(){
        return playerHead.transform;
    }

    public void SetLookSensitivity(float newSensitivity){
        currLookSpeedSensitivity = newSensitivity;
    }

    public float GetLookSensitivity(){
        return currLookSpeedSensitivity;
    }

    public void SetPlayerMovementAllowed(bool allowed){
        playerMovementAllowed = allowed;
    }

    public bool GetIsGrounded(){
        return controller.isGrounded;
    }
}
