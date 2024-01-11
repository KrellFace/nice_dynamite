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

    public string[] tutorialText;

    //DYNAMIC VARIABLES

    private bool dialogueSpawned;

    public bool playingTutorial;
    private int currTutorialText =0;


    public bool DEBUG_spawnText;

    // Start is called before the first frame update
    void Start()
    {
        gameFlowManager = FindObjectOfType<script_GameFlowManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if(playingTutorial&&!dialogueSpawned){
            
            SpawnPopUp(tutorialText[currTutorialText]);
            currTutorialText+=1;
            if(currTutorialText>tutorialText.Length-1){
                playingTutorial = false;
                gameFlowManager.ChangeState(enum_GameFlowState.READY_TO_DECEND);
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
    
        GameObject obj = Instantiate(dialoguePrefab, uiCanvas.transform);
        script_DialoguePopup popup = obj.GetComponent<script_DialoguePopup>();
        popup.Bind(text, this);
        dialogueSpawned=true;
    }
}
