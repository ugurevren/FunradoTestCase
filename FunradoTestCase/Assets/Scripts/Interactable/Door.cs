using UnityEngine;

namespace Interactable
{
    public class Door : MonoBehaviour, IInteractable
    {
        public enum DoorColor {Red, Blue}; // enum for the door color

        public enum DoorType {Key, Triggered}; // enum for the door type
        
        public DoorType doorType; // the door type
        public DoorColor doorColor; // the door color
        
        [SerializeField] private GameObject _lDoor; // the left door
        [SerializeField] private GameObject _rDoor; // the right door
        [SerializeField] private Material _materialRed; // the material of the red door
        [SerializeField] private Material _materialBlue; // the material of the blue door
        
        private HingeJoint _lHingeJoint; // the left hinge joint
        private HingeJoint _rHingeJoint; // the right hinge joint
        private JointLimits _closedLimits; // the closed limits of the door
        private JointLimits _openLimits; // the open limits of the door
        private void Start()
        {
            _lHingeJoint= _lDoor.GetComponent<HingeJoint>(); // get the left hinge joint
            _rHingeJoint= _rDoor.GetComponent<HingeJoint>(); // get the right hinge joint
            
            _closedLimits = new JointLimits {max = 0, min = 0}; // set the closed limits
            _openLimits = new JointLimits {max = 120, min = -120}; // set the open limits

            _lHingeJoint.limits = _closedLimits; // close the left door
            _rHingeJoint.limits= _closedLimits; // close the right door
            
            if (doorType == DoorType.Triggered)
            {
                // if the door type is triggered, disable the collider
                GetComponent<Collider>().enabled = false;
            }
            
            // set the color of the doors
            _lDoor.GetComponent<Renderer>().material = doorColor == DoorColor.Red ? _materialRed : _materialBlue; 
            _rDoor.GetComponent<Renderer>().material = doorColor == DoorColor.Red ? _materialRed : _materialBlue;
        }
        public bool Interact()
        {
            // if the door type is triggered and there is a interact open the door
            if (doorType == DoorType.Triggered)
            {
                //Open Door
                _lHingeJoint.limits = _openLimits;
                _rHingeJoint.limits = _openLimits;
                return true;
            }
            if (doorType == DoorType.Key)
            {
                // if the door type is key, check if the player has the key
                foreach (var key in PlayerInventory.keys)
                {
                    if (key.keyColor == CollectableKey.KeyColor.Red && doorColor == DoorColor.Red)
                    {
                        // if the player has the right key, open the red door
                        key.RemoveSprite(); // remove the key sprite
                        PlayerInventory.keys.Remove(key); // remove the key from the player inventory
                        // open the door
                        _lHingeJoint.limits = _openLimits;
                        _rHingeJoint.limits = _openLimits;
                        return true;
                    }
                    if (key.keyColor == CollectableKey.KeyColor.Blue && doorColor == DoorColor.Blue)
                    {
                        // if the player has the right key, open the blue door
                        key.RemoveSprite(); // remove the key sprite
                        PlayerInventory.keys.Remove(key); // remove the key from the player inventory
                        // open the door
                        _lHingeJoint.limits = _openLimits;
                        _rHingeJoint.limits = _openLimits;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}