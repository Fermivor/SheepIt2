using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManagerCustom : Prototype.NetworkLobby.LobbyManager
{


    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("Disconnection handle");
        GameData.INSTANCE.CmdDeletePlayerInfoObsolete(conn.connectionId);
        base.OnServerDisconnect(conn);
    }


    public override void OnServerSceneChanged(string sceneName)
    {        
        base.OnServerSceneChanged(sceneName);
        GameManager.INSTANCE.Init();
    }

}
