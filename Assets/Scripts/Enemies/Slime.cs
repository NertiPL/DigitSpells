using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public override void WalkAnim()
    {

    }

    public override void CollideAnim()
    {
        player.GetComponent<PlayerController>().GetHit(dmgOnCol);
    }
}
