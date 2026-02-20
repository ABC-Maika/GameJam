using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	private PlayerControls controls;
	private Vector2 moveInput;
	private Rigidbody2D rb;
	private PlayerStats stats = new PlayerStats();
	
	[SerializeField] private LayerMask groundLayer;

	[SerializeField] private bool isGrounded = true;

	private bool isKnockedBack = false;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		controls = new PlayerControls();

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
		isGrounded = Physics2D.OverlapCircle(transform.position, 1f, groundLayer);

		if (!isGrounded) return;
		
		if (isKnockedBack)
		{
			return;
		}

		float targetSpeed = moveInput.x * stats.speed;

		float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? 50f : 30f;

		float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accelerationRate * Time.fixedDeltaTime);

		rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
	}

	private void Jump()
	{
		if (!isGrounded) return;

		rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpSpeed);
	}

	public void ApplyKnockback(Vector2 force)
	{
		isKnockedBack = true;

		rb.linearVelocity = Vector2.zero;
		rb.AddForce(force, ForceMode2D.Impulse);

		Invoke(nameof(ResetKnockback), 0.2f); // tweak time if needed
	}

	private void ResetKnockback()
	{
		isKnockedBack = false;
	}
}