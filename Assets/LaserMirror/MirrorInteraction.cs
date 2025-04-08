using UnityEngine;

public class MirrorInteraction : MonoBehaviour
{
    [Tooltip("Angle (in degrees) the mirror rotates per button press.")]
    public float rotationAngle = 22.5f;
    
    [Tooltip("Sound played when the player enters the mirror's collider.")]
    public AudioClip entrySound;

    // Reference to the AudioSource component for playing sounds.
    private AudioSource audioSource;

    // This variable tracks whether the user is close enough to rotate the mirror.
    private bool isPlayerInRange = false;

    void Start()
    {
        // Attempt to get the AudioSource on the same GameObject.
        audioSource = GetComponent<AudioSource>();

        // Optionally add an AudioSource if one isn't already attached.
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player or the player's controller.
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            // Play the entry sound when the player enters the collider.
            if (entrySound != null && audioSource != null)
            {
                audioSource.PlayOneShot(entrySound);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    void Update()
    {
        // If the player is in range and presses the A button on the right controller, rotate the mirror.
        if (isPlayerInRange && OVRInput.GetDown(OVRInput.Button.One))
        {
            RotateMirror();
        }
    }

    private void RotateMirror()
    {
        // Rotate the mirror by 'rotationAngle' degrees around its local Y axis.
        transform.Rotate(Vector3.up, rotationAngle, Space.Self);

        // Optional: Add additional feedback, such as sound or haptics, here.
    }
}
