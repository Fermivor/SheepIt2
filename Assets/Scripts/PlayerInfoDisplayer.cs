using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoDisplayer : MonoBehaviour {

    [SerializeField]
    Text m_name;

    [SerializeField]
    Text m_score;

    PlayerInfo m_playerInfo;
    // Update is called once per frame
    void Update () {
        GetComponent<RectTransform>().localScale = Vector3.one;
        //Debug.Log(m_playerInfo);
        if (m_playerInfo)
        {
            m_name.text = m_playerInfo.GetName();
            m_score.text = m_playerInfo.Score + "";
        }
	}

    public void SetPlayerInfo(PlayerInfo a_playerInfo)
    {
        m_playerInfo = a_playerInfo;
    }

}
