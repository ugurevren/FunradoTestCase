using System;
using System.Collections;
using Interactable;
using UnityEngine;

namespace Character
{
    public class PlayerController : Character
    {
        [SerializeField] protected internal int levelIncreaseRate;
        private void Awake()
        {
            floatingTextObject = GetComponentInChildren<FloatingText>().gameObject; // Get the floating text object.
        }

        private void Start()
        {
            floatingText= floatingTextObject.GetComponent<FloatingText>();  // Get the floating text component.
            UpdateLevelText(level); // Update the level text.
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // If the player collides with an interactable object, interact with it.
            var interactable = other.GetComponent<IInteractable>();
            interactable?.Interact();
            if (other.CompareTag("Enemy"))
            {
                // If the player collides with an enemy, start combat.
                Combat(other);
            }
        }

        public override void Combat(Collider other)
        {
            var enemy = other.GetComponent<Character>();    // Get the enemy character.
            enemy.Combat(gameObject.GetComponent<Collider>());  // Start combat with the enemy.
            
            int enemyLevel = enemy.level;   // Get the level of the enemy.

            if (enemyLevel > level)
            {
                // Destroy player and restart game
                gameObject.GetComponent<Animator>().enabled = false;
                level = 1;
                // Reload current scene
                StartCoroutine(RestartScene(2f));
            }
            else if (enemyLevel < level)
            {
                // Destroy enemy and increase level
                level+=levelIncreaseRate;
                UpdateLevelText(level);
            }
        }
        private IEnumerator RestartScene(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
                .buildIndex-1);
        }
    }
}