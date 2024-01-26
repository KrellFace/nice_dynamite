using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_DialogueManager : MonoBehaviour
{
    //GAME OBJECTS
    public GameObject dialoguePrefab;
    public Canvas uiCanvas;
    private script_GameFlowManager gameFlowManager;

    //FIXED VARIABLES

    public string[] introText;
    public string[] glowStickTutorialText;
    public string[] photoTutorialText;
    public string[] readyToDescendText;

    //DYNAMIC VARIABLES

    private bool dialogueSpawned;
    public bool playingIntro;
    public bool playingGlowSticks;
    public bool playingPhotoTut;
    public bool playingReadyToD;
    private int currIntroText =0;



    public bool DEBUG_spawnText;

    // Start is called before the first frame update
    void Start()
    {
        gameFlowManager = FindObjectOfType<script_GameFlowManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if(playingIntro&&!dialogueSpawned){
            
            SpawnPopUp(introText[currIntroText]);
            currIntroText+=1;
            if(currIntroText>introText.Length-1){
                playingIntro = false;
                gameFlowManager.ChangeState(enum_GameFlowState.GLOWSTICKS_READY);
                currIntroText=0;
            }
        }

        if(playingGlowSticks&&!dialogueSpawned){
            
            SpawnPopUp(glowStickTutorialText[currIntroText]);
            currIntroText+=1;
            if(currIntroText>glowStickTutorialText.Length-1){
                playingGlowSticks = false;
                gameFlowManager.glowSticksDialogueOver=true;
                currIntroText=0;
            }
        }

        if(playingPhotoTut&&!dialogueSpawned){
            
            SpawnPopUp(photoTutorialText[currIntroText]);
            currIntroText+=1;
            if(currIntroText>photoTutorialText.Length-1){
                playingPhotoTut = false;
                gameFlowManager.firstPhotoDialogueOver=true;
                currIntroText=0;
            }
        }

        if(playingReadyToD&&!dialogueSpawned){
            
            SpawnPopUp(readyToDescendText[currIntroText]);
            currIntroText+=1;
            if(currIntroText>readyToDescendText.Length-1){
                playingReadyToD = false;
                gameFlowManager.ChangeState(enum_GameFlowState.READY_TO_DECEND);
                currIntroText=0;
            }
        }

        if(DEBUG_spawnText){
            //GameObject obj = Instantiate(dialoguePrefab, uiCanvas.transform);
            //script_DialoguePopup popup = obj.GetComponent<script_DialoguePopup>();
            //popup.Bind("TESTING TEXT LOL", this);
            //dialogueSpawned=true;
            SpawnPopUp("Testing text lol");
            DEBUG_spawnText=false;
        }
    }

    public void DialogueClosed(){
        dialogueSpawned = false;
        Debug.Log("Closed dialogue");

    }

    public void SpawnPopUp(string text){
        if(!dialogueSpawned){
                
            GameObject obj = Instantiate(dialoguePrefab, uiCanvas.transform);
            script_DialoguePopup popup = obj.GetComponent<script_DialoguePopup>();
            popup.Bind(text, this);
            dialogueSpawned=true;
        }
    }
}
