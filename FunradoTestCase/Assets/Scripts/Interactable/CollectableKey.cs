using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Interactable
{
    public class CollectableKey : MonoBehaviour, IInteractable
    {
        public enum KeyColor {Red, Blue}; // enum for the key color
        public KeyColor keyColor; // the key color
        [SerializeField] private Transform uiTransform; // the transform of the key in the ui
        [SerializeField] private Vector3 uiToWorldTransform; // the transform of the key in the world
        [SerializeField] private Material _materialRed; // the material of the red key
        [SerializeField] private Material _materialBlue; // the material of the blue key
        [SerializeField] private Sprite _spriteRed; // the sprite of the red key
        [SerializeField] private Sprite _spriteBlue; // the sprite of the blue key
        private Image[] _images; // the images in the ui
        private Image _image; // the image of the key in the ui
        private void Start()
        {
            _images = new Image[uiTransform.childCount]; // initialize the images array
            for (int i = 0; i < uiTransform.childCount; i++)
            {
                // get the images in the ui
                //TODO
                _images[i] = uiTransform.GetChild(i).GetComponent<Image>(); // get the image component
                _images[i].color = new Color(1, 1, 1, 0f); // set the color to transparent
            }
        }
        private void OnValidate()
        {
            GetComponent<Renderer>().material = keyColor == KeyColor.Red ? _materialRed : _materialBlue;
        }
        
        public bool Interact()
        {
            Vector3 pos1 = Camera.main.ScreenToWorldPoint(uiTransform.position); //
            Vector3 pos2;
            // raycast to find the position of the key in the world
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(uiTransform.position);
            if (Physics.Raycast(ray, out hit))
            {
                pos2 = hit.point; // if the raycast hits something, set pos2 to the hit point
            }
            else
            {
                pos2 = uiToWorldTransform; // if the raycast doesn't hit anything, set pos2 to the uiToWorldTransform
            }
            //uiToWorldTransform = center of pos1 and pos2
            uiToWorldTransform = (pos1 + pos2) / 2;
            
            
            
            StartCoroutine(MoveToPosition(transform, uiToWorldTransform, 1f)); // move the key to the ui position
            StartCoroutine(UpdateInventory(1f));  // update the inventory after 1 second
            GetComponent<Collider>().enabled = false; // disable the collider
            Invoke("SetSprite", 1f); // call SetSprite() after 1 second
            return true;
            
        }
        public void SetSprite()
        {
            // set the sprite of the key in the ui
            foreach (Image i in _images)
            {
                if (i.sprite == null)
                {
                    // for each empty image in the ui, set the sprite to the key sprite
                    _image = i; // set the image to the empty image
                    _image.sprite = keyColor == KeyColor.Red ? _spriteRed : _spriteBlue; // set the sprite
                    _image.color = new Color(1, 1, 1, 1f); // set the color to opaque
                    break;
                }
                
            }
            
        }
        public void RemoveSprite()
        {
            // remove the sprite of the key in the ui
            _image.sprite = null; // set the sprite to null
            _image.color = new Color(1, 1, 1, 0f); // set the color to transparent
        }
        private IEnumerator UpdateInventory(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            PlayerInventory.keys.Add(this); // add the key to the player inventory
            Destroy(gameObject); // destroy the key
        }
        
        private IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
        {
            // move the key to the ui position
            var currentPos = transform.position; // get the current position
            var t = 0f; // initialize t
            while(t < 1) 
            {
                t += Time.deltaTime / timeToMove; // increment t
                transform.position = Vector3.Lerp(currentPos, position, t); // move the key to the ui position
                yield return null;
            }
        }
        
    }
}