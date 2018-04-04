using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum MENUTYPE {NOTHING, MAIN, END, GAME, PAUSE, SCORE, CHRONO};


[System.Serializable]
public class MenuEntry
{
	public MENUTYPE m_type;
	public Menu m_menu;
}
public class MenuManager : NetworkBehaviour {
	[SerializeField]
	List<MenuEntry> m_listMenu;
	static public MenuManager INSTANCE;
	MENUTYPE m_currentMenu = MENUTYPE.NOTHING;
	[SerializeField]
	MENUTYPE m_startMenu;


	// Use this for initialization
	void Awake () {
		if (INSTANCE != null && INSTANCE != this)
		{
			Destroy(gameObject);
		}
		else
		{
			INSTANCE = this;
			foreach(MenuEntry entry in m_listMenu)
			{
				entry.m_menu.gameObject.SetActive(false);
			}
			OpenMenu(m_startMenu);
		}
	}

	// Update is called once per frame
	void Update () {

		MenuEntry menuEntry = INSTANCE.m_listMenu.Find(x => x.m_type == INSTANCE.m_currentMenu);
		if (menuEntry != null)
		{
			MenuBackGround.INSTANCE.SetAlpha( menuEntry.m_menu.GetAlphaBack() );
        }
        else
        {
            MenuBackGround.INSTANCE.SetAlpha(0.0f);
        }

    }

	public Menu OpenMenu(MENUTYPE a_type)
	{
		MenuEntry menuEntry = m_listMenu.Find(x => x.m_type == a_type);
        if (menuEntry != null && a_type != m_currentMenu)
		{
            CloseMenu();
            menuEntry.m_menu.gameObject.SetActive(true);
			m_currentMenu = a_type;
		}
        return menuEntry.m_menu;

    }

    [ClientRpc]
    public void RpcOpenMenu(MENUTYPE a_type)
    {
        OpenMenu(a_type);
    }



    public void CloseMenu()
	{
		MenuEntry menuEntry = m_listMenu.Find(x => x.m_type == m_currentMenu);
		if(menuEntry != null)
		{
			menuEntry.m_menu.gameObject.SetActive(false);
			m_currentMenu = MENUTYPE.NOTHING;
		}
	}


}