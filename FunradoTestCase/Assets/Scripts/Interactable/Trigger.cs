using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class Trigger : MonoBehaviour, Interactable
    {
        public  bool isOpen = false;
        private Material _materialRed;
        private Material _materialBlue;
        public GameObject connectedDoor;
        private TriggerImage _triggerImage;

        private void Awake()
        {
            _triggerImage = GetComponentInChildren<TriggerImage>();
        }

        private void Start()
        {
            _materialRed = Resources.Load<Material>("Materials/Red");
            _materialBlue = Resources.Load<Material>("Materials/Blue");
            if (connectedDoor.GetComponent<Door>().doorColor == Door.DoorColor.Blue)
            {
               transform.GetChild(0).GetComponent<Renderer>().material = _materialBlue;
            }
            else if (connectedDoor.GetComponent<Door>().doorColor == Door.DoorColor.Red)
            {
                transform.GetChild(0).GetComponent<Renderer>().material = _materialRed;
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
