using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_GameStateTrigger : MonoBehaviour
{
    private script_GameFlowManager gameFlowManager;

    public enum_GameFlowState gameStateTriggered;
    void Start()
    {
        gameFlowManager = FindObjectOfType<script_GameFlowManager>();
    }

    private void OnTriggerEnter(Collider col){
        if(col.gameObject.CompareTag("Player")){
            gameFlowManager.ChangeState(gameStateTriggered);
            Debug.Log("Player exited");
        }
    }
}
