using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

// This is the Player class, it inherits Entity.
public class Player : Entity
{
    //The amount of time between basic attacks.
    private const float ATTACK_COOLDOWN_LENGTH = 1.1f;

    //The base amount of experience needed to level up.
    private const int BASE_EXP_TO_LEVEL_UP = 100;

    //The damage multiplers for the aoe and basic attacks.
    private const int AOE_DAMAGE_MULTIPLIER = 19;
    private const int BASIC_DAMAGE_MULTIPLIER = 25;
    
    //The particles for the aoe attack.
    public ParticleSystem[] particles;

    //The various audio sources attatched to the player. Multiple are needed as more than one can happen at any given time.
    public AudioSource AttackAudio, FireAOEAudio, RunningAudio, HealingAudio;

    //The text objects on the HUD canvas for displaying the HP/mana/level/experience.
    public Text HPandManaText, LevelText;

    //The player's mana pool. 100 by default.
    private float mana = 100.0f;

    //The level that the player is currently at.
    private int playerLevel;

    //The current amount of time left on a basic attack cooldown.
    private float attackCooldown;

    //The amount of experience points needed to level up.
    private float expToLevelUp;

    //The strength of the AOE attack.
    private float AOEAttackStrength;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        lineID = 0;
        attackCooldown = 0;
        baseAttackStrength = 15;
        AOEAttackStrength = 7;
        playerLevel = 1;
        expToLevelUp = BASE_EXP_TO_LEVEL_UP;
        UpdateHUDText();
    }

    // Update is called once per frame
    void Update()
    {
        DoMovement();
        DoAttack();
    }

    //DoMovement() controls the movement of the player.
    private void DoMovement()
    {
        // Move the player forwards
        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.AddForce(transform.forward * moveSpeed);
        }

        //When the player starts moving forward, trigger running animation/sound.
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("isRunning", true);
            RunningAudio.Play();
        }
        //When the player stops moving forward, stops the running animation/sound.
        else if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("isRunning", false);
            RunningAudio.Stop();
        }

        // Rotate around our y-axis
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }

    //DoAttack controls how the player attacks the enemies.
    private void DoAttack()
    {
        //if the player has mana left, they can do an aoe attack that does damage over time.
        if(mana > 0)
        {
            //When the right mouse is pressed the aoe sound/particles are started.
            if (Input.GetMouseButtonDown(1))
            {
                FireAOEAudio.Play();
                EmmitParticles();
            }
            //When the right mouse is lifted the aoe sound/particles are stopped.
            else if (Input.GetMouseButtonUp(1))
            {
                FireAOEAudio.Stop();
                StopParticles();
            }
            //While the right mouse is down, drain mana and update the text to reflect that.    
            if (Input.GetMouseButton(1))
            {
                mana -= Time.deltaTime * 10;
                UpdateHUDText();
            }
        }
        else
        {
            //if no mana, make sure the aoe audio and particles are off.
            FireAOEAudio.Stop();
            StopParticles();
        }
        

        if (attackCooldown <= 0)
        {
            //When the left mouse button is down, do a punch animation and sound effect.
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("isAttacking", true);
                AttackAudio.Play();
                attackCooldown = ATTACK_COOLDOWN_LENGTH;
            }
        }
        else
            attackCooldown -= Time.deltaTime;
    }

    //Starts emiting the aoe attack's particles.
    private void EmmitParticles()
    {
        foreach (ParticleSystem p in particles)
            p.Play();
    }

    //Stops emiting the aoe attack's particles.
    private void StopParticles()
    {
        foreach (ParticleSystem p in particles)
            p.Stop();
    }

    //HealHealthAndMana() is called when in a healing zone and heals the player's health and mana.
    public void HealHealthAndMana()
    {
        HP += Time.deltaTime * 3;
        mana += Time.deltaTime * 3;

        if (HP > 100)
            HP = 100;
        if (mana > 100)
            mana = 100;
        if (HP == 100 && mana == 100)
            HealingAudio.Stop(); //if health and mana are full, make sure the sound effect has stopped.

        //Update the HUD to show the replenishment.
        UpdateHUDText();
    }

    //UpdateHUDText() updates a canvas to show the player's hp/mana/level/exp needed to level up.
    private void UpdateHUDText()
    {
        HPandManaText.text = "Health: " + (int)HP + "\n\tMana: " + (int)mana;
        LevelText.text = "Lvl " + playerLevel + "\nExp To Level Up " + expToLevelUp;
    }

    //GiveExperiencePoints() gives the experience points to the player when they come in contact with and ExpOrb.
    // If the amount of exp needed goes <= 0, then calculate the amount of exp needed to reach the next level.
    private void GiveExperiencePoints(int points)
    {
        expToLevelUp -= points;

        if(expToLevelUp <= 0)
        {
            playerLevel++;
            expToLevelUp += BASE_EXP_TO_LEVEL_UP + (playerLevel * 20);
        }
    }

    //GetDamageAmount() is called by various enemies. It tells them how much damage needs to be applied from their collisions with the player.
    public float GetDamageAmount()
    {
        if (Input.GetMouseButton(0))
        {
            return (baseAttackStrength + BASIC_DAMAGE_MULTIPLIER * playerLevel) * Time.deltaTime;
        }

        if (Input.GetMouseButton(1))
        {
            return (AOEAttackStrength + AOE_DAMAGE_MULTIPLIER * playerLevel) * Time.deltaTime;
        }

        return 0.0f;
    }

    //Plays the healing sound effect
    public void PlayHealingAudio() { HealingAudio.Play(); }

    //Stops the healing sound effect
    public void StopHealingAudio() { HealingAudio.Stop(); }

    //Gives the player damage. If the player's hp drops <= 0, the player dies.
    public void TakeDamage(float amount)
    {
        HP -= amount;
        UpdateHUDText();

        if(HP <= 0)
        {
            HP = 0;
            UpdateHUDText();
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("3. GameOver");
        }
    }

    //Detects when the player has come in contact with an ExpOrb.
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Exp")
        {
            Destroy(coll.gameObject);
            GiveExperiencePoints(20);
            UpdateHUDText();
        }
    }
}