﻿using Prototype.NetworkLobby;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public enum AnimalType { SHEEP, WOLF}

public class GameManager : NetworkBehaviour
{

    public static GameManager INSTANCE;
    GameObject[] m_players;

    [SerializeField]
    GameObject m_spawnObjects;

	HUDManager m_hud;

	Timer m_timerRound;
	float m_roundMaxTime = 1000;

    NetworkStartPosition[] spawnPoints;

    int m_preda = -1;
    int currentSpawn = 0;

    void Start()
    {
        if (INSTANCE != null && INSTANCE != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            INSTANCE = this;
            DontDestroyOnLoad(this);
        }

        m_timerRound = TimerFactory.INSTANCE.getTimer();
        
    }

	void Update(){
		if (m_timerRound.IsTimerRunning()) {
			//Display time in UI

			float timeLeft = m_timerRound.GetTimeLeft();
			string minLeft = ((int)timeLeft / 60).ToString ();
			string secLeft = ((int)timeLeft % 60).ToString ();

			m_hud.RpcSetTimerTime (minLeft + ":" + secLeft);
		}
	}

    public void BeginGame()
    {
		m_hud = GameObject.FindGameObjectWithTag ("UI").GetComponent<HUDManager> ();
        StartCoroutine(BeginGameCoroutine());
    }

    IEnumerator BeginGameCoroutine()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("BEGIN GAME");


        List<LobbyPlayer> _lobbyPlayerList = LobbyPlayerListCustom.GetInstance().GetPlayerList();
        m_players = GameObject.FindGameObjectsWithTag("Player");
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        currentSpawn = 0;
        GameData.INSTANCE.RpcRetrievePlayerInfo();


        foreach (GameObject go in m_players)
		{
            PlayerInfo playerInfo = go.GetComponent<PlayerInfo>();
            LobbyPlayer lobbyPlayer = _lobbyPlayerList.Find(o => o.connectionToClient.connectionId == playerInfo.GetPlayerId());
            //Debug.Log(playerInfo.GetComponent<NetworkInstanceId>());

            //Debug.Log(lobbyPlayer.playerColor);
            playerInfo.setData(lobbyPlayer.playerColor, lobbyPlayer.playerName);
            //GameData.INSTANCE.GetPlayerInfo(go.GetComponent<NetworkIdentity>().clientAuthorityOwner.connectionId);

            //go.GetComponent<PlayerController>().RpcDisplayMyColor(playerInfo._playercolor);
        }

        m_preda = -1;
        StartRound();

    }

    private void DestroyChilds(Transform a_transform)
    {
        for( int i = a_transform.childCount - 1 ; i >= 0 ; --i )
        {
            Destroy(a_transform.GetChild(i).gameObject);
        }
    }

    private void StartRound()
    {
        DestroyChilds(m_spawnObjects.transform);
  

        m_preda++;
        if (m_preda >= m_players.Length)
        {
            //tout le monde a été prédateur
            BeginGame();
            return;
        }
        for (int i = 0; i < m_players.Length; i++)
        {
            AnimalType type;
            if(i == m_preda)
            {
                type = AnimalType.WOLF;
                m_players[i].GetComponent<PlayerController>().RpcSetPredator(true);
            }
            else
            {
                type = AnimalType.SHEEP;
                m_players[i].GetComponent<PlayerController>().RpcSetPredator(false);
            }
            m_players[i].GetComponent<PlayerController>().RpcSetSkin(type);
            m_players[i].GetComponent<PlayerController>().RpcSetPosition(spawnPoints[currentSpawn%spawnPoints.Length].transform.position);
            currentSpawn++;
        }

		m_timerRound.StartTimer (m_roundMaxTime, () => { StartRound(); });
    }


    [Command]
    public void CmdAddPoint(int a_predator, int a_victim)
    {
        PlayerController victim = GameData.INSTANCE.GetPlayerInfo(a_victim).gameObject.GetComponent<PlayerController>();
        victim.RpcDestroyYourSkin();
        victim.RpcDestroyYourAbility();
        PlayerInfo predaInfos = GameData.INSTANCE.GetPlayerInfo (a_predator);
		predaInfos._playerScore++;


        //Temp round stoping criteria
        if (predaInfos._playerScore == m_players.Length -1)
        {
            StartRound();
        }
    }

    public void SpawnObject(GameObject a_object, Vector3 a_position, Quaternion a_rotation)
    {
        if (isServer)
        {
            NetworkServer.Spawn(Instantiate(a_object, a_position, a_rotation, m_spawnObjects.transform));
        }
    }
}