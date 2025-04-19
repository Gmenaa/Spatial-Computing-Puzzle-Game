using UnityEngine;

public class PipeConnection : MonoBehaviour
{
    public Transform[] connectionPoints;  //Empty child Connection objects marking ends of pipes
    private Renderer pipeRenderer;        //Controls pipe appearance
    public Color connectedColor = Color.green;
    public Color defaultColor = Color.white;

    void Start()
    {
        AlignConnectionPoints();

        pipeRenderer = GetComponent<Renderer>();
        if (pipeRenderer == null)
        {
            pipeRenderer = GetComponentInChildren<Renderer>();
        }

        if (pipeRenderer == null)
        {
            Debug.LogError("No Renderer found on " + gameObject.name);
        }
        else
        {
            pipeRenderer.material.color = defaultColor;
        }

        void AlignConnectionPoints() //making sure Connection objects are properly aligned
        {
            if (connectionPoints.Length < 2) return;

            //float pipeHeight = GetComponent<MeshRenderer>().bounds.size.y / 2;
            //connectionPoints[0].localPosition = new Vector3(0, pipeHeight, 0);
            //connectionPoints[1].localPosition = new Vector3(0, -pipeHeight, 0);
        }
    }

    public bool IsConnectedTo(PipeConnection otherPipe)
    {
        foreach (Transform point in connectionPoints)
        {
            foreach (Transform otherPoint in otherPipe.connectionPoints)
            {
                float distance = Vector3.Distance(point.position, otherPoint.position);
                Debug.Log($"Checking {point.name} (pipe {gameObject.name}) with {otherPoint.name} (pipe {otherPipe.gameObject.name}) - Distance: {distance}");

                if (distance < 0.3f)
                {
                    Debug.Log($"Connection found between {gameObject.name} and {otherPipe.gameObject.name} at {point.name} & {otherPoint.name}");
                    return true;
                }
            }
        }
        return false;
    }

    public void CheckAndUpdateColor(PipeConnection[] allPipes)
    {
        bool isConnected = false;

        foreach (PipeConnection otherPipe in allPipes)
        {
            if (otherPipe != this && IsConnectedTo(otherPipe))
            {
                isConnected = true;
                break; // Stop checking if we find a valid connection
            }
        }

        ChangeColor(isConnected ? connectedColor : defaultColor);
    }

    public void ChangeColor(Color newColor)
    {
        if (pipeRenderer != null)
        {
            pipeRenderer.material.color = newColor;
        }
    }


}
