using UnityEngine;

public class MirrorSelector : MonoBehaviour
{
    [Header("Pointer Settings")]
    public Transform rayOrigin;        
    public float rayDistance = 10f;    
    public LayerMask mirrorLayer;      

    private MirrorRotator hoveredMirror;

    void Update()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        Debug.DrawRay(rayOrigin.position, rayOrigin.forward * rayDistance, Color.blue); // ? debugging


        // Check if the ray hits a mirror
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance, mirrorLayer))
        {
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

// ! Apply to tracking space > hand anchors
