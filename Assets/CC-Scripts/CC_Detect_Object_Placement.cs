using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Detect_Object_Placement : MonoBehaviour
{
     public CC_Object_Placement_Manager_Script placementManager; // Use the correct class name

    [Header("Beaker Placement SFX")]
    public AudioSource sfxAudioSource;       // drag in your SFX AudioSource (can be the same one you use elsewhere)
    public AudioClip beakerPlaceClip;        

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("redBeaker") || other.CompareTag("yellowBeaker") ||
            other.CompareTag("blueBeaker") || other.CompareTag("greenBeaker") ||
            other.CompareTag("orangeBeaker"))
        {
        
        // play the beaker-placement sound 
        if (sfxAudioSource != null && beakerPlaceClip != null)
            sfxAudioSource.PlayOneShot(beakerPlaceClip);
           
            // When an object enters the vessel, inform the manager 
            placementManager.OnObjectPlaced(other.gameObject);
        }
    }
}


