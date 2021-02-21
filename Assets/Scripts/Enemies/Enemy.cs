using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Enemy is the base class for all the enemies in the game and it inherits Entity.
public class Enemy : Entity
{
    // An enum for the different possible states.
    public enum State { Idle, Wander, Charge, Flee, Attack, Explode, ReadyCharge };
    
    // The current state is saved here using the above enum.
    protected State currentState;

    // The enemies current target position.
    protected Vector3 targetPosition;

    // The enemies make use of the Player object.
    public GameObject Player;

    // A reference to the NodeMap for pathfinding and some debug display.
    public NodeMap nodeMap;

    // Prefabs for the experience orb and a death audio game object. These are instansiated when the enemy dies.
    public GameObject ExperienceOrb;
    public GameObject DeathAudio;

    // The amount of experience orbs spawned when the enemy dies is random, these are the lower and upper limits.
    public int ExperienceGainLower, ExperienceGainUpper;

    // The Wandering state is done here in DoWander();
    // It simply picks a nearby node to wander to, when it gets there, it chooses another.
    // This is a protected virtual method as it is extended in specific enemies use of it.
    public virtual void DoWander()
    {
        //checks if close to the desired node. If so, chooses another.
        if (Vector3.Distance(new Vector3(targetPosition.x, 0, targetPosition.z), new Vector3(transform.position.x, 0, transform.position.z)) <= 0.1f)
        {
            ChooseNewTargetPos();
        }

        MoveTowardsTarget(moveSpeed);
    }

    //MoveTowardsTarget() changes position and rotation to get closer to the target.
    protected void MoveTowardsTarget(float speed)
    {
        transform.position = new Vector3(transform.position.x, targetPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.LookAt(targetPosition);
        FixYPosition();
    }

    //Uses the NodeMap to choose a nearby node.
    protected void ChooseNewTargetPos()
    {
        targetPosition = nodeMap.GetRandomNeighborNodePos(transform.position, lineID);
    }

    //DoDie is called when an enemy dies, it spawns experience orbs and a death audio object.
    protected virtual void DoDie()
    {
        for (int i = 0; i < Random.Range(ExperienceGainLower, ExperienceGainUpper); i++)
            Instantiate(ExperienceOrb, new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z), Quaternion.identity);

        Instantiate(DeathAudio);

        //The enemy is destroyed.
        Destroy(gameObject);
    }

    //Calculated damage done by the enemy and if below 0, calls DoDie().
    protected void DetectDamageFromPlayer(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            HP -= coll.gameObject.GetComponent<Player>().GetDamageAmount();

            ///Debug.Log("HP = " + HP);

            if (HP <= 0)
                DoDie();
        }
    }
}
