﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameData : NetworkBehaviour
{
    List<PlayerInfo> m_playerList = new List<PlayerInfo>();
    static public GameData INSTANCE;
    void Start()
    {
        if (INSTANCE != null && INSTANCE != this)
        {
            Destroy(gameObject);
        }
        else
        {
            INSTANCE = this;
        }
    }

    [ClientRpc]
    public void RpcRetrievePlayerInfo()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject go in players)
        {
            AddPlayerInfo(go.GetComponent<PlayerInfo>());
        }
    }

    void AddPlayerInfo(PlayerInfo a_playerInfo)
    {
        m_playerList.Add(a_playerInfo);
    }

    public PlayerInfo GetPlayerInfo(int iConnectionID)
    {
        PlayerInfo target;
        target = m_playerList.Find(o => o.GetPlayerId() == iConnectionID);

        // Envoyer exception si target == null
        if (target == null)
        {
            Debug.Log("Player " + iConnectionID + " not found!");
            throw new System.MissingMemberException();

        }
        return target;
    }

    public List<PlayerInfo> GetPlayerInfoList()
    {
        return m_playerList;
    }

    private void Update()
    {
       /* if (m_playerList.Count > 0)
        {
            Debug.Log(m_playerList[0]._playercolor);
        }*/
    }


}
