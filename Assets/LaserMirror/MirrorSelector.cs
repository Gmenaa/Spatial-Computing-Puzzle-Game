using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;

public class MirrorSelector : MonoBehaviour
{
    [SerializeField] private Transform _pointerTransform;
    [SerializeField] private float _rayDistance = 10f;
    [SerializeField] private LayerMask _mirrorMask;

    private MirrorRotator _hoveredMirror;

    void Update()
    {
        // Origin & direction from child transform
        Vector3 origin  = _pointerTransform.position;
        Vector3 forward = _pointerTransform.forward;

        Debug.DrawRay(origin, forward * _rayDistance, Color.cyan);

        // Raycast against Mirror layer
        if (Physics.Raycast(origin, forward, out var hit, _rayDistance, _mirrorMask))
            _hoveredMirror = hit.collider.GetComponent<MirrorRotator>();
        else
            _hoveredMirror = null;

        // On A, rotate the mirror
        if (_hoveredMirror != null && OVRInput.GetDown(OVRInput.Button.One))
            _hoveredMirror.RotateMirror();
    }
}
