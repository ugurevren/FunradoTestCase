using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class Trigger : MonoBehaviour, IInteractable
    {
        // This class is used for the trigger of doors.
        private  bool isOpen = false; // is the door open
        [SerializeField] private Material _materialRed; // the red material
        [SerializeField] private Material _materialBlue; // the blue material
        [SerializeField] private Renderer renderer; // the renderer of the trigger
        [SerializeField] private GameObject connectedDoor; // the connected door to trigger
        private TriggerImage _triggerImage; // the trigger image
        [SerializeField] 
        

        private void Awake()
        {
            _triggerImage = GetComponentInChildren<TriggerImage>(); // get the trigger image
        }
        
        private void Start()
        {
            if (connectedDoor.GetComponent<Door>().doorColor == Door.DoorColor.Blue)
            {
                // set the color of the trigger as blue if the door is blue
               renderer.material = _materialBlue;
            }
            else if (connectedDoor.GetComponent<Door>().doorColor == Door.DoorColor.Red)
            {
                // set the color of the trigger as red if the door is red
                renderer.material = _materialRed;
            }
            
        }

        public bool Interact()
        {
            isOpen = true;
            _triggerImage.Unlocked();
            connectedDoor.GetComponent<Door>().Interact();
            return true;
        }
        
    }
}
