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

        if (connectedPath != null)
        {
            foreach (PipeConnection pipe in connectedPath)
            {
                pipe.SetComplete();
            }

            // ! I THINK THIS IS WHERE TO PUT THE WIN CONDITION
            // ! IF NOT, CHAMGE TO CORRECT PLACE
            // ! THIS SCEEN DOES NOT NEED gameloop.TriggerGameOver();
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
    }

    private List<PipeConnection> GetConnectedPath(PipeConnection start, PipeConnection end)
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

}