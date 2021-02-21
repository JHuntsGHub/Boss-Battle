using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The WanderNode controls if the boss is in a wandering state.
public class WanderNode : BehaviourNode
{
    //The player object
    GameObject player;

    //The boss object
    BossWhole boss;

    //WanderNode takes the player and boss as parameters.
    public WanderNode(GameObject player, BossWhole boss)
    {
        this.player = player;
        this.boss = boss;
    }

    //The WanderNode's execute checks if the player is near. If so, fails, otherwise succeeds.
    public override BehaviourState Execute()
    {
        if (Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(boss.transform.position.x, 0, boss.transform.position.z)) > boss.playerinSightRange)
        {
            boss.DoWander();
            boss.debugText.text = "Boss BT info: Wandering succeded.";
            return BehaviourState.Succeed;
        }
        else
            return BehaviourState.Fail;
    }
}
