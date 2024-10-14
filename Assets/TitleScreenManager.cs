using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TitleScreenManager : MonoBehaviour
{
    public void Test()
    {
        console.log("test");
    }
    public void StartNetworkAsHostt()
    {
        NetworkManager.Singleton.StartHost();
    }
    public void StartNewGame()
    {
        StartCoroutine(WorldSaveGameManager.instance.LoadNewGame());
    }


}
