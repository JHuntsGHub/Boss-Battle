using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherMinion : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentState = State.Idle;
        baseAttackStrength = 15;
        ExperienceGainLower = 2;
        ExperienceGainUpper = 7;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                DoIdle();
                break;
            case State.Attack:
                DoAttack();
                break;
            case State.Flee:
                DoFlee();
                break;
        }
    }

    private void DoIdle()
    {

    }

    private void DoAttack()
    {

    }

    private void DoFlee()
    {
        
    }

}
