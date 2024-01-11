using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_StartMenu : MonoBehaviour
{
    private script_PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<script_PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(){

        Cursor.lockState = CursorLockMode.Locked;
        playerController.SetPlayerMovementAllowed(true);
        Destroy(this.gameObject);
    }
}
