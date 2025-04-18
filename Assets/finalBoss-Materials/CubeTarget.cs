using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTarget : MonoBehaviour
{
    private int hitCount = 0; // tracks number of hits thus far
    public int requiredHits = 3; // number of hits needed to trigger explosion of shooter
    public Animator animator;
    public attackBotSpawner botSpawner;

    public AudioSource audioSource;
    public AudioClip explosionSound; // Explosion sound effect



   void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // find the attackBotSpawner
        if (botSpawner == null)
        {
            botSpawner = FindObjectOfType<attackBotSpawner>();
        }

        if (botSpawner == null)
        {
            Debug.LogWarning("No attackBotSpawner!");
        }
    }


    public void OnHit()
    {
        Debug.Log("hit!!");
        hitCount++;

        if (hitCount == requiredHits)
        {
            TriggerExplosion();
        }
    }

    void TriggerExplosion()
    {
        // play explosion sound
        audioSource.PlayOneShot(explosionSound);

        // trigger animated disintigration scene
        animator.SetTrigger("shooter_death");
        animator.SetTrigger("shooter_death2");

        // Change the cube's shape (make it misshapen)
        transform.localScale = new Vector3(1.5f, 0.5f, 1f); 

        // disable  collider to stop further interaction
        GetComponent<Collider>().enabled = false;
    }

    void stopspawn1(){
        botSpawner.StopSpawningFromAngle1();
    }
    void stopspawn2(){
        botSpawner.StopSpawningFromAngle2();
    }

}
