using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This node is for moving the boss closer to the player.
public class MoveToPlayerNode : BehaviourNode
{
    //The player and boss objects.
    GameObject player;
    BossWhole boss;

    //MoveToPlayerNode takes in the player and boss as parameters.
    public MoveToPlayerNode(GameObject player, BossWhole boss)
    {
        this.player = player;
        this.boss = boss;
    }

    //The execute simply makes sure the player is in sight range and moves closer if so.
    public override BehaviourState Execute()
    {


        if (Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(boss.transform.position.x, 0, boss.transform.position.z)) <= boss.playerinSightRange)
        {
            boss.MoveTowardPlayer();
            boss.debugText.text = "Boss BT info: Moved Towards Player.";
            return BehaviourState.Succeed;
        }
        else
            return BehaviourState.Fail;
    }
}
