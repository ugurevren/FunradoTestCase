using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Interactable
{
    public class CollectableKey : MonoBehaviour, Interactable
    {
        public enum KeyColor {Red, Blue};
        public KeyColor keyColor;
        public Transform uiTransform;
        public Vector3 uiToWorldTransform;
        private Material _materialRed;
        private Material _materialBlue;
        private Sprite _spriteRed;
        private Sprite _spriteBlue;
        private Image[] _images;
        private Image _image;
        private void Start()
        {
            _images = new Image[uiTransform.childCount];
            for (int i = 0; i < uiTransform.childCount; i++)
            {
                _images[i] = uiTransform.GetChild(i).GetComponent<Image>();
                _images[i].color = new Color(1, 1, 1, 0f);
            }


        }
        private void OnValidate()
        {
            _materialRed = Resources.Load<Material>("Materials/Red");
            _materialBlue = Resources.Load<Material>("Materials/Blue");
            _spriteRed = Resources.Load<Sprite>("Sprites/RedKeyImage");
            _spriteBlue = Resources.Load<Sprite>("Sprites/BlueKeyImage");
            GetComponent<Renderer>().material = keyColor == KeyColor.Red ? _materialRed : _materialBlue;
        }
        
        public bool Interact()
        {
            Vector3 pos1 = Camera.main.ScreenToWorldPoint(uiTransform.position);
            Vector3 pos2;
            // raycast to find the position of the key in the world
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(uiTransform.position);
            if (Physics.Raycast(ray, out hit))
            {
                pos2 = hit.point;
            }
            else
            {
                pos2 = uiToWorldTransform;
            }
            //uiToWorldTransform = center of pos1 and pos2
            uiToWorldTransform = (pos1 + pos2) / 2;
            
            
            
            StartCoroutine(MoveToPosition(transform, uiToWorldTransform, 1f));
            StartCoroutine(UpdateInventory(1f));
            GetComponent<Collider>().enabled = false;
            Invoke("SetSprite", 1f);
            return true;
            
        }
        public void SetSprite()
        {
            
            foreach (Image i in _images)
            {
                if (i.sprite == null)
                {
                    _image = i;
                    _image.sprite = keyColor == KeyColor.Red ? _spriteRed : _spriteBlue;
                    _image.color = new Color(1, 1, 1, 1f);
                    break;
                }
                
            }
            
        }
        public void RemoveSprite()
        {
            _image.sprite = null;
            _image.color = new Color(1, 1, 1, 0f);
        }
        private IEnumerator UpdateInventory(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            PlayerInventory.keys.Add(this);
            Destroy(gameObject);
        }
        
        private IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
        {
            var currentPos = transform.position;
            var t = 0f;
            while(t < 1)
            {
                t += Time.deltaTime / timeToMove;
                transform.position = Vector3.Lerp(currentPos, position, t);
                yield return null;
            }
        }
        
    }
}