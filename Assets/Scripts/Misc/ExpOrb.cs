using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    // The experience orb's rigidbody is used to add some force to it upon initialisation.
    public Rigidbody rigidbody;

    // The collider is not enabled by default. It is then enabled after some time so the player has a chance to see the orb.
    public Collider collider;

    // The amount of time before the player can collide with the orb.
    private float colliderActivateTimer = 1.5f;

    // Start is called before the first frame update
    private void Start()
    {
        // Adds a small upwards force to the orb so they appear to explode out of a dying enemy.
        rigidbody.AddForce(Random.Range(20.0f, 60.0f), Random.Range(450.0f, 750.0f), Random.Range(20.0f, 60.0f));
    }

    //The update method simply checks if the collider is ready to be enabled.
    private void Update()
    {
        colliderActivateTimer -= Time.deltaTime;

        if (colliderActivateTimer <= 0)
            collider.enabled = true;
    }
}
