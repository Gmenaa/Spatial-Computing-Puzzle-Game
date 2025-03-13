using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform) //Check if THIS pipe was clicked
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        StartCoroutine(RotatePipeSmooth(Vector3.forward * rotationAngle));
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        StartCoroutine(RotatePipeSmooth(Vector3.up * rotationAngle));
                    }
                }
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

        // Re-check connections after rotating
        PipeConnection[] allPipes = FindObjectsOfType<PipeConnection>();
        foreach (PipeConnection pipe in allPipes)
        {
            pipe.CheckAndUpdateColor(allPipes);
        }

        FindObjectOfType<CompleteConnectionChecker>()?.CheckFullConnection();
    }
}