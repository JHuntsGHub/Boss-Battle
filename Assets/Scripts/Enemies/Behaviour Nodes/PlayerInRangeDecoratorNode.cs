using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlayerInRangeDecoratorNode is a decorator node parenting the aoe attack node. It sets the range of the aoe attack at a random point between lower and upper bounds.
public class PlayerInRangeDecoratorNode : BehaviourNode
{
    private AOEAttackNode aoeAttackNode;
    private BossWhole boss;

    //The parameters are the AOE attack node and the boss.
    public PlayerInRangeDecoratorNode(AOEAttackNode aoeAttackNode, BossWhole boss)
    {
        this.aoeAttackNode = aoeAttackNode;
        this.boss = boss;
    }

    public override BehaviourState Execute()
    {
        aoeAttackNode.aoeAttackRange = Random.Range(boss.aoeLowerRange, boss.aoeUpperRange);
        return aoeAttackNode.Execute();
    }
}
