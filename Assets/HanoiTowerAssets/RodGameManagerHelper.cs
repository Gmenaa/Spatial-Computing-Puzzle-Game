using Oculus.Interaction;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RodGameManagerHelper : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject rod;
    private SnapInteractor _currentInteractor;
    private List<int> diskSizes = new List<int>();
    
    private void HandleInteractorAdded(SnapInteractor interactor)
    {
        diskSizes.Clear();
        
        for (int i = 0; i < rod.transform.childCount; i++)
        {
            Transform childTransform = rod.transform.GetChild(i);
            SnapInteractable snapPos = childTransform.GetComponent<SnapInteractable>();

            if (snapPos != null && snapPos.Interactors.Any())
            {
                SnapInteractor diskInteractor = snapPos.Interactors.FirstOrDefault();
                if (diskInteractor != null)
                {
                    GameObject disk = diskInteractor.transform.parent.gameObject;
                    HanoiDisk hanoiDisk = disk.GetComponent<HanoiDisk>();
                    
                    diskSizes.Add(hanoiDisk.diskSize);
                }
            }
        }
        if (diskSizes.Count == 4 && IsOrderedDescending(diskSizes))
        {
            _gameManager.HandleWin();
        }
    }

    private bool IsOrderedDescending(List<int> sizes)
    {
        if (sizes.Count == 0) return false;

        for (int i = 1; i < sizes.Count; i++)
        {
            if (sizes[i] >= sizes[i - 1])
            {
                return false;
            }
        }

        return true;
    }
    
    private void OnEnable()
    {
        foreach (Transform child in rod.transform)
        {
            SnapInteractable snapPos = child.GetComponent<SnapInteractable>();
            if (snapPos != null)
            {
                snapPos.WhenInteractorAdded.Action += HandleInteractorAdded;
            }
        }
    }

    private void OnDisable()
    {
        if (rod == null) return;
        
        foreach (Transform child in rod.transform)
        {
            SnapInteractable snapPos = child.GetComponent<SnapInteractable>();
            if (snapPos != null)
            {
                snapPos.WhenInteractorAdded.Action -= HandleInteractorAdded;
            }
        }
    }
}
