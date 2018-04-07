using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PigStrategy : AbilityStrategy
{

    [SerializeField]
    GameObject m_mudBall;

    // Movement
    public override void PlayerMovement()
    {
        base.PlayerMovement();
    }


    // Ability1: spawn a Fence
    protected override void Ability1()
    {
        Vector3 position = m_player.transform.position;
        Matrix4x4 m = Matrix4x4.Translate(m_player.transform.right);
        position = m.MultiplyPoint3x4(position);

        Quaternion rotation = m_player.transform.rotation;
        rotation *= Quaternion.Euler(Vector3.forward * 90);

        GameManager.INSTANCE.SpawnObject(m_mudBall, position, rotation);
    }

    // Ability2
    protected override void Ability2() { }

    // Death
    public override void PlayerDeath() { }

}
