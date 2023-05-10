using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 playerVelocity;

    public float playerSpeed = 2.0f;
    private Joystick _joystick;
    private float moving;
    Animator _animator;
    private Vector3 move;

    private float horizontalInput;
    private float verticalInput;
    
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        _joystick = Joystick.Instance;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        moving = _joystick.Direction.magnitude;
        _animator.SetFloat("moving",moving);
        
        // Get input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Combine joystick and keyboard input
        move = new Vector3(_joystick.Horizontal + horizontalInput, 0, _joystick.Vertical + verticalInput);
        move.Normalize();
        
        // Rotate
        if (move.magnitude > 0.1f) {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360.0f * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        controller.Move((move * playerSpeed + playerVelocity) * Time.deltaTime);
    }
}