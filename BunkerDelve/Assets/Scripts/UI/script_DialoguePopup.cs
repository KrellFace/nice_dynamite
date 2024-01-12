using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class script_DialoguePopup : MonoBehaviour
{
    //GAME OBJECTS
    public TMP_Text tmpObj;
    private script_DialogueManager dialogueManager;

    //FIXED VARIABLES
    private float yLocOut = -610;
    private float yLocIn = -360;
    private float currYLoc = 0f;

    private float slideInTime = 0.25f;
    private float characterAppearTime = 0.001f;

    private float hangTime = 3f;

    //DYNAMIC VARAIBLES
    private enum_DialogueState state;
    private string textToBind;
    private string currText;
    private float timer = 0f;
    private int textAmountIn = 0;

    // Update is called once per frame
    void Update()
    {
        if(state == enum_DialogueState.SLIDING_IN){
            timer+=Time.deltaTime;
            currYLoc = Mathf.Lerp(yLocOut,yLocIn, timer/slideInTime);
            this.transform.localPosition = new Vector3(0,currYLoc, 0);
            if(timer>slideInTime){
                state = enum_DialogueState.TEXT_IN;
                timer = 0f;
            }
        }
        else if(state == enum_DialogueState.TEXT_IN){
            timer+=Time.deltaTime;
            if(timer>characterAppearTime){
                textAmountIn+=1;
                currText = textToBind.Substring(0,textAmountIn);
                tmpObj.text = currText;
                timer = 0f;
            }

            if(textAmountIn>textToBind.Length-1){
                state = enum_DialogueState.HOLDING;
                timer = 0f;
            }
        }
        else if(state == enum_DialogueState.HOLDING){
            timer+=Time.deltaTime;
            if(timer>hangTime){
                state = enum_DialogueState.SLIDING_OUT;
                timer = 0f;
            }
        }
        else {
            timer+=Time.deltaTime;
            currYLoc = Mathf.Lerp(yLocIn,yLocOut, timer/slideInTime);
            this.transform.localPosition = new Vector3(0,currYLoc, 0);
            if(timer>slideInTime){
                dialogueManager.DialogueClosed();
                Destroy(this.gameObject);
            }
        }
    }

    public void Bind(string text, script_DialogueManager dm){
        state = enum_DialogueState.SLIDING_IN;
        this.dialogueManager = dm;
        textToBind = text;
        this.transform.position = new Vector3(0,yLocIn, 0);
        tmpObj.text = "";
    }
}
