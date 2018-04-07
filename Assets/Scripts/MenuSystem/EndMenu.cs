using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : Menu {

    [SerializeField]
    GameObject m_PrefabPlayerInfoDisplayer;


    [SerializeField]
    GameObject m_TabDisplay;

    private void OnEnable()
    {
         if(!GameData.INSTANCE || m_TabDisplay.transform.childCount == GameData.INSTANCE.GetNumberPlayer() )
        {
            return;
        }
        //start get score from gamemanager
        Utils.DestroyChilds(m_TabDisplay.transform);
        Debug.Log("Add childs " + GameData.INSTANCE.GetNumberPlayer());
        foreach (PlayerInfo info in GameData.INSTANCE.GetPlayerInfoListByScore())
        {
            GameObject go = GameObject.Instantiate(m_PrefabPlayerInfoDisplayer);
            go.transform.parent = m_TabDisplay.transform;
            go.GetComponent<PlayerInfoDisplayer>().SetPlayerInfo(info);
        }
    }

    private void OnDisable()
    {
    }

    public override float GetAlphaBack()
    {
        return 1f;
    }


}
