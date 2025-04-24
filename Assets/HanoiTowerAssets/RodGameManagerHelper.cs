using Oculus.Interaction;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RodGameManagerHelper : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject rod;
    [SerializeField] private bool isStartedRod;
    [SerializeField] private int numberOfDisks = 4;
    
    [Header("Disk-Move SFX")]
    [SerializeField] private AudioSource sfxAudioSource;    // assign via Inspector
    [SerializeField] private AudioClip diskMoveClip;        // your “move” sound
    [SerializeField] private float moveCooldown = 0.1f;     // debounce so it won’t spam
    private float lastMoveTime;

    
    [SerializeField] private List<GameObject> initialDisks;
    
    private List<int> diskSizes = new List<int>();

    private void Start()
    {
        if (initialDisks != null && initialDisks.Count > 0)
        {
            int positionsCount = rod.transform.childCount;
            if (initialDisks.Count > positionsCount)
            {
                Debug.LogError("number of intial disks should be equal to the positions");
                return;
            }
            for (int i = 0; i < initialDisks.Count; i++)
            {
                int posIndex = positionsCount - 1 - i;
                Transform snapTransform = rod.transform.GetChild(posIndex);
                SnapInteractable snapInteractable = snapTransform.GetComponent<SnapInteractable>();
                SnapInteractor diskInteractor = initialDisks[i].GetComponentInChildren<SnapInteractor>();
                if (snapInteractable != null && diskInteractor != null)
                {
                    snapInteractable.AddInteractor(diskInteractor);
                }
            }
            UpdateDiskInteractability();
            UpdateSnapInteractableAvailability();
        }
    }

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
        
        if (diskSizes.Count >= 2 && !IsOrderedDescending(diskSizes))
        {
            _gameManager.HandleLose();
            return;
        }
        
        if (diskSizes.Count == numberOfDisks && IsOrderedDescending(diskSizes) && !isStartedRod)
        {
            _gameManager.HandleWin();
        }
        UpdateDiskInteractability();
        UpdateSnapInteractableAvailability();
    }
    
    private void HandleInteractorRemoved(SnapInteractor interactor)
    {
        // play a disk move sound, but only once per cooldown 
        if (Time.time - lastMoveTime > moveCooldown 
        && sfxAudioSource != null 
        && diskMoveClip != null)
    {
        sfxAudioSource.PlayOneShot(diskMoveClip);
        lastMoveTime = Time.time;
    }
        UpdateDiskInteractability();
        UpdateSnapInteractableAvailability();
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
    
    private void UpdateDiskInteractability()
    {
        int topIndex = -1;
        for (int i = 0; i < rod.transform.childCount; i++)
        {
            Transform child = rod.transform.GetChild(i);
            SnapInteractable snapPos = child.GetComponent<SnapInteractable>();
            if (snapPos != null && snapPos.Interactors.Any())
            {
                topIndex = i;
            }
        }
        
        for (int i = 0; i < rod.transform.childCount; i++)
        {
            Transform child = rod.transform.GetChild(i);
            SnapInteractable snapPos = child.GetComponent<SnapInteractable>();
            if (snapPos != null && snapPos.Interactors.Any())
            {
                SnapInteractor diskInteractor = snapPos.Interactors.FirstOrDefault();
                if (diskInteractor != null)
                {
                    GameObject disk = diskInteractor.transform.parent.gameObject;
                    bool isTop = (i == topIndex);
                    SetDiskInteractable(disk, isTop);
                }
            }
        }
    }
    
    private void SetDiskInteractable(GameObject disk, bool interactable)
    {
        Collider col = disk.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = interactable;
        }
    }
    
    private void UpdateSnapInteractableAvailability()
    {
        int lowestFree = GetLowestFreeSnapIndex();
        for (int i = 0; i < rod.transform.childCount; i++)
        {
            Transform child = rod.transform.GetChild(i);
            SnapInteractable snapPos = child.GetComponent<SnapInteractable>();
            if (snapPos != null)
            {
                if (!snapPos.Interactors.Any())
                {
                    snapPos.enabled = (i == lowestFree);
                }
                else
                {
                    snapPos.enabled = true;
                }
            }
        }
    }
    private int GetLowestFreeSnapIndex()
    {
        int count = rod.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = rod.transform.GetChild(i);
            SnapInteractable snapPos = child.GetComponent<SnapInteractable>();
            if (snapPos != null && !snapPos.Interactors.Any())
            {
                return i;
            }
        }
        return -1;
    }
    
    private void OnEnable()
    {
        foreach (Transform child in rod.transform)
        {
            SnapInteractable snapPos = child.GetComponent<SnapInteractable>();
            if (snapPos != null)
            {
                snapPos.WhenInteractorAdded.Action += HandleInteractorAdded;
                snapPos.WhenInteractorRemoved.Action += HandleInteractorRemoved;
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
                snapPos.WhenInteractorRemoved.Action -= HandleInteractorRemoved;
            }
        }
    }
}
