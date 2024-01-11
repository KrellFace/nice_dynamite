using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class note_Appear : MonoBehaviour
{
    [Header("Raycast Features")]
    [SerializeField] private float rayLength = 5;
    private Camera _camera;

    private  NoteController _noteController;

    [Header("Crosshair")]
    [SerializeField] private Image croshair;

    [Header("Input Key")]
    [SerializeField] private KeyCode interactKey;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }


    void Update()
    {
        if (Physics.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, out RaycastHit hit, rayLength))
        {
            var readableItem = hit.collider.GetComponent<NoteController>();
            if (readableItem != null)
            {
                _noteController = readableItem;
                highlightCrosshair(true);
            }
            else
            {
                ClearNote();
            }
        }
        else
        {
            ClearNote();
        }
        if (_noteController != null)
        {
            if (Input.GetKeyDown(interactKey))
            {
                _noteController.Shownote();
            }
        }
    }
    void ClearNote()
    {
        if(_noteController != null)
        {
            highlightCrosshair(false);
            _noteController = null;
        }
    }
    void highlightCrosshair(bool on)
    {
        if (on)
        {
            croshair.color = Color.red;
        }
        else
        {
            croshair.color = Color.white;
        }
    }
    
}
