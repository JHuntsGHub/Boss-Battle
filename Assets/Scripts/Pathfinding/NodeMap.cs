using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NodeMap is used to map the nodes together and contains the pathfinding algorithms.
// NodeMap also is used to display some debug info such as drawing paths of the entities that use it.
public class NodeMap : MonoBehaviour
{
    //All the nodes in the scene.
    public Node[] nodes;

    //An array that is used to draw the paths that entities are using.
    public LineRenderer[] lineRenderers;

    //A bool that is used to determine if the debug info should be shown.
    private bool isDubugModeOn;

    //the element in the HUD canvas used for debug info.
    public GameObject canvasDebugInfo;

    // Start is called before the first frame update. This is where the mouse is locked to the screen and set to be invisible.
    void Start()
    {
        isDubugModeOn = true; //Starts as true that way toggleDebugVisibility() can be called after to hide the nodes themselves.
        ToggleDebugVisibility();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        //F3 is used to toggle the debug object's visibility.
        if (Input.GetKeyUp(KeyCode.F3))
        {
            ToggleDebugVisibility();
        }
        //F2 is a cheat key that reloads the boss battle scene.
        if (Input.GetKeyDown(KeyCode.F2))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("2. BossBattle");
        }
        //F9 is a cheat key that brings the user back to the main menu.
        if (Input.GetKeyDown(KeyCode.F9))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("1. MainMenu");
        }
    }

    //ToggleDebugVisibility shows/hides the nodes and toggles the isDebugModeOn bool.
    private void ToggleDebugVisibility()
    {
        isDubugModeOn = !isDubugModeOn;

        canvasDebugInfo.SetActive(isDubugModeOn);

        foreach (Node n in nodes)
            n.GetComponent<SpriteRenderer>().enabled = isDubugModeOn;
    }

    // GetClosestNode takes in a Vector 3 and returns the Node that is closest.
    private Node GetClosestNode(Vector3 position)
    {
        Node currentClosest = nodes[0];

        for(int i = 1; i < nodes.Length; i++)
        {
            if (Vector3.Distance(position, nodes[i].transform.position) < Vector3.Distance(position, currentClosest.transform.position))
                currentClosest = nodes[i];
        }
        return currentClosest;
    }

    /*
     * GetPathToTarget is the main pathfinding algorithm for this game.
     * It takes a start position and target position in and returns an array of points along a path to the target.
     * It also takes ihn a lineID integer that is used to to draw the line in that entities colour.
     */
    public Vector3[] GetPathToTarget(Vector3 startPosition, Vector3 targetPosition, int lineID)
    {
        // Begins by getting the node closest to the current and target positions.
        Node current = GetClosestNode(startPosition);
        Node target = GetClosestNode(targetPosition);

        // Creates a queue of nodes that will make up the path. The closest node is added straight away.
        Queue<Node> nodePath = new Queue<Node>();
        nodePath.Enqueue(current);

        // A while loop keeps looking for best path to the target.
        // Since the node network is closed, this will never be infinite.
        while(current != target)
        {
            int indexOfClosest = 0;
            float distanceOfClosest = Vector3.Distance(current.Neighbours[0].transform.position, target.transform.position);

            // Searches through the node's neighbors for the one closest to the target
            for(int i = 1; i < current.Neighbours.Length; i++)
            {
                float distance = Vector3.Distance(current.Neighbours[i].transform.position, target.transform.position);
                if(distance < distanceOfClosest)
                {
                    distanceOfClosest = distance;
                    indexOfClosest = i;
                }
            }

            // Adds the closest node to the target to the queue which will then have it's neighbors searched.
            current = current.Neighbours[indexOfClosest];
            nodePath.Enqueue(current);
        }

        ///Debug.Log("--------------------------------");
        ///foreach (Node n in nodePath)
        ///    Debug.Log(n.name);
        ///Debug.Log("--------------------------------");
        ///Debug.Log(nodePath.Count);

        // Finds out the real length of the queue. The way C# extends queues means that there willmost likely be some amount of null entries towards the end.
        int actualLength = 0;
        foreach(Node n in nodePath)
        {
            if (n != null)
                actualLength++;
        }

        // Converts the queue into a vector array. A queue was used initially as the number of nodes in the path was not known.
        Vector3[] pathInVector3 = new Vector3[nodePath.Count];
        for (int i = 0; i < actualLength; i++)
        {
            pathInVector3[i] = nodePath.Dequeue().transform.position;
        }
        
        // uses the path to draw the line if the debug mode is on.
        DrawLine(startPosition, targetPosition, pathInVector3, lineID);

        //returns the vector array of the path.
        return pathInVector3;
    }

    // GetRandomNeighborNodePos() takes in a vector and returns a nearby node's position.
    // Also takes in a lineID that is used to draw the debug line.
    public Vector3 GetRandomNeighborNodePos(Vector3 currentPosition, int lineID)
    {
        //gets the node closest to the current position.
        Node currentNode = GetClosestNode(currentPosition);

        //picks a random neighbour of that node.
        int numOfNeighbours = currentNode.Neighbours.Length;
        currentNode = currentNode.Neighbours[Random.Range(0, numOfNeighbours - 1)];

        DrawSmallLine(currentPosition, currentNode.transform.position, lineID);

        //returns a vector of the target node.
        return currentNode.transform.position;
    }

    // DrawLine is used to draw a debug line showing a path from a given start position to a given end position along a given array
    // of node positions using the lineID to determine which line renderer will be used.
    public void DrawLine(Vector3 startPosition, Vector3 endPosition, Vector3[] nodePositions, int lineID)
    {
        if (isDubugModeOn)
        {
            lineRenderers[lineID].enabled = true;
            lineRenderers[lineID].startWidth = 0.5f;
            lineRenderers[lineID].endWidth = 0.5f;
            
            Vector3[] totalPositions = new Vector3[nodePositions.Length + 2];
            totalPositions[0] = startPosition;

            for (int i = 0; i < nodePositions.Length; i++)
                totalPositions[i + 1] = nodePositions[i];
            totalPositions[totalPositions.Length - 1] = endPosition;

            ///Debug.Log("_-_-_-_-_-_-_-_-_-");
            ///foreach(Vector3 v in totalPositions)
            ///    Debug.Log("x:" + v.x + ", y:" + v.y + "z:" + v.z);
            ///Debug.Log("_-_-_-_-_-_-_-_-_-");

            lineRenderers[lineID].positionCount = totalPositions.Length;
            lineRenderers[lineID].SetPositions(totalPositions);
        }
        else
            lineRenderers[lineID].enabled = false;
    }

    //draws a small line between two points
    public void DrawSmallLine(Vector3 currentPosition, Vector3 targetPosition, int lineID)
    {
        //draws a line between the current position and the target node.
        if (isDubugModeOn)
        {
            lineRenderers[lineID].enabled = true;
            lineRenderers[lineID].startWidth = 0.5f;
            lineRenderers[lineID].endWidth = 0.5f;

            lineRenderers[lineID].positionCount = 2;
            lineRenderers[lineID].SetPositions(new Vector3[2] { new Vector3(currentPosition.x, targetPosition.y, currentPosition.z), targetPosition });
        }
        else
            lineRenderers[lineID].enabled = false;
    }
}
