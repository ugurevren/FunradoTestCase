using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, ICharacter
{
    public int enemyLevel = 1;
    
    public FloatingText _floatingText;
    public GameObject _floatingTextObject;
    
    private void Awake()
    {
        _floatingTextObject = GetComponentInChildren<FloatingText>().gameObject;
    }

    private void Start()
    {
        _floatingText= _floatingTextObject.GetComponent<FloatingText>();
        ((ICharacter) this).UpdateLevel(enemyLevel);
    }
    public void Combat(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateLevel(int level)
    {
        _floatingText.SetText(level.ToString());
    }
}
