using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{
    // Singleton
    private static WorldCanvas _instance; // Instance of the WorldCanvas
    public static WorldCanvas Instance => _instance; // Get the instance of the WorldCanvas
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);    // Destroy the new instance if it is not null
        }
        else
        {
            _instance = this;   // Set the instance
        }
    }
}
