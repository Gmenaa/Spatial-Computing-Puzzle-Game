using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteConnectionChecker : MonoBehaviour
{
    public PipeConnection startPipe;
    public PipeConnection endPipe;
    public Color completeColor = Color.blue;

    public void CheckFullConnection()
    {
        if (IsPathComplete(startPipe, endPipe))
        {
            Debug.Log("Puzzle Solved! Pipes are fully connected.");
            PipeConnection[] allPipes = FindObjectsOfType<PipeConnection>();
            foreach (PipeConnection pipe in allPipes)
            {
                pipe.ChangeColor(completeColor);
            }
        }
    }

    private bool IsPathComplete(PipeConnection start, PipeConnection end)
    {
        HashSet<PipeConnection> visited = new HashSet<PipeConnection>();
        Queue<PipeConnection> queue = new Queue<PipeConnection>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            PipeConnection currentPipe = queue.Dequeue();
            if (currentPipe == end) return true;

            foreach (PipeConnection otherPipe in FindObjectsOfType<PipeConnection>())
            {
                if (!visited.Contains(otherPipe) && currentPipe.IsConnectedTo(otherPipe))
                {
                    visited.Add(otherPipe);
                    queue.Enqueue(otherPipe);
                }
            }
        }
        return false;
    }
}