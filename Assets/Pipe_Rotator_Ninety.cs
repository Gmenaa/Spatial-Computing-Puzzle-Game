using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PipeRotatorNinety : MonoBehaviour
{
    public float rotationAngle = 90f;
    public float rotationDuration = 0.3f;
    private PipeConnection pipeConnection;
    private bool isRotating = false;

    void Start()
    {
        pipeConnection = GetComponent<PipeConnection>();

    }

    void Update()
    {
        if (isRotating) return;
    }

    public void RotateRight()
    {
        if (!isRotating)
            StartCoroutine(RotatePipeSmooth(Vector3.forward * rotationAngle));
    }

    public void RotateLeft()
    {
        if (!isRotating)
            StartCoroutine(RotatePipeSmooth(Vector3.up * rotationAngle));
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

        CompleteConnectionChecker checker = FindObjectOfType<CompleteConnectionChecker>();
        if (checker != null)
        {
            PipeConnection[] allPipes = FindObjectsOfType<PipeConnection>();
            checker.HighlightConnectedPipes(pipeConnection, allPipes);
            checker.CheckFullConnection();
        }
    }
}
