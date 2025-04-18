using UnityEngine;

public class FollowPlayerCanvas : MonoBehaviour
{
    public Transform playerHead; 

    public float distanceFromPlayer = 2f;

    void Start()
    {
        if (playerHead == null)
        {
            Debug.LogError("Player head is not assigned!");
            return;
        }

        // Move the canvas in front of the player
        transform.position = playerHead.position + playerHead.forward * distanceFromPlayer;

        // Make it face the player
        transform.LookAt(playerHead);

        // Flip it 180 degrees to face the right way
        transform.Rotate(0, 180f, 0);
    }
}
