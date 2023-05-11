using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int level = 0;
    public int maxLevel = 5;
    
    public AudioClip collectSound;
    public AudioClip loseSound;
    public AudioSource audioSource;
    
    public FloatingText _floatingText;
    public GameObject _floatingTextObject;
    private void Awake()
    {
        _floatingTextObject = GetComponentInChildren<FloatingText>().gameObject;
    }

    private void Start()
    {
        _floatingText= _floatingTextObject.GetComponent<FloatingText>();
        _floatingText.SetText(level.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            LevelUp(other);
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
    

    private void LevelUp(Collider other)
    {
        // Increase level and destroy collectable
        if (level <= maxLevel)
        {
            level++;
            _floatingText.SetText(level.ToString());
        }
        if (other)
        {
            Destroy(other.gameObject);
        }
        

        // Play collect sound
        if (collectSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
    }
    
    //Editor class
    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            PlayerController playerController = (PlayerController) target;
            if (GUILayout.Button("Level Up"))
            {
                playerController.LevelUp(null);
            }
        }
    }
    #endif
}