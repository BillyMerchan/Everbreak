using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    // Reading input
    PlayerControls playerControls;

    [SerializeField] Vector2 movementInput;
    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;
    [SerializeField] public float moveAmount;

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
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        // if object is destroyed, unsubscribe from this event
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput(){
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        // returns absolute number and clamps down (0-1) for total movement amount
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput)+Mathf.Abs(horizontalInput));
        
        // Clamps to walk
        if (moveAmount <= 0.5 && moveAmount > 0){
            moveAmount = 0.5f;
        
        // Clamps to run
        } else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }
    }

    // Controlling character
}
