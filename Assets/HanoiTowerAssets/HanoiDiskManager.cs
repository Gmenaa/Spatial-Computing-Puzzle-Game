using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanoiDiskManager : MonoBehaviour
{
    public DiskPlacementManager startingRod;
    public Transform[] disks;
    public float initializationDelay = 0.3f;

    private void Start()
    {
        StartCoroutine(InitializeDisks());
    }

    private IEnumerator InitializeDisks()
    {
        yield return new WaitForSeconds(initializationDelay);

        for (int i = 0; i < disks.Length; i++)
        {
            if (i < startingRod.snapPositions.Length)
            {
                Transform disk = disks[i];
                Rigidbody rb = disk.GetComponent<Rigidbody>();
                DiskSnap diskSnap = disk.GetComponent<DiskSnap>();
                DiskStack stack = startingRod.GetComponent<DiskStack>();

                // Snap to the correct starting position
                // disk.position = startingRod.snapPositions[i].position;
                // disk.rotation = Quaternion.identity;
                
                rb.isKinematic = true;
                stack.AddDisk(disk);
                diskSnap.MarkAsSnapped(stack, i == disks.Length - 1);
            }
        }
    }
}
