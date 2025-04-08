using UnityEngine;

public class MirrorRotator : MonoBehaviour
{
    public float rotationAngle = 22.5f;
    public OVRInput.Button rotationButton = OVRInput.Button.One;

    void Update()
    {
        if (OVRInput.GetDown(rotationButton))
        {
            RotateMirror();
        }
    }

    // Rotates the mirror by the specified angle around Y axis.
    public void RotateMirror()
    {
        transform.Rotate(Vector3.up, rotationAngle, Space.Self);
        
        // TODO: haptic feed back 
    }
}
