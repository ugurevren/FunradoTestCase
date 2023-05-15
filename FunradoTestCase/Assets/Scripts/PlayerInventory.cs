using System;
using System.Collections.Generic;
using Character;
using Interactable;
using UnityEngine;
public class PlayerInventory : MonoBehaviour
    {
        public static List<CollectableKey> keys;
        
        public PlayerController playerController;
        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            keys = new List<CollectableKey>();
        }
        public void LevelUp()
        {
            var level = playerController.level;
            var maxLevel = playerController.maxLevel;
            // Increase level and destroy collectable
            if (level < maxLevel)
            {
                level+=5;
                playerController.UpdateLevelText(level);
                playerController.level = level;
            }
        }
    }