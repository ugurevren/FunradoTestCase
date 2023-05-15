using UnityEngine;

namespace Interactable
{
    public class CollectableBook : MonoBehaviour , Interactable
    {
        private PlayerInventory _playerInventory;
        private void Start()
        {
            _playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
        }
        public bool Interact()
        {
            _playerInventory.LevelUp();
            Destroy(gameObject);
            return true;
        }
    }
}