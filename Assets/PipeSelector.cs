using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine.InputSystem;

public class PipeSelector : MonoBehaviour
{
    [SerializeField] private Transform pointerTransform;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private LayerMask pipeMask;

    [SerializeField] private InputActionProperty rotateLeftAction;
    [SerializeField] private InputActionProperty rotateRightAction;

    private MonoBehaviour hoveredRotator;

    void Start()
    {
        rotateLeftAction.action.Enable();
        rotateRightAction.action.Enable();
    }

    void Update()
    {
        Vector3 origin = pointerTransform.position;
        Vector3 direction = pointerTransform.forward;

        Debug.DrawRay(origin, direction * rayDistance, Color.red);

        hoveredRotator = null;

        if (Physics.Raycast(origin, direction, out var hit, rayDistance, pipeMask))
        {
            hoveredRotator = hit.collider.GetComponent<PipeRotatorStraight>()
                            ?? (MonoBehaviour)hit.collider.GetComponent<PipeRotatorNinety>()
                            ?? hit.collider.GetComponent<PipeRotator>();
        }

        if (hoveredRotator != null)
        {
            if (rotateRightAction.action.WasPressedThisFrame())
                hoveredRotator.Invoke("RotateRight", 0f);

            if (rotateLeftAction.action.WasPressedThisFrame())
                hoveredRotator.Invoke("RotateLeft", 0f);
        }
    }
}
