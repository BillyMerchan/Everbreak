using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerLocomotionManager playerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();
        // Player specific functionality

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    protected override void Update()
    {
        base.Update();

        // If game object is not owned by us, do not control or edit it
        if(!IsOwner)
        {
            return;
        }
        // run every frame - handling movement
        playerLocomotionManager.HandleAllMovement();
    }
}
