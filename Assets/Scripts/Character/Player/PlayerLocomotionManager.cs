using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;
    // Taken from input manager
    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float rotationSpeed = 15;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        // Grounded movement
        HandleGroundedMovement();
        HandleRotation();
        // Aerial movement
    }

    private void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;

        // Clamp Movements
    }

    private void HandleGroundedMovement()
    {
        GetVerticalAndHorizontalInputs();
        // Our move direction is based on camera perspective + inputs
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (PlayerInputManager.instance.moveAmount > 0.5f)
        {
            // move at running speed
            player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else if (PlayerInputManager.instance.moveAmount <= 0.5f)
        {
            // move at walking speed
            player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);

        }
    }

    private void HandleRotation()
    {
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    private void AttemptToPerformDodge()
    {
        // if moving when we dodge, perform roll
        if (moveAmount > 0) 
        {
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            rollDirection.y = 0;
            rollDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            // Performing roll animation
        }
        // if we are stationary, perform bacsktep
        else {
            // performing backstep animation
        }
    }
}
