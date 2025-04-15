using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class attackBotSpawner : MonoBehaviour
{
    public GameObject bot;
    public float minEdgeDistance = 3f;  // min distance from camera
    public Transform playerCamera; 

    // angle ranges for two independent spawn angles  (bots spwan in a v shape from one common point)
    public float minAngle1 = -60f;
    public float maxAngle1 = -60f;
    public float minAngle2 = -115f;
    public float maxAngle2 = -115f;

    // bools to control whether angles are still active for spawning
    private bool angle1Active = true;
    private bool angle2Active = true;
    private bool spawn1Stopped = false;
    private bool spawn2Stopped = false;

    private float timer1 = 1f;
    private float timer2 = 0f;
    public float spawnTimer1 = 5f;
    public float spawnTimer2 = 7f;
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 10f;

    public GameObject teleportHotspot;
    public AudioClip teleclip;
    public AudioSource telesource;


    void Start()
    {
        if (teleportHotspot != null)
        {
            teleportHotspot.SetActive(false);
        }

        // If the camera isn't assigned, try to find it
        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }
            ResetTimers();
    }

    void Update()
    {
        timer1 += Time.deltaTime;
        timer2 += Time.deltaTime;

        // spawn bots based on first angle
        if (timer1 > spawnTimer1 && angle1Active)
        {
            SpawnGhost(minAngle1, maxAngle1);
            timer1 = 0f;  // reset angle 1 timer
            spawnTimer1 = Random.Range(minSpawnTime, maxSpawnTime);
        }
        else
        {
            timer1 += Time.deltaTime;
        }

        // spawn bots based on second angle
        if (timer2 > spawnTimer2 && angle2Active)
        {
            SpawnGhost(minAngle2, maxAngle2);
            timer2 = 0f;  // reset angle 2 timer
            spawnTimer2 = Random.Range(minSpawnTime, maxSpawnTime);
        }
        else
        {
            timer2 += Time.deltaTime;
        }
    }

    void ResetTimers()
    {
        spawnTimer1 = Random.Range(minSpawnTime, maxSpawnTime);
        spawnTimer2 = Random.Range(minSpawnTime, maxSpawnTime);
    }
    
    public void SpawnGhost(float minAngle, float maxAngle)
    {
        // generate random angle in specified range
        float randomAngle = Random.Range(minAngle, maxAngle);

        // determine spawn direction in a 2D plane
        Vector3 spawnDirection = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 0, Mathf.Sin(randomAngle * Mathf.Deg2Rad));

        // calculate spawn position at minEdgeDistance from camera
        Vector3 spawnPosition = playerCamera.position + spawnDirection.normalized * minEdgeDistance;

        // raycast downward to find ground level
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity))
        {
            spawnPosition.y = hit.point.y;
        }

        // instantiate prefab at calculated position
        Instantiate(bot, spawnPosition, Quaternion.identity);
    }

    // stop spawning from angle 1
    public void StopSpawningFromAngle1()
    {
        Debug.Log("inactive1");
        angle1Active = false;
        spawn1Stopped = true;
        CheckForTeleportActivation();
    }

    // stop spawning from angle 2
    public void StopSpawningFromAngle2()
    {
        Debug.Log("Stopping angle 2");
        angle2Active = false;
        spawn2Stopped = true;
        CheckForTeleportActivation();
    }

    // show teleport hotspot after destroying both bot spawners
    private void CheckForTeleportActivation()
    {
        if (spawn1Stopped && spawn2Stopped)
        {
            Debug.Log("Both spawn points stopped. Enabling teleport hotspot.");
            teleportHotspot.SetActive(true);

            if (teleclip != null)
            {
                Invoke(nameof(PlayTeleportSound), 1f);
            }
        }
    }

    // plays upon teleport hotspot revelation
    void PlayTeleportSound()
    {
        AudioSource.PlayClipAtPoint(teleclip, teleportHotspot.transform.position);
    }
}

