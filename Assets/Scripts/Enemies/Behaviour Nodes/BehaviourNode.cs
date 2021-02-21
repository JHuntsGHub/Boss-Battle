using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the BehaviourNode all the other behaviours inherit from. It contains an enum and two empty methods, ready for overriding.
public class BehaviourNode
{
    //I used an enum to determine success or failure. I could have used a bool but this way meant the code was more easily read.
    public enum BehaviourState { Succeed, Fail }


    public virtual BehaviourState Execute()
    {
        return BehaviourState.Succeed;
    }

    public virtual void DoAction()
    {

    }

}
