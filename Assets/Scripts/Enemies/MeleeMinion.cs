using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//MeleeMinion is an enemy that wanders the map and occasionally tries to charge at the player.
public class MeleeMinion : Enemy
{
    //The extra movement speed while charging.
    public float chargeMoveBoost = 8.0f;

    //The debug text on the HUF showing state.
    public Text debugText;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentState = State.Wander; //sets the starting state to wander.
        debugText.text = "Melee State: Wander";
        baseAttackStrength = 24;
        ExperienceGainLower = 2;
        ExperienceGainUpper = 7;
        ChooseNewTargetPos();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Wander:
                DoWander();
                break;
            case State.Charge:
                DoCharge();
                break;
            case State.ReadyCharge:
                DoReadyCharge();
                break;
        }
    }
    
    //Inherits the wander state from enemy and then checks if the player is near.
    public override void DoWander()
    {
        base.DoWander();

        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(Player.transform.position.x, 0, Player.transform.position.z)) < 30.0f)
        {
            animator.SetBool("isCharging", true);
            debugText.text = "Melee State: Charge";
            currentState = State.Charge;
        }
    }

    private void DoCharge()
    {
        //charge code here
        //nodeMap.GetPathToTarget(transform.position, Player.transform.position, entityColour, lineID);
        //transform.position = Vector3.MoveTowards(transform.position, nodeMap.GetPathToTarget(transform.position, Player.transform.position, lineID)[1], moveSpeed * Time.deltaTime);
    }

    private void DoReadyCharge()
    {

    }

    //detects damage coming from collisions with the player.
    private void OnTriggerStay(Collider coll)
    {
        DetectDamageFromPlayer(coll);
    }
}
