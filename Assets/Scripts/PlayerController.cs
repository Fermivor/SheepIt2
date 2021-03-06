﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	private AbilityStrategy _Strat;

    private void Start()
    {
        //Deactivate the main camera and activate the player camera
        if(isLocalPlayer)
        Camera.main.GetComponent<CameraFollow>().SetPlayerToFollow(gameObject);
    }


    [ClientRpc]
	public void RpcSetSkin(AnimalType type)
	{
		if (gameObject.GetComponentInChildren<Skin> () != null) {
			Destroy (gameObject.GetComponentInChildren<Skin> ().gameObject);
            Debug.Log("My children : " + transform.childCount);
		}
        if (gameObject.GetComponentInChildren<AbilityStrategy>() != null)
        {
            Destroy(gameObject.GetComponentInChildren<AbilityStrategy>().gameObject);
        }

        GameObject skin = SkinFactory.INSTANCE.getSkin(type);

        skin.transform.SetParent(gameObject.transform, false);

        //set corresponding strategy
            
        GameObject strategy = AbilityStrategyFactory.INSTANCE.getAbilityStrategy(type);
        strategy.transform.SetParent(gameObject.transform, false);
        _Strat = strategy.GetComponent<AbilityStrategy>();
        _Strat.Init(gameObject);
    }

    void OnCollisionEnter2D(Collision2D coll)
	{
        if (isServer)
        {
            if (coll.collider.CompareTag("PlayerSkin"))
            {
                bool isCollPredator = coll.gameObject.GetComponent<PlayerInfo>().IsPreda;
                //if this object is a predator and the collison is a prey
                if (!GetComponent<PlayerInfo>().IsPreda && isCollPredator)
                {
                   // DestroyYourSkin();
                  //  DestroyYourAbility();

                    Debug.Log("Collided between predator and prey");
                    GameManager.INSTANCE.AddPoint(coll.gameObject.GetComponent<NetworkIdentity>().clientAuthorityOwner.connectionId, gameObject.GetComponent<NetworkIdentity>().clientAuthorityOwner.connectionId);
                }
            }
        }
    
    }


    [ClientRpc]
    public void RpcDestroyYourAbility()
    {
        DestroyYourAbility();
    }

    void DestroyYourAbility()
    {
        if (_Strat)
        {
            Destroy(_Strat.gameObject);
        }
    }



    [ClientRpc]
    public void RpcDestroyYourSkin()
    {
        DestroyYourSkin();
    }

    void  DestroyYourSkin()
    {
        //desactiver le skin de la proie (à améliorer probablement)
        Skin skin = gameObject.GetComponentInChildren<Skin>();
        if (skin)
        {
            Destroy(skin.gameObject);
        }
    }


    [ClientRpc]
    public void RpcDisplayMyColor(Color color)
    {
        if (isLocalPlayer)
        {
            Camera.main.backgroundColor = color;
        }
    }

    void DisplayMyColor(Color color)
    {
        if (isLocalPlayer)
        {
            Camera.main.backgroundColor = color;
        }
    }

    [ClientRpc]
    public void RpcSetPosition(Vector3 a_position)
    {
        transform.position = a_position;
        if (isLocalPlayer)
        {
            GetComponent<PlayerInfo>().CmdSetIsPositionSetted(true);
        }
    }



    [ClientRpc]
    public void RpcFear(Vector3 a_position, float a_speed, float a_time)
    {
        if (isLocalPlayer)
        {
            _Strat.Fear(a_position, a_speed, a_time);
        }
    }

    [ClientRpc]
    public void RpcSlow(float a_slowMultiplier, float a_slowDuration)
    {
        if (isLocalPlayer)
        {
            _Strat.Slow(a_slowMultiplier, a_slowDuration);
        }
    }

    [Command]
    public void CmdTransmitInput(int a_keyCode)
    {
        if (!isServer)
        {
            return;
        }

        if((KeyCode)a_keyCode == KeyCode.A)
        {
            _Strat.UseAbility1();
        }
    }


    // Update is called once per frame
    void Update () {
		if (!isLocalPlayer || _Strat == null)
		{
			return;
		}

        if (GameData.INSTANCE.IsGamePaused)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3();
            GetComponent<Rigidbody2D>().angularVelocity = 0;
            return;
        }



		//ScoreMenu
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MenuManager.INSTANCE.OpenMenu(MENUTYPE.SCORE);
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            MenuManager.INSTANCE.CloseMenu();
        }
        
        //Ability1
        if (Input.GetKeyDown(KeyCode.A) && GetComponent<PlayerInfo>().IsAlive){
            CmdTransmitInput((int)KeyCode.A);
		}
	
		_Strat.PlayerMovement();

    }


}
