using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PigStrategy : AbilityStrategy
{
    // Movement
    public override void PlayerMovement()
    {
        base.PlayerMovement();
    }


    // Ability1: spawn a Fence
    protected override void Ability1()
    {
        
    }

    // Ability2
    protected override void Ability2() { }

    // Death
    public override void PlayerDeath() { }

}
