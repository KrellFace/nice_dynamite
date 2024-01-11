using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class NoteController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode closeKey;

      [Space(10)]
   public GameObject Player;

      [Header("UI Text")]
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextAreaUI;

      [Space(10)]
    [SerializeField] [TextArea] private string noteText;
      [Space(10)]
    [SerializeField] private UnityEvent openEvent;
    private bool isOpen = false;
    public void Shownote()
    {
        noteTextAreaUI.text = noteText;
        noteCanvas.SetActive(true);
        openEvent.Invoke();
        //Disable Player?
        isOpen = true;
    }

    void DisableNote()
    {
        noteCanvas.SetActive(false);
        //enable player
        isOpen = false;
    }

    void DisablePlayer(bool disable)
    {
        //player.enabled = !disable;
    }
    private void Update()
    {
        if (isOpen)
        {
            if(Input.GetKeyDown(closeKey))
            {
                DisableNote();
            }
        }
    }
}
