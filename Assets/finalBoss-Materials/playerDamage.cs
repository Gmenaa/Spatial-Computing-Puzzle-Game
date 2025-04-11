using UnityEngine;

public class playerDamage : MonoBehaviour
{
    public Transform playerCamera;

    public float damageDistance = .01f;
    public float damageCooldown = 1f;
    private float lastDamageTime = 0f;

    private PlayerHealth playerHealth;
    public attackBots attackBot;

    private bool hasAttacked = false;

    public AudioSource source;
    public AudioClip OwwieAudioClip;
    


    void Start()
    {

        // assign attackBot
        if (attackBot == null)
        {
            attackBot = GetComponent<attackBots>();
        }

        // find main camera
        if (playerCamera == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                playerCamera = mainCam.transform;
            }
            else
            {
                Debug.LogError("Main camera not found");
            }
        }

        if (playerCamera != null)
        {
            playerHealth = playerCamera.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                playerHealth = playerCamera.GetComponentInParent<PlayerHealth>();
            }
        }

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth script not found");
        }
    }

    void Update()
    {
        if (playerCamera == null || playerHealth == null)
            return;


        float distance = Vector3.Distance(transform.position, playerCamera.position);

        // if bot is within damage zone of player, hit!
        if (distance <= damageDistance && Time.time - lastDamageTime >= damageCooldown)
        {
            Debug.LogError("hit! owwie!");
            playerHealth.TakeDamage1();
            AudioSource.PlayClipAtPoint(OwwieAudioClip, transform.position);
            lastDamageTime = Time.time;
            
            if (attackBot != null)
            {
                attackBot.Kill();
            }

            hasAttacked = true; // prevent further damage from dead bot

        }
    }
}
