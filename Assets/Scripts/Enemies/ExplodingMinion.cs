using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplodingMinion : Enemy
{
    //The extra movement speed while charging.
    public float chargeMoveBoost = 2.5f;

    //The audio source the charging sound will come from presceding the explosion.
    public AudioSource audioSource;

    //The prefab that contains the explosion particles and sound
    public GameObject explosionPrefab;

    //The debug text on the HUF showing state.
    public Text debugText;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentState = State.Wander; //Starting state set to wander.
        debugText.text = "Exploder State: Wander";
        baseAttackStrength = 34f;
        ExperienceGainLower = 2;
        ExperienceGainUpper = 7;
        ChooseNewTargetPos();
    }

    // The Exploding Minion's behaviour is controlled through a finite state machine here in Update();
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
            case State.Explode:
                DoExplode();
                break;
        }
    }

    //in the wander state, the minion, moves randomly from node to node.
    // - if the player gets close enough, the charge state is triggered.
    public override void DoWander()
    {
        base.DoWander();

        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(Player.transform.position.x, 0, Player.transform.position.z)) < 25.0f)
        {
            animator.SetBool("isCharging", true);
            currentState = State.Charge;
            debugText.text = "Exploder State: Charge";
            animator.SetBool("isRunning", true);
        }
    }

    // The charge state tries to rush towards the player.
    // - if the minion gets close enough, the explosion state is triggered.
    // - if the player gets far enough away, the wander state is triggered.
    private void DoCharge()
    {
        // gets the path to the player.
        Vector3[] pathToPlayer = nodeMap.GetPathToTarget(transform.position, Player.transform.position, lineID);

        //if the minion is close enough to the player, trigger the explode state.
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(Player.transform.position.x, 0, Player.transform.position.z)) <= 4f)
        {
            currentState = State.Explode;
            debugText.text = "Exploder State: Explode";
            animator.SetBool("isExploding", true);
            audioSource.Play();
            return;
        }
        // if the number of points on the path to the player is 1, then head straight for the player.
        else if (pathToPlayer.Length == 1)
        {
            targetPosition = Player.transform.position;
        }
        // if there are more than 5 nodes between this minion and the player, revert to wandering.
        else if (pathToPlayer.Length > 5)
        {
            ChooseNewTargetPos();
            currentState = State.Wander;
            debugText.text = "Exploder State: Wander";
            animator.SetBool("isRunning", false);
            return;
        }
        else
        {
            targetPosition = pathToPlayer[1];
        }
        MoveTowardsTarget(moveSpeed + chargeMoveBoost);
    }

    // Does the explosion state. This state, once begun, results in the death of the minion.
    // The player can still kill the minion before it explodes for exp but is better off running away.
    private void DoExplode()
    {
        //Checks to see if the charging sound effect is over.
        if (!audioSource.isPlaying)
        {
            // spawns the prefab containing the explosion sound and particles.
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // checks if the player is close enough, if so, does damage.
            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(Player.transform.position.x, 0, Player.transform.position.z)) <= 13f)
                Player.GetComponent<Player>().TakeDamage(baseAttackStrength);

            // The minion is destroyed.
            Destroy(gameObject);
        }
    }

    //Detects damage resulting from collisions with the player's attacks.
    private void OnTriggerStay(Collider coll)
    {
        DetectDamageFromPlayer(coll);
    }

    protected override void DoDie()
    {
        debugText.text = "Exploder State: Dead";
        base.DoDie();
    }
}
