using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfo  : NetworkBehaviour/*_ pour partager et sync de partout */
{

    [SyncVar(hook = "OnColorChange")]
	Color m_color = Color.black;
    [SyncVar(hook = "OnNameChange")]
    string m_name;
    [SyncVar(hook = "OnScoreChange")]
    int m_score;
    [SyncVar(hook = "OnIsAliveChange")]
    bool m_isAlive;
    [SyncVar(hook = "OnIsPredaChange")]
    bool m_isPreda;
    [SyncVar(hook = "OnIsPositionSetted")]
    bool m_isPositionSetted;
    [SyncVar(hook = "OnUniqueIdChange")]
    int m_uniqueId;


    public PlayerInfo(Color iPlayerColor, string iPlayerName)
    {
        m_color = iPlayerColor;
        m_name = iPlayerName;
        Score = 0;
    }

    public bool IsAlive
    {
        get
        {
            return m_isAlive;
        }

        set
        {
            m_isAlive = value;
        }
    }

    public int Score
    {
        get
        {
            return m_score;
        }

        set
        {
            m_score = value;
        }
    }

    public bool IsPreda
    {
        get
        {
            return m_isPreda;
        }

        set
        {
            m_isPreda = value;
        }
    }

    public int UniqueId
    {
        get
        {
            return m_uniqueId;
        }

        set
        {
            m_uniqueId = value;
        }
    }

    public bool IsPositionSetted
    {
        get
        {
            return m_isPositionSetted;
        }

        set
        {
            m_isPositionSetted = value;
        }
    }

    public string GetName()
    {
        return m_name;
    }

    public void IncrementScore()
    {
        ++Score;
    }

    [Command]
    public void CmdSetIsPositionSetted(bool a_bool)
    {
        IsPositionSetted = a_bool;
    }


    public void SetData(Color playerColor, string playerName)
    {
        m_color = playerColor;
        m_name = playerName;
        Score = 0;
    }

    public int GetPlayerId()
    {
        return connectionToClient == null ? connectionToServer.connectionId : connectionToClient.connectionId;
    }


    void OnColorChange(Color a_color)
    {
        Debug.Log("ChangeColor " + a_color);
        m_color = a_color;
    }

    void OnNameChange(String a_name)
    {
        Debug.Log("ChangeName " + a_name);
        m_name = a_name;
    }

    void OnScoreChange(int a_score)
    {
        Debug.Log("ChangeScore " + a_score);
        Score = a_score;
    }

    void OnIsAliveChange(bool a_bool)
    {
        Debug.Log("ChangeIsAlive " + a_bool);
        IsAlive = a_bool;
    }

    void OnIsPredaChange(bool a_bool)
    {
        Debug.Log("ChangeIsPreda " + a_bool);
        IsPreda = a_bool;
    }

    void OnIsPositionSetted(bool a_bool)
    {
        Debug.Log("ChangeIsPositionSetted " + a_bool);
        IsPositionSetted = a_bool;
    }
    
    void OnUniqueIdChange(int a_uniqueId)
    {
        Debug.Log("ChangeUniqueID " + a_uniqueId);
        UniqueId = a_uniqueId;
    }

}
