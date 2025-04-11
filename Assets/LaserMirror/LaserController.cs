using UnityEngine;

public class LaserController : MonoBehaviour
{
    public Transform laserEmitter;       // Reference to the emitter
    public LineRenderer lineRenderer;      // LineRenderer for visualizing the laser
    public float maxDistance = 100f;       // Maximum ray distance

    public LayerMask mirrorLayerMask;

    void Update()
    {
        CastLaser(laserEmitter.position, laserEmitter.forward);
    }

    void CastLaser(Vector3 origin, Vector3 direction)
    {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, origin);

        Vector3 currentOrigin = origin;
        Vector3 currentDirection = direction;

        while (true)
        {
            RaycastHit hit;
            // QueryTriggerInteraction to ignore trigger colliders
            if (Physics.Raycast(currentOrigin, currentDirection, out hit, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                if (hit.collider.CompareTag("Mirror"))
                {
                    currentDirection = Vector3.Reflect(currentDirection, hit.normal);
                    currentOrigin = hit.point + currentDirection * 0.05f;
                }
                else if (hit.collider.CompareTag("LaserTarget"))
                {
                    Renderer targetRenderer = hit.collider.GetComponent<Renderer>();
                    if (targetRenderer != null)
                    {
                        targetRenderer.material.color = Color.green;
                    }
                    else
                    {
                        Debug.LogWarning("The LaserTarget object does not have a Renderer component!");
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
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentOrigin + currentDirection * maxDistance);
                break;
            }
        }
    }

}
