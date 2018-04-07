using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PigMudBall : NetworkBehaviour{


    [SerializeField]
    private float m_velocity = 6;

    [SerializeField]
    private float m_slowMultiplier = 0.5f;

    [SerializeField]
    private float m_slowDuration = 1.5f;

    private Rigidbody2D m_rb;


    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        m_rb.velocity = -transform.up * m_velocity;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (isServer)
        {
            if (coll.CompareTag("PlayerSkin"))
            {
                PlayerController pc = coll.gameObject.transform.parent.gameObject.GetComponent<PlayerController>();
                pc.RpcSlow(m_slowMultiplier, m_slowDuration);

                if (this != null) Destroy(gameObject);
            }
            else
            {
                if (this != null) Destroy(gameObject);
            }
        }
    }
}
