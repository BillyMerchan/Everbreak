using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    
    CharacterNetworkManager characterNetworkManager;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool applyRootMotion = false;


    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        // If character is being controlled from our side, assign psition to our transform
        if(IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        // If character is being controlled from elsewhere , assign its position locally by the position of it's network transform
        else
        {
            // Position
            transform.position = Vector3.SmoothDamp(transform.position,
                characterNetworkManager.networkPosition.Value,
                ref characterNetworkManager.networkPositionVelocity,
                characterNetworkManager.networkPositionSmoothTime);

            // Rotation
            transform.rotation = Quaternion.Slerp(transform.rotation,
                characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {
        
    }
}
