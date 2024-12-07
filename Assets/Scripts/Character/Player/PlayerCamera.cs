using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public Camera cameraObject;
    public PlayerManager player;
    [SerializeField] Transform cameraPivotTransform;

    // Change these to tweak camera performance
    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 1;        // Bigger the number, longer for the camera to reach its position during movement
    [SerializeField] float leftAndRightRotationSpeed = 220;
    [SerializeField] float upAndDownRotationSpeed = 220;
    [SerializeField] float minimumPivot = -30;          // Lowest angle can look down
    [SerializeField] float maximumPivot = 60;           // Highest angle can look up
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;   

    // Just displays camera values
    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;   // Used for camera collisions (moves the camera to this position when colliding)
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition;    // Values used for camera collision
    private float targetCameraZPosition;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            // Follow Player
            HandleFollowTarget();
            // Rotate Around the Player
            HandleRotations();
            // Collide with the environment
            HandleCollisions();
        }

    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraZPosition = Vector3.SmoothDamp(
            transform.position,
            player.transform.position, 
            ref cameraVelocity, 
            cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraZPosition;
    }

    private void HandleRotations()
    {
        // If locked on, force rotation towrads target
        // else rotate normally
        
        // normal rotations
        // rotate left and right based on horizontal movement of mouse
        leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        // rotate up and down based on vertical movement of mouse
        upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        // clamp upAndDownLookAngle
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        // Rotate this game object on the y axis (left and right)
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        // Rotate this pivot gameobject up and down:w

        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        targetCameraZPosition 4= cameraZPosition;
        RaycastHit hit;
        // direction for collision
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        // Check if object is in front of the camera, from desired direction
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            // if there is, check distance
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }
        // if our target position is less than collision radius, subtract collision radius
        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        // Apply final position using a Lerp
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }
}
