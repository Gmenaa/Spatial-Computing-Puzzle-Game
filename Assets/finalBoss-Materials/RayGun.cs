using System;
using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.Attributes;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class RayGun : MonoBehaviour
{

    public OVRInput.RawButton shootingButton;

    public LineRenderer linePrefab;
    public Transform shootingpoint;

    public float maxLineDistance = 5;
    public float lineShowTimer = 0.3f;

    public AudioSource source;
    public AudioClip shootingAudioClip;
    

    void Start()
    {
        
    }

    void Update()
    {
        if (OVRInput.GetDown(shootingButton)){
            shoot();
        }
    }

    public void shoot()
    {
        source.PlayOneShot(shootingAudioClip);

        Ray ray = new Ray(shootingpoint.position, shootingpoint.forward);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit, maxLineDistance);

        Vector3 endPoint = Vector3.zero;

        if (hasHit) {
            endPoint = hit.point;

            attackBots bots = hit.transform.GetComponentInParent<attackBots>();
            CubeTarget cubeTarget = hit.transform.GetComponent<CubeTarget>();

            //if its a bot thats hit, kill
            if (bots){
                bots.Kill();
            }
            // else, if its a shooter thats hit, count as one hit
            else if (cubeTarget)
            {
                cubeTarget.OnHit(); // Call the OnHit method on the cube to track hits
                Debug.Log("boom");
            }
            else{

            }

        }
        else {
            endPoint = shootingpoint.position + shootingpoint.forward * maxLineDistance;
        }
        
        Debug.Log("PEW!");
        
        LineRenderer line = Instantiate(linePrefab);

        line.positionCount = 2;
        line.SetPosition(0,shootingpoint.position);
        Vector3 endpoint = shootingpoint.position  + shootingpoint.forward * maxLineDistance;

        line.SetPosition(1, endpoint);

        Destroy(line.gameObject, lineShowTimer);

    }
}
