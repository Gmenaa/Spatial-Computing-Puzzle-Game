using UnityEngine;

public class MirrorSelector : MonoBehaviour
{
    [Header("Pointer Settings")]
    public Transform rayOrigin;        // Assign this in the Inspector
    public float rayDistance = 10f;    
    public LayerMask mirrorLayer;      // Assign a dedicated Mirror layer in the Inspector

    private MirrorRotator hoveredMirror;

    void Update()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        // Draw a ray in the Scene view for debugging
        Debug.DrawRay(rayOrigin.position, rayOrigin.forward * rayDistance, Color.blue);


        RaycastHit hit;

        // Cast a ray to look for mirrors
        if (Physics.Raycast(ray, out hit, rayDistance, mirrorLayer))
        {
            // Store a reference to the MirrorRotator script
            hoveredMirror = hit.collider.GetComponent<MirrorRotator>();
        }
        else
        {
            hoveredMirror = null;
        }

        // Rotate the mirror by pressing A button
        if (hoveredMirror != null && OVRInput.GetDown(OVRInput.Button.One))
        {
            hoveredMirror.RotateMirror();
        }
    }
}

// ! apply to tracking space > hand anchors
