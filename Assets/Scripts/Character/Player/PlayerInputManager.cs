using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerManager player;

    // Reading input
    PlayerControls playerControls;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] Vector2 movementInput;
    public float horizontalInput;
    public float verticalInput;
    public float moveAmount;

    [Header("CAMERA INPUT")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("PLAYER ACTION INUT")]
    [SerializeField] bool dodgeInput = false;


    private void Awake(){
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }

    }

    private void Start(){

        DontDestroyOnLoad(gameObject);

        // Runs on scene change
        SceneManager.activeSceneChanged += OnSceneChange;

        instance.enabled = false;
        
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // loading into world scene and enabling player inputs/controls
        // otherwise (menu scene), disable player inputs
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            // If in world scene
            instance.enabled = true;
        }
        else
        {
            instance.enabled = false;
        }

    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;

        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        // if object is destroyed, unsubscribe from this event
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    // if we minimize or lower the window, stop adjusting inputs
    private void OnApplicationFocus(bool focus)
    {
        if(enabled)
        {
            if(focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
    }

    // Handle movement

    private void HandlePlayerMovementInput(){
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        // returns absolute number and clamps down (0-1) for total movement amount
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput)+Mathf.Abs(horizontalInput));
        
        // Clamps to walk
        if (moveAmount <= 0.5 && moveAmount > 0){
            moveAmount = 0.5f;
        
        // Clamps to run
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }
        if (player == null)
        {
            return;
        }

        // pass 0 on horizozntal, if not locked on no horizontal strafing
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);

        // if locked on pass horizontal as well
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    // Action

    // Controlling character
    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;
            // return (do nothing) if menu is open

            // Perform dodge
            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }
}
