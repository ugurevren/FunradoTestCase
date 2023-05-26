using System;
using System.Collections.Generic;
using Character;
using Interactable;
using UnityEngine;
public class PlayerInventory : MonoBehaviour
    {
        public static List<CollectableKey> keys; // the list of keys
        private PlayerController playerController;
        private void Start()
        {
            playerController = GetComponent<PlayerController>(); // get the player controller
            keys = new List<CollectableKey>(); // initialize the list of keys
        }
        public void LevelUp()
        {
            // This method is called when the player collects a book.
            var level = playerController.level;
            // Increase level and destroy collectable
            level += playerController.levelIncreaseRate;
            playerController.UpdateLevelText(level);
            playerController.level = level;
        }
    }