using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterNetworkManager : NetworkBehaviour
{
    CharacterManager character;
    [Header("Position")]
    public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public Vector3 networkPositionVelocity;
    public float networkPositionSmoothTime = 0.1f;
    public float networkRotationSmoothTime = 0.1f;

    protected virtual void Awake()
    {

    }

    // ServerRPC's are Client -> Host functions
    // [ServerRPC]
    // public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
    // {
    //     // if character is host/server, then call client RPC
    //     if (IsServer)
    //     {
    //         PlayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
    //     }
    // }

    // [ClientRPC]
    // // client rpc is sent to all clients from the server
    // public void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
    // {
    //     // make  asingleton to not run character who sent it (dont play animaton twice)
    //     if (clientID != NetworkManager.Singleton.LocalClientId)
    //     {
    //         PerformActionAnimationFromServer(animationID, applyRootMotion);
    //     }            
    // }
    private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(animationID, 0.2f);
    }
}
