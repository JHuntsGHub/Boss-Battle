using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The selector node will iterate through all of it's childeren. If one succeeds, the Selecter itself slucceeds.
public class SelectorNode : BehaviourNode
{
    BehaviourNode[] childNodes;

    public SelectorNode(BehaviourNode[] childNodes)
    {
        this.childNodes = childNodes;
    }

    
    public override BehaviourState Execute()
    {
        foreach(BehaviourNode node in childNodes)
        {
            if (node.Execute() == BehaviourState.Succeed)
                return BehaviourState.Succeed;
        }

        return BehaviourState.Fail;
    }
}
