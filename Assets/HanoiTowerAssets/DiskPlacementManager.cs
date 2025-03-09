using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiskPlacementManager : MonoBehaviour
{
    public Transform[] snapPositions;
    private int currentIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger detected: {other.name} entered {gameObject.name}");
        
        if (other.CompareTag("Disk") && currentIndex < snapPositions.Length)
        {
            // OVRGrabbable grabbable = other.GetComponent<OVRGrabbable>(); // will enable for prod for now CanPlaceDisk is enough
            Rigidbody diskRb = other.GetComponent<Rigidbody>();
            DiskStack stack = GetComponent<DiskStack>();
            DiskSnap diskSnap = other.GetComponent<DiskSnap>();
            
            if (stack.HasDisk(other.transform))
            {
                Debug.Log($"Disk {other.name} is already placed on {gameObject.name}, ignoring.");
                return;
            } 

            // Only snap when disk is released and it's a valid move this will be implemented for final for now CanPlaceDisk is enough
            // if (!grabbable.isGrabbed && stack.CanPlaceDisk(other.transform)) // will enable for prod
            if (stack.CanPlaceDisk(other.transform))
            {
                Debug.Log($"Placing disk: {other.name} on rod {gameObject.name} at position {snapPositions[currentIndex].position}");
                
                diskRb.isKinematic = true;
                other.transform.position = snapPositions[currentIndex].position;
                stack.AddDisk(other.transform);
                
                bool isTop = stack.IsTopDisk(other.transform);
                diskSnap.MarkAsSnapped(stack, isTop);
                currentIndex++;
            }
            else
            {
                Debug.Log("Cannot place a larger disk on a smaller one!");
            }
        }
    }
}
