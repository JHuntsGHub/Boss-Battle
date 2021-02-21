using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//BossWhole is the class that controls the boss npc
public class BossWhole : Enemy
{
    //RootNode is the root node of the behaviour tree.
    private SelectorNode RootNode;

    //currentNode could be used if there are any behaviours that need to finish before continuing.
    //public BehaviourNode currentNode;

    //The game object that is spawned when the boss uses their aoe attack.
    public GameObject AoePrefab;

    //The text on the HUD canvas displaying the boss' health.
    public Text HPText;

    //The debug text on the HUF showing state.
    public Text debugText;

    //How far the boss can see the player.
    public float playerinSightRange = 30f;

    //two variable for the decorator node. They are for determining the range the AOE attack can be used from
    public float aoeLowerRange = 15f, aoeUpperRange = 30f;

    //The distance from the player needed for a melee attack.
    public float playerInMeleeRange = 3f;

    //The strength of an aoe attack. Damage is low as the player is unable to dodge.
    private float aoeAttackStrength = 15f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        baseAttackStrength = 33;
        ExperienceGainLower = 8;
        ExperienceGainUpper = 14;
        ChooseNewTargetPos();
        InitialiseBehaviourTree();
        HPText.text = "Boss:\t\n" + (int)HP;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("--------Boss Inner State----------");
        
        //if (currentNode != null)
        //    currentNode.DoAction();
        //else
        RootNode.Execute();
    }

    //The initialisation of the boss' behaviour tree. 
    protected void InitialiseBehaviourTree()
    {
        MoveToPlayerNode moveToPlayerNode = new MoveToPlayerNode(Player, this);
        MeleeAttackNode meleeAttackNode = new MeleeAttackNode(Player, this);
        SequenceNode meleeSequence = new SequenceNode(new BehaviourNode[2] { moveToPlayerNode, meleeAttackNode });

        AOEAttackNode aoeAttackNode = new AOEAttackNode(Player, this);
        PlayerInRangeDecoratorNode playerInRangeDecoratorNode = new PlayerInRangeDecoratorNode(aoeAttackNode, this);
        SelectorNode attackTypeSelector = new SelectorNode(new BehaviourNode[2] { meleeSequence, playerInRangeDecoratorNode });

        WanderNode wanderNode = new WanderNode(Player, this);
        RootNode = new SelectorNode(new BehaviourNode[2] { attackTypeSelector, wanderNode });

        //currentNode = null;
    }

    //MoveTowardPlayer() is called by one of the nodes on the behaviour tree.
    public void MoveTowardPlayer()
    {
        Debug.Log("Moving Towards Player");

        // gets the path to the player.
        Vector3[] pathToPlayer = nodeMap.GetPathToTarget(transform.position, Player.transform.position, lineID);

        //if only one node to the player, just head straight for the player.
        if (pathToPlayer.Length == 1)
        {
            targetPosition = Player.transform.position;
        }
        else
        {
            targetPosition = pathToPlayer[1];
        }
        MoveTowardsTarget(moveSpeed);
    }

    //Does an AOE attack on the player, these can not be dodged, incentivising the healing zones.
    public void DoAOEAttack()
    {
        Debug.Log("Doing AOE attack");
        Instantiate(AoePrefab, Player.transform.position, Quaternion.identity);
        Player.GetComponent<Player>().TakeDamage(aoeAttackStrength);
        //currentNode = null;
    }

    //performs a basic attack on the player.
    public void DoBasicAttack()
    {
        Debug.Log("Doing Basic Attack");
        Player.GetComponent<Player>().TakeDamage(baseAttackStrength);
    }

    //detects incoming damage from the player.
    private void OnTriggerStay(Collider coll)
    {
        DetectDamageFromPlayer(coll);
        HPText.text = "Boss:\t\n" + (int)HP;
    }

    //the method that handles the boss' death. Transfers the player to a victory scene.
    protected override void DoDie()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("3. Victory");
    }
}
