using System;
using System.Collections;
using System.Collections.Generic;
using Interactable;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TriggerImage : MonoBehaviour
{
    private Transform _unitTransform;
    private Transform _worldSpaceCanvasTransform;
    private Sprite _lockSprite;
    private Sprite _unlockSprite;
    private Image _image;
    private Sprite _spriteLocked;
    private Sprite _spriteUnlocked;
    public Vector3 offset;
    private void Start()
    {
      
        _unitTransform = transform.parent;
        _worldSpaceCanvasTransform = WorldCanvasInstance.Instance.transform;
        transform.SetParent(_worldSpaceCanvasTransform);
        _image = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        _lockSprite = Resources.Load<Sprite>("Sprites/Locked");
        _unlockSprite = Resources.Load<Sprite>("Sprites/Unlocked");
    }
    public void Unlocked()
    {
        _image.sprite = _unlockSprite;
    }
}
