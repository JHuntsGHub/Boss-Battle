using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls an audio game object that is spawned when a NPC dies.
public class DeathAudio : MonoBehaviour
{
    //The Audio source that the death sound will play from.
    public AudioSource audio;

    // Update is called once per frame
    void Update()
    {
        //A simple check to see when the audio has stopped so that the game object will be destroyed.
        if (!audio.isPlaying)
            Destroy(gameObject);
    }
}
