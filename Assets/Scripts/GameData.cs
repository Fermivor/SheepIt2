using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameData : NetworkBehaviour
{
    List<PlayerInfo> m_playerList = new List<PlayerInfo>();
    static public GameData INSTANCE;

    int m_uniqueID = 0;

    [SyncVar(hook = "OnIsGamePaused")]
    bool m_isGamePaused;


    public bool IsGamePaused
    {
        get
        {
            return m_isGamePaused;
        }

        set
        {
            m_isGamePaused = value;
        }
    }


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

    void OnIsGamePaused(bool a_bool)
    {
        Debug.Log("ChangeIsGamePaused " + a_bool);
        m_isGamePaused = a_bool;
    }


    [ClientRpc]
    public void RpcRetrievePlayerInfo()
    {
        if (!isServer)
        {
            RetrievePlayerInfo();
        }
    }

    public void RetrievePlayerInfo()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in players)
        {
            if (isServer)
            {
                go.GetComponent<PlayerInfo>().UniqueId = m_uniqueID;
                ++m_uniqueID;
            }
            AddPlayerInfo(go.GetComponent<PlayerInfo>());
        }
    }



    void AddPlayerInfo(PlayerInfo a_playerInfo)
    {
        if (!m_playerList.Contains(a_playerInfo))
        {
            m_playerList.Add(a_playerInfo);
        }
    }

    [Command]
    public void CmdDeletePlayerInfoObsolete(int a_connectionId)
    {
        Debug.Log("Delete Obsolete");
        for(int i = m_playerList.Count - 1; i >= 0; --i)
        {
            if(m_playerList[i].GetPlayerId() == a_connectionId)
            {
                Debug.Log("Destroy PlayerInfo " + i);
                RpcDeletePlayerInfoObsolete(m_playerList[i].UniqueId);
                m_playerList.RemoveAt(i);


            }
        }
    }

    [ClientRpc]
    public void RpcDeletePlayerInfoObsolete(int a_uniqueId)
    {
        for (int i = m_playerList.Count - 1; i >= 0; --i)
        {
            if (m_playerList[i].UniqueId == a_uniqueId)
            {
                Debug.Log("Destroy PlayerInfo " + i);
                m_playerList.RemoveAt(i);
                break;
            }
        }

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

    public List<PlayerInfo> GetPlayerInfoListByScore()
    {
        m_playerList.Sort((l, r) =>  r.Score.CompareTo(l.Score));

        return m_playerList;
    }


    public int GetNumberPlayer()
    {
        return m_playerList.Count;
    }

    private void Update()
    {
    }

}
