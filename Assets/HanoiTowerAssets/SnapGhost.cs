using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SnapGhost : MonoBehaviour
{
    [SerializeField] private Material _ghostMaterial;
    [SerializeField] private SnapInteractable _snapInteractable;
    private GameObject _ghostObject;
    private SnapInteractor _currentInteractor;

    private void HandleInteractorAdded(SnapInteractor interactor)
    {
        if (_currentInteractor != interactor)
        {
            if (_ghostObject != null)
            {
                Destroy(_ghostObject);
                _ghostObject = null;
            }

            _currentInteractor = interactor;
            SetupGhostModel(interactor);
        }
    }
    
    
    private void OnEnable()
    {
        if (_snapInteractable != null)
        {
            _snapInteractable.WhenInteractorViewAdded += HandleInteractorViewAdded;
            _snapInteractable.WhenInteractorViewRemoved += HandleInteractorViewRemoved;
            _snapInteractable.WhenInteractorAdded.Action += HandleInteractorAdded;
            _snapInteractable.WhenSelectingInteractorViewAdded += HandleSelectingInteractorViewAdded;
        }
    }

    private void OnDisable()
    {
        if (_snapInteractable != null)
        {
            _snapInteractable.WhenInteractorAdded.Action -= HandleInteractorAdded;
            _snapInteractable.WhenSelectingInteractorViewAdded -= HandleSelectingInteractorViewAdded;
            _snapInteractable.WhenInteractorViewRemoved -= HandleInteractorViewRemoved;
            _snapInteractable.WhenInteractorViewAdded -= HandleInteractorViewAdded;
        }
    }

    private void HandleSelectingInteractorViewAdded(IInteractorView interactorView)
    {
        _ghostObject?.SetActive(false);
    }

    private void HandleInteractorViewAdded(IInteractorView interactorView)
    {
        _ghostObject?.SetActive(true);
    }

    private void HandleInteractorViewRemoved(IInteractorView interactorView)
    {
        _ghostObject?.SetActive(false);
    }
    
    private void SetupGhostModel(SnapInteractor interactor)
    {
        if (interactor == null || interactor.transform.parent == null)
        {
            Debug.LogWarning("Interactor or its parent is null. Cannot setup ghost model.");
            return;
        }

        Transform parentTransform = interactor.transform.parent;
        string parentName = string.IsNullOrEmpty(parentTransform.name) ? "GhostModel" : parentTransform.name;
        
        _ghostObject = new GameObject(parentName);
        _ghostObject.transform.SetParent(transform, false);
        _ghostObject.transform.localScale = parentTransform.localScale;
        _ghostObject.transform.localPosition = Vector3.zero;
        _ghostObject.transform.localRotation = Quaternion.identity;

        MeshFilter parentMeshFilter = parentTransform.GetComponent<MeshFilter>();
        if (parentMeshFilter != null)
        {
            MeshFilter ghostMeshFilter = _ghostObject.AddComponent<MeshFilter>();
            ghostMeshFilter.mesh = parentMeshFilter.mesh;
            MeshRenderer ghostRenderer = _ghostObject.AddComponent<MeshRenderer>();
            ghostRenderer.material = _ghostMaterial;
        }
        
        MeshFilter[] meshFilters = parentTransform.GetComponentsInChildren<MeshFilter>(includeInactive: true);
        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (meshFilter.gameObject == parentTransform.gameObject)
            {
                continue;
            }

            GameObject childGhost = new GameObject(meshFilter.gameObject.name);
            childGhost.transform.SetParent(_ghostObject.transform, false);
            childGhost.transform.localRotation = meshFilter.transform.localRotation;
            childGhost.transform.localScale = meshFilter.transform.localScale;
            childGhost.transform.localPosition = meshFilter.transform.localPosition;
            MeshFilter childMeshFilter = childGhost.AddComponent<MeshFilter>();
            childMeshFilter.mesh = meshFilter.mesh;
            MeshRenderer childRenderer = childGhost.AddComponent<MeshRenderer>();
            childRenderer.material = _ghostMaterial;
        }
    }
}