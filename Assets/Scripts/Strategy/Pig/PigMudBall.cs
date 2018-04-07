using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PigMudBall : NetworkBehaviour{
    //Timer m_timer;

    void Start()
    {
        if (isServer)
        {
            //m_timer = TimerFactory.INSTANCE.getTimer();
        }
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right);
    }
	

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (isServer)
        {
            if (coll.collider.CompareTag("PlayerSkin"))
            {

            }
            else
            {
                if (this != null) Destroy(gameObject);
            }
        }
    }
}
