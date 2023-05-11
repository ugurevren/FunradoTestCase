using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 playerVelocity;
    public float playerSpeed = 2.0f;
    private Joystick _joystick;
    private float _moving;
    Animator _animator;
    private Vector3 _move;
    
   
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        _joystick = Joystick.Instance;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _moving = _joystick.Direction.magnitude;
        _animator.SetFloat("moving",_moving);
        
        _move = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        _move.Normalize();
        
        // Rotate
        if (_move.magnitude > 0.1f) {
            Quaternion targetRotation = Quaternion.LookRotation(_move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360.0f * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        controller.Move((_move * playerSpeed + playerVelocity) * Time.deltaTime);
    }
}