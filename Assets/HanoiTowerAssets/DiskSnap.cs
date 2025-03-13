using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskSnap : MonoBehaviour
{
    private OVRGrabbable grabbable;
    private Rigidbody rb;
    private DiskStack parentStack;
    private bool isTopDisk = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<OVRGrabbable>();
    }

    private void Update()
    {
        if (grabbable.isGrabbed && isTopDisk)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            if (parentStack != null)
            {
                parentStack.RemoveDisk();
                Debug.Log($"Disk {gameObject.name} removed from stack {parentStack.gameObject.name}");
                parentStack = null;
            }
        }
        else if (grabbable.isGrabbed && !isTopDisk)
        {
            Debug.Log($"Cannot grab {gameObject.name} because it is not the topmost disk!");
        }
    }

    public void MarkAsSnapped(DiskStack stack, bool isTop)
    {
        parentStack = stack;
        isTopDisk = isTop;
    }
}