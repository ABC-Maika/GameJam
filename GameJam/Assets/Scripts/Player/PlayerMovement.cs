using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	private PlayerControls controls;
	private Vector2 moveInput;
	private Rigidbody2D rb;
	private PlayerStats stats = new PlayerStats();

	[Header("Detection")]
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private float groundCheckRadius = 0.3f;
	[SerializeField] private Vector2 groundCheckOffset = new Vector2(0, -0.5f);

	[Header("State")]
	[SerializeField] private bool isGrounded;
	private bool isKnockedBack = false;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		controls = new PlayerControls();

		// Rigidbody Setup: Ensure we start with rotation locked to prevent "tripping"
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;

		controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
		controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

		controls.Player.Jump.performed += ctx => Jump();
		controls.Player.Sprint.performed += ctx => stats.speed = stats.sprint;
		controls.Player.Sprint.canceled += ctx => stats.speed = stats.normalSpeed;
	}

	private void OnEnable() => controls.Enable();
	private void OnDisable() => controls.Disable();

	private void FixedUpdate()
	{
		// 1. Precise Ground Check
		isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + groundCheckOffset, groundCheckRadius, groundLayer);

		// 2. Handle Rotation Logic
		HandleRotationState();

		// 3. Movement Logic
		if (isKnockedBack) return;

		float targetSpeed = moveInput.x * stats.speed;
		float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? 50f : 30f;
		float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accelerationRate * Time.fixedDeltaTime);

		rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
	}

	private void HandleRotationState()
	{
		// If we are grounded or trying to move, we should stay upright
		if (isGrounded || Mathf.Abs(moveInput.x) > 0.1f)
		{
			// Smoothly snap back to upright if we were spinning
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.fixedDeltaTime * 10f);
			rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		else
		{
			// If we are in the air and NOT moving, let physics take over (Natural Falling)
			rb.constraints = RigidbodyConstraints2D.None;
		}
	}

	private void Jump()
	{
		if (!isGrounded) return;
		rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpSpeed);
	}

	public void ApplyKnockback(Vector2 force)
	{
		isKnockedBack = true;
		// Allow the player to tumble when hit
		rb.constraints = RigidbodyConstraints2D.None;

		rb.linearVelocity = Vector2.zero;
		rb.AddForce(force, ForceMode2D.Impulse);

		Invoke(nameof(ResetKnockback), 0.5f); // Increased time to allow for a "tumble"
	}

	private void ResetKnockback()
	{
		isKnockedBack = false;
		// Note: HandleRotationState will pull us back upright once we touch ground
	}

	// Visualization for debugging
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckOffset, groundCheckRadius);
	}
}