using UnityEngine;

public class wind : MonoBehaviour
{
    [SerializeField] private float windForce = 15f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Get wind direction based on rotation
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle));

        // Apply continuous force
        rb.AddForce(direction.normalized * windForce, ForceMode2D.Force);
    }
}