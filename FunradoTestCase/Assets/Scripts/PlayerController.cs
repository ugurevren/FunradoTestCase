using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, ICharacter
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
        ((ICharacter) this).UpdateLevel(level);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            LevelUp(other);
        }
        else if (other.CompareTag("Enemy"))
        {
            ((ICharacter) this).Combat(other);
        }
    }

    void ICharacter.Combat(Collider other)
    {
        int enemyLevel = other.GetComponent<EnemyController>().enemyLevel;

        if (enemyLevel > level)
        {
            // Player loses, restart game
            level = 1;
            // Reload current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
                .buildIndex);

            // Play lose sound
            if (loseSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(loseSound);
            }
        }
        else if (enemyLevel < level)
        {
            // Destroy enemy and increase level
            if (level <= maxLevel)
            {
                level++;
                ((ICharacter) this).UpdateLevel(level);
            }
            Destroy(other.gameObject);
        }
    }


    void ICharacter.UpdateLevel(int level)
    {
        _floatingText.SetText(level.ToString());
    }

    private void LevelUp(Collider other)
    {
        // Increase level and destroy collectable
        if (level <= maxLevel)
        {
            level++;
            ((ICharacter) this).UpdateLevel(level);
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