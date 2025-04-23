using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteConnectionChecker : MonoBehaviour
{
    public PipeConnection startPipe;
    public PipeConnection endPipe;

    public void CheckFullConnection()
    {
        List<PipeConnection> connectedPath = GetConnectedPath(startPipe, endPipe);
        PipeConnection[] allPipes = FindObjectsOfType<PipeConnection>();

        if (connectedPath != null)
        {
            foreach (PipeConnection pipe in connectedPath)
            {
                    pipe.SetComplete();
            }

            GameLoop gameLoop = FindObjectOfType<GameLoop>();

            if (gameLoop != null)
            {
                // Trigger the win condition in GameLoop
                gameLoop.TriggerWin();
            }
            else
            {
                Debug.LogError("GameLoop not found in scene!");
            }
        }
        else //if connectedPath returned null, win condition hasn't been met; set white glow to pipes that connect to the start
        {
            HighlightConnectedPipes(startPipe, allPipes);
        }
    }

    private List<PipeConnection> GetConnectedPath(PipeConnection start, PipeConnection end)
        /*This function returns a list of all pipes in the full connection from the starting pipe to the ending pipe, if such a chain of pipes exists*/
    {
        Dictionary<PipeConnection, PipeConnection> cameFrom = new Dictionary<PipeConnection, PipeConnection>();
        Queue<PipeConnection> queue = new Queue<PipeConnection>();
        HashSet<PipeConnection> visited = new HashSet<PipeConnection>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            PipeConnection current = queue.Dequeue();

            if (current == end)
            {
                List<PipeConnection> path = new List<PipeConnection>();
                PipeConnection node = end;
                while (node != null)
                {
                    path.Add(node);
                    cameFrom.TryGetValue(node, out node);
                }
                path.Reverse();
                return path;
            }

            foreach (PipeConnection neighbor in FindObjectsOfType<PipeConnection>())
            {
                if (!visited.Contains(neighbor) && current.IsConnectedTo(neighbor))
                {
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return null;
    }

    public void HighlightConnectedPipes(PipeConnection start, PipeConnection[] allPipes)
    {
        /*Highlights pipes (white glow) that are connected to the start but don't form a full connection*/
        HashSet<PipeConnection> reachable = new HashSet<PipeConnection>();
        Queue<PipeConnection> queue = new Queue<PipeConnection>();

        queue.Enqueue(start);
        reachable.Add(start);

        while (queue.Count > 0)
        {
            PipeConnection current = queue.Dequeue();

            foreach (PipeConnection neighbor in allPipes)
            {
                if (!reachable.Contains(neighbor) && current.IsConnectedTo(neighbor))
                {
                    reachable.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        foreach (PipeConnection pipe in allPipes)
        {
            if (pipe == startPipe || pipe == endPipe)
            {
                pipe.SetGlow(false);
            }
            else if (reachable.Contains(pipe))
            {
                pipe.SetGlow(true);
            }
            else
            {
                pipe.SetGlow(false);
            }
        }
    }
}