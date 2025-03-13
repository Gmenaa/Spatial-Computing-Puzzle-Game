using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskStack : MonoBehaviour
{
    private Stack<Transform> diskStack = new Stack<Transform>();
    public int totalDisks;
    
    public bool CanPlaceDisk(Transform newDisk)
    {
        if (diskStack.Count == 0) return true;
        Transform topDisk = diskStack.Peek();
        
        Debug.Log($"topDisk size {topDisk.localScale.x}");
        Debug.Log($"newDisk size {newDisk.localScale.x}");
        
        
        return newDisk.localScale.x < topDisk.localScale.x;
    }
    
    public void AddDisk(Transform newDisk)
    {
        diskStack.Push(newDisk);
        CheckWinCondition();
    }
    
    public bool HasDisk(Transform disk)
    {
        return diskStack.Contains(disk);
    }

    
    public bool IsTopDisk(Transform disk)
    {
        return diskStack.Count > 0 && diskStack.Peek() == disk;
    }
    
    public Transform RemoveDisk()
    {
        if (diskStack.Count > 0)
            return diskStack.Pop();
        return null;
    }
    
    private bool IsStackOrdered()
    {
        Transform[] diskArray = diskStack.ToArray();
        for (int i = 0; i < diskArray.Length - 1; i++)
        {
            if (diskArray[i].localScale.x < diskArray[i + 1].localScale.x)
            {
                return false;
            }
        }
        return true;
    }
    
    private void CheckWinCondition()
    {
        Debug.Log($"Count {diskStack.Count}");
        Debug.Log($"totalDisks {totalDisks}");
        
        if (diskStack.Count == totalDisks && IsStackOrdered())
        {
            Debug.Log($"<color=green>WIN CONDITION MET! Rod {gameObject.name} has all {totalDisks} disks.</color>");
        }
    }
}
