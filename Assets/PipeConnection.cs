using UnityEngine;

public class PipeConnection : MonoBehaviour
{
    public Transform[] connectionPoints;  //Empty child Connection objects marking ends of pipes
    private Renderer pipeRenderer;        //Controls pipe appearance
    public Material glowMaterial;
    public Material defaultMaterial;
    public Material completeMaterial;

    void Start()
    {

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
            pipeRenderer.material = defaultMaterial;
        }
    }

    public bool IsConnectedTo(PipeConnection otherPipe)
    {
        foreach (Transform point in connectionPoints)
        {
            foreach (Transform otherPoint in otherPipe.connectionPoints)
            {
                float distance = Vector3.Distance(point.position, otherPoint.position);

                if (distance < 0.3f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void SetGlow(bool isConnected)
    {
        if (pipeRenderer != null)
        {
            pipeRenderer.material = isConnected ? glowMaterial : defaultMaterial;
        }
    }

    public void SetComplete()
    {
        if (pipeRenderer != null)
        {
            pipeRenderer.material = completeMaterial;
        }
    }

}
