using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class attackBots : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public float speed = 1;
    public PlayerHealth playerHealth;  // Reference to PlayerHealth script

    void Start()
    {
        
    }

    void Update()
    {
        // find player camera 

        if(!agent.enabled)
        return;

        Vector3 targetPosition = Camera.main.transform.position;

        agent.SetDestination(targetPosition);
        agent.speed = speed;
        
    }

    public void Kill()
    {
        // disable bots and trigger death animation
        agent.enabled = false;
        animator.SetTrigger("death");
        Destroy(gameObject, .2f);
    }

}
