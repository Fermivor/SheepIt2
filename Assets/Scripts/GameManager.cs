using Prototype.NetworkLobby;
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

    [SerializeField]
    GameObject m_spawnObjectsContainer;

	HUDManager m_hud;

	Timer m_timerRound;
    [SerializeField]
    float m_roundMaxTime = 40;

    NetworkStartPosition[] m_spawnPoints;

    bool m_isInit = false;

    int m_preda = -1;
    int currentSpawn = 0;
    int pilou = 0;
    bool m_roundStarted = false;


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
			m_hud.RpcSetTimerTime (m_timerRound + "");

            
		}
        /* List<PlayerInfo> list = GameData.INSTANCE.GetPlayerInfoList();
         foreach(PlayerInfo info in list)
         {
         //    info.IncrementScore();
         }*/


        List<PlayerInfo> alive = GameData.INSTANCE.GetPlayerInfoList().FindAll(o => o.IsAlive && !o.IsPreda);
        List<PlayerInfo> predas = GameData.INSTANCE.GetPlayerInfoList().FindAll(o => o.IsPreda); 
        if ((alive.Count == 0 || predas.Count == 0) && m_roundStarted)
        {
            Debug.Log("Add Point end of round");

            EndOfRound();
        }

    }


    public void Init()
    {
        if (m_isInit)
        {
            return;
        }
        m_hud = GameObject.FindGameObjectWithTag("UI").GetComponent<HUDManager>();

        StartCoroutine(InitCoroutine());


        m_isInit = true;
    }


    IEnumerator InitCoroutine()
    {
        yield return new WaitForSeconds(2);
        List<LobbyPlayer> _lobbyPlayerList = LobbyPlayerListCustom.GetInstance().GetPlayerList();

        GameData.INSTANCE.RpcRetrievePlayerInfo();
        //Rpc notsynchronize so get on server separatly
        GameData.INSTANCE.RetrievePlayerInfo();

        List<PlayerInfo> list = GameData.INSTANCE.GetPlayerInfoList();
        m_spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        foreach (PlayerInfo playerInfo in list)
        {
            LobbyPlayer lobbyPlayer = _lobbyPlayerList.Find(o => o.connectionToClient.connectionId == playerInfo.GetPlayerId());
            playerInfo.setData(lobbyPlayer.playerColor, lobbyPlayer.playerName);
        }



        BeginGame();
    }


    void BeginGame()
    {
        Debug.Log("BEGIN GAME "  + pilou);
        ++pilou;

        List<PlayerInfo> list = GameData.INSTANCE.GetPlayerInfoList();
        foreach (PlayerInfo info in list)
        {
            info.Score = 0;
        }

        currentSpawn = 0;
        m_preda = -1;

        StartRound();

    }



    private void StartRound()
    {
        Util.DestroyChilds(m_spawnObjectsContainer.transform);
  

        ++m_preda;
        Debug.Log("Preda " + m_preda);
        if (m_preda >= GameData.INSTANCE.GetNumberPlayer())
        {
            //tout le monde a été prédateur
            BeginGame();
            return;
        }

        List<PlayerInfo> list = GameData.INSTANCE.GetPlayerInfoList();
        Debug.Log("count players" + list);
        int i = 0;
        foreach (PlayerInfo playerInfo in list)
        {
            Debug.Log("NamePlayer " + playerInfo.GetName());
            playerInfo.gameObject.GetComponent<PlayerController>().RpcSetPosition(m_spawnPoints[currentSpawn % m_spawnPoints.Length].transform.position);
            AnimalType type;
            if(i == m_preda)
            {
                playerInfo.IsPreda = true;
                type = AnimalType.WOLF;
            }
            else
            {
                playerInfo.IsPreda = false;
                type = AnimalType.SHEEP;
            }
            playerInfo.gameObject.GetComponent<PlayerController>().RpcSetSkin(type);
            playerInfo.IsAlive = true;
            ++currentSpawn;
            ++i;
        }

		m_timerRound.StartTimer (m_roundMaxTime, () => { EndOfRound(); /*StartRound();*/ });
        StartCoroutine(StartRoundCoroutine());

    }

    IEnumerator StartRoundCoroutine()
    {
    
        yield return new WaitForSeconds(2.0f);
        Debug.Log("StartRound");
        m_roundStarted = true;

    }

    private void EndOfRound()
    {

        Debug.Log("EndOfRound");
        m_roundStarted = false;
        List<PlayerInfo> list = GameData.INSTANCE.GetPlayerInfoList();
        foreach (PlayerInfo playerInfo in list)
        {
            if(playerInfo.IsAlive && !playerInfo.IsPreda)
            {
                playerInfo.IncrementScore();
            }
        }
        StartRound();
    }

    //Genre cait un confirm des côté client avant de lancer le round mais gaffe aux deco dans c emoment


    public void AddPoint(int a_predator, int a_victim)
    {
        if (!m_roundStarted)
        {
            return;
        }

        Debug.Log("add Point");
        PlayerController victim = GameData.INSTANCE.GetPlayerInfo(a_victim).gameObject.GetComponent<PlayerController>();
        victim.RpcDestroyYourSkin();
        victim.RpcDestroyYourAbility();
        GameData.INSTANCE.GetPlayerInfo(a_victim).IsAlive = false;
        PlayerInfo predaInfos = GameData.INSTANCE.GetPlayerInfo (a_predator);
		predaInfos.IncrementScore();

      
    }

    public void SpawnObject(GameObject a_object, Vector3 a_position, Quaternion a_rotation)
    {
        if (isServer)
        {
            NetworkServer.Spawn(Instantiate(a_object, a_position, a_rotation, m_spawnObjectsContainer.transform));
        }
    }
}