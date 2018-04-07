using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadingMenu : Menu {

    [SerializeField]
    Text m_display;

    int m_limit = 0;
    float m_t = 0;
    private void Start()
    {
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void Update()
    {
        m_t += Time.deltaTime;
        if(m_t > 1)
        {
            m_t = 0;
            m_limit = ++m_limit % 3;
        }
        m_display.text = "LOADING ";
        for (int i = 0; i < m_limit; i++)
        {
            m_display.text += ". ";
        }
    }

    public override float GetAlphaBack()
    {
        return 1.0f;
    }


}
