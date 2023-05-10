using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int level = 1;
    public int maxLevel = 5;

    public AudioClip collectSound;
    public AudioClip loseSound;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            // Increase level and destroy collectable
            level++;
            if (level > maxLevel)
            {
                level = maxLevel;
            }
            Destroy(other.gameObject);

            // Play collect sound
            if (collectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collectSound);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            int enemyLevel = other.GetComponent<EnemyController>().level;

            if (enemyLevel > level)
            {
                // Player loses, restart game
                level = 1;
                // Reload current scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
                
                // Play lose sound
                if (loseSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(loseSound);
                }
            }
            else if (enemyLevel < level)
            {
                // Destroy enemy and increase level
                level++;
                if (level > maxLevel)
                {
                    level = maxLevel;
                }
                Destroy(other.gameObject);
            }
        }
    }
}