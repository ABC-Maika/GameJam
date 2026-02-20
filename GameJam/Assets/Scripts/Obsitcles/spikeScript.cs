using UnityEngine;

public class spikeScript : MonoBehaviour
{
    private PlayerStats stats = new PlayerStats();
    public float knockbackForce = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ApplyKnockback(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Calculate direction: Away from the spike, slightly upward
            Vector2 direction = (player.transform.position - transform.position).normalized;
            direction += Vector2.up * 0.5f;

            rb.linearVelocity = Vector2.zero; // Reset speed for a clean "pop"
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            stats.takeDamage(20);
            ApplyKnockback(collision.gameObject);
        }
    }
}
