using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Node is a simple class that is used by NodeMap.cs to map out the traversable area for NPCs.
// Node is just used to keep track of it's neighbours.
public class Node : MonoBehaviour
{
    // Neighbours is an array of the other nodes closest to this one. It is initialised within Unity.
    public Node[] Neighbours;

    // OnDrawGizmos is only used in the editor to more easily see the paths between the nodes.
    private void OnDrawGizmos()
    {
        foreach(Node n in Neighbours)
        {
            Gizmos.DrawLine(transform.position, n.transform.position);
        }
    }
}
