using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TimerMenu : Menu {

    [SerializeField]
    Text m_display;

    Timer m_timer;

    Action m_callback = null;

    bool m_everRun;
    
    private void Start()
    {
    }

    private void OnEnable()
    {
        m_everRun = false;
    }

    private void OnDisable()
    {
    }

    public void StartTimer(float a_time, Action a_callback = null)
    { 
        m_callback = a_callback;
        if (m_timer == null)
        {
            m_timer = TimerFactory.INSTANCE.getTimer();
        }
        m_timer.StartTimer(a_time);
        m_everRun = true;
    }


    [ClientRpc]
    void RpcSetTime(string a_time)
    {
        m_display.text = a_time;
    }


    private void Update()
    {
        if (isServer && m_everRun)
        {
            RpcSetTime(Math.Ceiling(m_timer.GetTimeLeft()) + "");
            if (m_timer.IsTimeUp())
            {
                if (m_callback != null)
                {
                    m_callback.Invoke();
                }
                RpcCloseMenu();
            }
        }
    }

    public override float GetAlphaBack()
    {
        return 0.8f;
    }


}
