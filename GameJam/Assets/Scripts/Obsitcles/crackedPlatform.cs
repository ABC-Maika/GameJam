using UnityEngine;

public class CrackedPlatform : MonoBehaviour
{
    // Settings
    public float timeToCollapse = 1.0f;
    public float timeToRespawn = 3.0f;

    // Internal Timers
    private float collapseTimer;
    private float respawnTimer;

    // State Flags
    private bool isSteppedOn = false;
    private bool isBroken = false;

    // Components
    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;

    void Start()
    {
        // Get components at the start
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();

        // Set initial timer values
        collapseTimer = timeToCollapse;
        respawnTimer = timeToRespawn;
    }

    void Update()
    {
        // Logic 1: If player stepped on it, count down to disappear
        if (isSteppedOn && !isBroken)
        {
            collapseTimer -= Time.deltaTime;
            if (collapseTimer <= 0)
            {
                BreakPlatform();
            }
        }

        // Logic 2: If it is broken, count down to respawn
        if (isBroken)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                RespawnPlatform();
            }
        }
    }

    private void BreakPlatform()
    {
        isBroken = true;
        spriteRenderer.enabled = false; // Make it invisible
        platformCollider.enabled = false; // Make it non-solid
    }

    private void RespawnPlatform()
    {
        // Reset all states and timers
        isBroken = false;
        isSteppedOn = false;
        collapseTimer = timeToCollapse;
        respawnTimer = timeToRespawn;

        spriteRenderer.enabled = true; // Make it visible again
        platformCollider.enabled = true; // Make it solid again
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Same tag check style as your Spike Script
        if (collision.gameObject.tag == "Player")
        {
            isSteppedOn = true;
        }
    }
}