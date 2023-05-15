using UnityEngine;

namespace Interactable
{
    public class Door : MonoBehaviour, Interactable
    {
        public enum DoorColor {Red, Blue};
        public enum DoorType {Key, Triggered};
        public DoorType doorType;
        public DoorColor doorColor;
        private GameObject _lDoor;
        private GameObject _rDoor;
        private Material _materialRed;
        private Material _materialBlue;
        private void Start()
        {
            _rDoor = transform.GetChild(0).gameObject;
            _lDoor = transform.GetChild(1).gameObject;
            _lDoor.GetComponent<HingeJoint>().limits = new JointLimits {max = 0, min = 0};
            _rDoor.GetComponent<HingeJoint>().limits = new JointLimits {max = 0, min = 0};
            
            _materialRed = Resources.Load<Material>("Materials/Red");
            _materialBlue = Resources.Load<Material>("Materials/Blue");

            if (doorType == DoorType.Triggered)
            {
                GetComponent<Collider>().enabled = false;
            }
                
            _lDoor.GetComponent<Renderer>().material = doorColor == DoorColor.Red ? _materialRed : _materialBlue;
            _rDoor.GetComponent<Renderer>().material = doorColor == DoorColor.Red ? _materialRed : _materialBlue;
        }
        public bool Interact()
        {
            if (doorType == DoorType.Triggered)
            {
                //Open Door
                _lDoor.GetComponent<HingeJoint>().limits = new JointLimits {max = 120, min = -120};
                _rDoor.GetComponent<HingeJoint>().limits = new JointLimits {max = 120, min = -120};
                return true;
            }
            if (doorType == DoorType.Key)
            {
                foreach (var key in PlayerInventory.keys)
                {
                    if (key.keyColor == CollectableKey.KeyColor.Red && doorColor == DoorColor.Red)
                    {
                        key.RemoveSprite();
                        PlayerInventory.keys.Remove(key);
                        //Open Red Door
                        _lDoor.GetComponent<HingeJoint>().limits = new JointLimits {max = 120, min = -120};
                        _rDoor.GetComponent<HingeJoint>().limits = new JointLimits {max = 120, min = -120};
                        return true;
                    }
                    if (key.keyColor == CollectableKey.KeyColor.Blue && doorColor == DoorColor.Blue)
                    {
                        key.RemoveSprite();
                        PlayerInventory.keys.Remove(key);
                        //Open Blue Door
                        _lDoor.GetComponent<HingeJoint>().limits = new JointLimits {max = 120, min = -120};
                        _rDoor.GetComponent<HingeJoint>().limits = new JointLimits {max = 120, min = -120};
                        return true;
                    }
                }
            }
            return false;
        }

    }
}