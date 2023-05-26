using System;
using System.Collections;
using System.Collections.Generic;
using Interactable;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TriggerImage : MonoBehaviour
{
    private Transform _worldSpaceCanvasTransform;
    private Sprite _unlockSprite;
    [SerializeField] private Image unlockedImageComponent; // Image component of the TriggerImage
    [SerializeField] private Sprite unlockedSprite; // Sprite of the TriggerImage
    private void Start()
    {
        _worldSpaceCanvasTransform = WorldCanvas.Instance.transform; // Get the WorldSpaceCanvas
        transform.SetParent(_worldSpaceCanvasTransform); // Set the parent of the TriggerImage to the WorldSpaceCanvas
    }
    public void Unlocked()
    {
        // Set the sprite of the TriggerImage to the Unlocked sprite
        unlockedImageComponent.sprite = unlockedSprite;
    }
}
