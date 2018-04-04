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

    [ClientRpc]
    public void RpcStartTimer(float a_time)
    {
        m_timer.StartTimer(a_time);
    }

    private void Update()
    {
        m_display.text = m_timer.GetCurrentTime() + "";
    }

    public override float GetAlphaBack()
    {
        return 0.8f;
    }


}
