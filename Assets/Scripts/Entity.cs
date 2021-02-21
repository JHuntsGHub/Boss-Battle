using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Entity is used as the base class for all enemies and the player.
// It contains crucial methods and variables that are used by all.
public class Entity : MonoBehaviour
{
    //The amount of health the entity has, 100 by default.
    public float HP = 100;

    //The move and rotation speeds for the entity, 5 and 180 by default.
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 180f;

    //The base strength for an attack, 10 by default.
    protected float baseAttackStrength = 10.0f;

    //The y offset from the floor. Different entities have their pivots in different positions relative to themselves. This variable is used so that code can be applied to them uniformly.
    protected float yOffsetFromFloor = 0.0f;

    //The lineID for this entity. Each enemy and the player have a unique ID that signals to the NodeMap which line renderer they use so they can have different colours.
    //This is just a q.o.l feature that makes it easier to see which is which.
    public int lineID;

    //The animator and rigidbody used for this entity. 
    public Animator animator;
    public Rigidbody rigidbody;

    // Start is called before the first frame update
    // Each inherited class uses what is in this start function by calling the base.Start() in each of their respective Start() functions.
    protected virtual void Start()
    {
        InitialiseYOffsetFromFloor();
    }

    //InitialiseYOffsetFromFloor() just initialises the yOffsetFromFloor variable.
    private void InitialiseYOffsetFromFloor()
    {
        yOffsetFromFloor = transform.position.y;
    }

    // FixYPosition just sets the entity at the appropriate y level.
    protected void FixYPosition()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(new Vector3(transform.position.x, 5, transform.position.z + 0.05f), -transform.up, 10.0f);

        for(int i = 0; i< hits.Length; i++)
        {
            if (hits[i].collider.gameObject.tag == "Floor")
            {
                transform.position = new Vector3(transform.position.x, hits[i].collider.transform.position.y + yOffsetFromFloor, transform.position.z);
                return;
            }
        }

        Debug.LogError("Floor Not Detected.");
    }
}
