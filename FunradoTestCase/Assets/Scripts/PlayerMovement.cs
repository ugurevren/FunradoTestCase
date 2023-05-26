using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField] private float playerSpeed = 2.0f;
    private Joystick _joystick; 
    private float _moving;
    private Animator _animator;
    private Vector3 _move;
    
   
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>(); // Get the CharacterController
        _joystick = Joystick.Instance; // Get the Joystick
        _animator = GetComponent<Animator>(); // Get the Animator
    }

    void Update()
    {
        _moving = _joystick.Direction.magnitude; // Get the magnitude of the joystick
        _animator.SetFloat("moving",_moving); // Set the moving parameter of the animator to the magnitude of the joystick
        _move = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical); // Get the direction of the joystick
        _move.Normalize(); // Normalize the direction of the joystick
       
        if (_move.magnitude > 0.1f) {
            // Rotate towards the direction of the joystick
            Quaternion targetRotation = Quaternion.LookRotation(_move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360.0f * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
        // Move the player
        controller.Move((_move * playerSpeed + playerVelocity) * Time.deltaTime);
    }
}