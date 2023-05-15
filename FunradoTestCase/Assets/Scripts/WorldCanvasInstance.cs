using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasInstance : MonoBehaviour
{
private static WorldCanvasInstance _instance;
    public static WorldCanvasInstance Instance => _instance;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
