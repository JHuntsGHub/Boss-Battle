using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sequence nodes execute each of thir childeren in order, if any one fails, this fails.
public class SequenceNode : BehaviourNode
{
    //The sequences' childeren.
    private BehaviourNode[] childNodes;

    //SequenceNode takes it's childeren in as a perameter.
    public SequenceNode(BehaviourNode[] childNodes)
    {
        this.childNodes = childNodes;
    }

    //The execute command simply iterates through it's childeren, executing them one at a time.
    public override BehaviourState Execute()
    {
        foreach (BehaviourNode node in childNodes)
        {
            if (node.Execute() == BehaviourState.Fail)
                return BehaviourState.Fail;
        }
        
        return BehaviourState.Succeed;
    }
}
