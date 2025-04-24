using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;

public class PipeSelector : MonoBehaviour
{
    [SerializeField] private Transform pointerTransform;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private LayerMask pipeMask;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxAudioSource;   
    [SerializeField] private AudioClip pipeTurnClip;      

    private MonoBehaviour hoveredRotator;

    void Start()
    {


        // auto-grab an AudioSource on this GameObject
        if (sfxAudioSource == null)
            sfxAudioSource = GetComponent<AudioSource>();
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
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                if (sfxAudioSource != null && pipeTurnClip != null) // play sound effect
                    sfxAudioSource.PlayOneShot(pipeTurnClip);
                
                hoveredRotator.Invoke("RotateRight", 0f);
            }

            if (OVRInput.GetDown(OVRInput.RawButton.X))
            {
                if (sfxAudioSource != null && pipeTurnClip != null)
                    sfxAudioSource.PlayOneShot(pipeTurnClip);
                
                hoveredRotator.Invoke("RotateLeft", 0f);
            }
        }
    }
}
