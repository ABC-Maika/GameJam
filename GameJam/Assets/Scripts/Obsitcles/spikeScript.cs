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
		PlayerMovement movement = player.GetComponent<PlayerMovement>();
		if (movement != null)
		{
			float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
			Vector2 direction = new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle));

			movement.ApplyKnockback(direction.normalized * knockbackForce);
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
