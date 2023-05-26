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
        _worldSpaceCanvasTransform = WorldCanvas.Instance.transform;
        transform.SetParent(_worldSpaceCanvasTransform);
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_unitTransform== null)
        {
            // if the unit is destroyed, destroy the floating text
            Destroy(gameObject);
        }
        Transform transform1; // the transform of the floating text
        // make the floating text always face the camera
        (transform1 = transform).rotation = Quaternion.LookRotation(transform.position - _mainCameraTransform.position); 
        transform1.position = _unitTransform.position + offset; // set the position of the floating text
        
        
    }
    public void SetText(string text)
    {
        // set the text of the floating text
        _text.text = text;
    }
}
