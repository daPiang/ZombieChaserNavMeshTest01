using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Adjust this to control the movement speed
    [SerializeField] private float rotationSpeed = 3f; // Adjust this for mouse rotation speed
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anims;
    [SerializeField] private float sprintMul;
    private float multiplier;
    private float mouseX;
    private bool hasReachedGoal, isBitten;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Get input from the player
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction relative to the camera
        Vector3 cameraForward = playerCamera.forward;
        Vector3 cameraRight = playerCamera.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        Vector3 movement = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        // Move the player
        rb.velocity = moveSpeed * multiplier * movement;

        // Rotate the player based on mouse input
        mouseX = Input.GetAxis("Mouse X");
        RotatePlayer();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            multiplier = sprintMul;
        }
        else multiplier = 1;

        // Handle animations
        HandleAnims();
    }

    private void OnCollisionEnter(Collision other) {
        if(other.transform.CompareTag("Zombie"))
        {
            isBitten = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Finish"))
        {
            hasReachedGoal = true;
        }
    }

    private void RotatePlayer()
    {
        Vector3 rotation = transform.localEulerAngles;
        rotation.y += mouseX * rotationSpeed;
        transform.localEulerAngles = rotation;
    }

    private void HandleAnims()
    {
        anims.SetBool("isMoving", IsMoving());
        anims.SetBool("isRunning", IsSprinting());
    }

    private bool IsMoving()
    {
        return rb.velocity != Vector3.zero;
    }

    private bool IsSprinting()
    {
        return multiplier != 1;
    }

    public bool HasReachedGoal()
    {
        return hasReachedGoal;
    }

    public bool IsBitten()
    {
        return isBitten;
    }
}
