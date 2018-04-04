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

    private void Start()
    {
        m_timer = TimerFactory.INSTANCE.getTimer();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void StartTimer(float a_time, Action a_callback = null)
    { 
        m_callback = a_callback;
        RpcStartTimer(a_time);
    }


    [ClientRpc]
    void RpcStartTimer(float a_time)
    {
        m_timer.StartTimer(a_time);
    }

    private void Update()
    {
        m_display.text = Math.Ceiling(m_timer.GetTimeLeft()) + "";
        if (m_timer.IsTimeUp())
        {
            if (m_callback != null)
            {
                m_callback.Invoke();
            }
            CloseMenu();
        }
    }

    public override float GetAlphaBack()
    {
        return 0.8f;
    }


}
