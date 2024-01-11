using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class script_GameEndedUI : MonoBehaviour
{
    public TMP_Text tm;

    public GameObject uiStuff;

    private script_PlayerController controller;

    private void Start() {
        controller = FindObjectOfType<script_PlayerController>();
    }
    
    public void ActivateEndScreen(bool won){
        uiStuff.SetActive(true);
        if(won){
            tm.text = "You Escaped!";
        }
        else{
            tm.text = "You Died";
        }

        controller.SetPlayerMovementAllowed(false);
        Cursor.lockState = CursorLockMode.None;

    }

    public void RestartGame(){
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

    }
}
