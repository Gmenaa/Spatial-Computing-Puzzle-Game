using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Oculus.VR; //

public class PipeRotatorNinety : MonoBehaviour
{
    public float rotationAngle = 90f;
    public float rotationDuration = 0.3f;
    private PipeConnection pipeConnection;
    private bool isRotating = false;

    public InputActionProperty rotateRightAction;
    public InputActionProperty rotateLeftAction;

    public Transform leftHandAnchor;
    public Transform rightHandAnchor;

    void Start()
    {
        pipeConnection = GetComponent<PipeConnection>();

        rotateRightAction.action.Enable();
        rotateLeftAction.action.Enable();
    }

    void Update()
    {
        if (isRotating) return;

        if (rotateRightAction.action.WasPressedThisFrame())
        {
            Vector3 rightControllerPosition = rightHandAnchor.position;
            float distanceToRightHand = Vector3.Distance(transform.position, rightControllerPosition);
            float interactionRange = 1.0f;
            if (distanceToRightHand < interactionRange)
            {
                StartCoroutine(RotatePipeSmooth(Vector3.forward * rotationAngle));
            }
        }

        if (rotateLeftAction.action.WasPressedThisFrame())
        {
            Vector3 leftControllerPosition = leftHandAnchor.position;
            float distanceToLeftHand = Vector3.Distance(transform.position, leftControllerPosition);
            float interactionRange = 1.0f;
            if (distanceToLeftHand < interactionRange)
            {
                StartCoroutine(RotatePipeSmooth(Vector3.up * rotationAngle));
            }
            
        }
    }

    IEnumerator RotatePipeSmooth(Vector3 rotationAxis)
    {
        isRotating = true;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles + rotationAxis);
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        isRotating = false;

        PipeConnection[] allPipes = FindObjectsOfType<PipeConnection>();
        foreach (PipeConnection pipe in allPipes)
        {
            pipe.CheckAndUpdateColor(allPipes);
        }

        FindObjectOfType<CompleteConnectionChecker>()?.CheckFullConnection();
    }
}
