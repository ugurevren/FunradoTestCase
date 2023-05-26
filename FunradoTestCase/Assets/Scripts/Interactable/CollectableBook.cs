using UnityEngine;

namespace Interactable
{
    public class CollectableBook : MonoBehaviour , IInteractable
    {
        // This class is used for the collectable book.
        private PlayerInventory _playerInventory;
        private void Start()
        {
            _playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();   // Get the player inventory.
        }
        public bool Interact()
        {
            // This method is called when the player interacts with the book.
            _playerInventory.LevelUp();
            Destroy(gameObject);
            return true;
        }
    }
}