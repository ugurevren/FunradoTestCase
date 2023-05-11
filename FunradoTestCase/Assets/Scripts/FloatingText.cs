using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    private Transform _mainCameraTransform;
    private Transform _unitTransform;
    private Transform _worldSpaceCanvasTransform;
    
    private TextMeshProUGUI _text;

    public Vector3 offset;
    // Start is called before the first frame update
    private void Start()
    {
        _mainCameraTransform= Camera.main.transform;
        _unitTransform = transform.parent;
        _worldSpaceCanvasTransform = GameObject.FindObjectOfType<Canvas>().transform;
        transform.SetParent(_worldSpaceCanvasTransform);
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        Transform transform1;
        (transform1 = transform).rotation = Quaternion.LookRotation(transform.position - _mainCameraTransform.position);
        transform1.position = _unitTransform.position + offset;
    }
    
    public void SetText(string text)
    {
      
        _text.text = text;
        Debug.Log("Text set");
    }
}
