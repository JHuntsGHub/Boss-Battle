using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HealingPad handles collisions with the player determining when hp/mana should be healed and when the audio for healing should be played.
public class HealingPad : MonoBehaviour
{
    // The game object of the player object.
    public GameObject player;
    
    // Detects when the player comes into contact with the healing zone. Plays audio if so.
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
            coll.gameObject.GetComponent<Player>().PlayHealingAudio();
    }

    // Detects if the player is in the healing zone, if so, commences healing.
    private void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
            player.GetComponent<Player>().HealHealthAndMana();
    }

    // Detects if the player leaves the healing zone and stops the audio.
    private void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
            player.GetComponent<Player>().StopHealingAudio();
    }
}
