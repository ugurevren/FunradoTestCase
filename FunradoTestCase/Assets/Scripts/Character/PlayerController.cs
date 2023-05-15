using System;
using System.Collections;
using Interactable;
using UnityEngine;

namespace Character
{
    public class PlayerController : Character
    {
        public int maxLevel = 20;

        private void Awake()
        {
            _floatingTextObject = GetComponentInChildren<FloatingText>().gameObject;
        }

        private void Start()
        {
            _floatingText= _floatingTextObject.GetComponent<FloatingText>();
            UpdateLevelText(level);
        }
        

        private void OnTriggerEnter(Collider other)
        {
            var interactable = other.GetComponent(typeof(Interactable.Interactable)) as Interactable.Interactable;
            Debug.Log(other.name);
            interactable?.Interact();
            if (other.CompareTag("Enemy"))
            {
                Combat(other);
            }
        }

        public override void Combat(Collider other)
        {
            var enemy = other.GetComponent<Character>();
            enemy.Combat(gameObject.GetComponent<Collider>());
            
            int enemyLevel = enemy.level;

            if (enemyLevel > level)
            {
                gameObject.GetComponent<Animator>().enabled = false;
                // Player loses, restart game
                level = 1;
                // Reload current scene
                StartCoroutine(RestartScene(2f));
            }
            else if (enemyLevel < level)
            {
                // Destroy enemy and increase level
                if (level <= maxLevel - 5)
                {
                    level+=5;
                    UpdateLevelText(level);
                }
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