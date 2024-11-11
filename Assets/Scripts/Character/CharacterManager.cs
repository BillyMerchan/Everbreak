using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour;
{
    public CharacterController characterController;

    CharacterNetworkManager CharacterNetworkManager;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
    }

    protected virtual void Update()
    {
        // If character is being controlled from our side, assign psition to our transform
        if(isOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
        }
        // If character is being controlled from elsewhere , assign its position locally by the position of it's network transform
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position,
            characterNetworkManager.networkPosition.Value,
            ref characterNetworkManager.networkPositionVelocity,
            characterNetworkManager.networkPositionSmoothTime)
        }
    }
}
