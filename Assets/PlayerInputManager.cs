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

    // Controlling character
}
