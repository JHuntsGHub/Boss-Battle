using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MeleeAttackNode determines when the boss should punch the player.
public class MeleeAttackNode : BehaviourNode
{
    //The player and boss objects.
    GameObject player;
    BossWhole boss;

    //The variable keeping track of the cooldown timer and the time of the the cooldown length below.
    private float attackCooldown;
    private const float ATTACK_COOLDOWN_TIME = 2.3f;

    //MeleeAttackNode takes the player and boss in as parameters.
    public MeleeAttackNode(GameObject player, BossWhole boss)
    {
        this.player = player;
        this.boss = boss;
        attackCooldown = 0;
    }

    //The execute function sees how close the boss is to the player. If close enough it will succeed, otherwise failure.
    public override BehaviourState Execute()
    {
        if (Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(boss.transform.position.x, 0, boss.transform.position.z)) <= boss.playerInMeleeRange)
        {
            if (attackCooldown <= 0)
            {
                attackCooldown = ATTACK_COOLDOWN_TIME;
                boss.animator.SetBool("isAttacking", true);
                DoAction();
                return BehaviourState.Succeed;
            }
        }
        else
            attackCooldown -= Time.deltaTime;
        return BehaviourState.Fail;
    }

    public override void DoAction()
    {
        boss.debugText.text = "Boss BT info: MeleeAttackSuccessful.";
        boss.DoBasicAttack();
    }
}
