using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Menu : NetworkBehaviour
{
	public void CloseMenu()
	{
		MenuManager.INSTANCE.CloseMenu();
	}

    [ClientRpc]
    protected void RpcCloseMenu()
    {
        CloseMenu();
    }


    public virtual float GetAlphaBack()
    {
        return 1.0f;
    }
}