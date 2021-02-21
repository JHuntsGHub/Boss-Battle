using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The AOE Attack Node handles when the boss unleashes an aoe attack.
public class AOEAttackNode : BehaviourNode
{
    //the range of the aoe attack as set by PlayerRangeDecoratorNode.cs
    public float aoeAttackRange;

    //The player and boss objects.
    private GameObject player;
    private BossWhole boss;

    //The variable keeping track of the cooldown timer and the time of the the cooldown length below.
    private float aoeCooldown;
    private const float AOE_COOLDOWN_TIME = 6.0f;

    //AOEAttackNode take the player and boss as parameters.
    public AOEAttackNode(GameObject player, BossWhole boss)
    {
        this.player = player;
        this.boss = boss;
        aoeCooldown = 0;
    }

    //The execute function, determins if the aoeAttackRange is greater than the distance between the boss and player. If so, an AOE attack is launached provided the coodown is done.
    public override BehaviourState Execute()
    {
        if (Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(boss.transform.position.x, 0, boss.transform.position.z)) < aoeAttackRange)
        {
            if(aoeCooldown <= 0)
            {
                //boss.currentNode = this;
                aoeCooldown = AOE_COOLDOWN_TIME;
                boss.animator.SetBool("isAttacking", true);
                DoAction();
                return BehaviourState.Succeed;
            }
        }
        else
            aoeCooldown -= Time.deltaTime;
        return BehaviourState.Fail;
    }

    //DoAction() is possible to be called directly from the boss object if needed.
    public override void DoAction()
    {
        boss.debugText.text = "Boss BT info: AOE attacked.";
        boss.DoAOEAttack();
    }
}
