using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	private PlayerControls controls;
	private Vector2 moveInput;
	private Rigidbody2D rb;
	private PlayerStats stats = new PlayerStats();

	[Header("Sprite Rotation")]
	[SerializeField] private Transform spriteChild;
	[SerializeField] private float rotationSpeed = 180f;

	[Header("Detection")]
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private float groundCheckRadius = 0.3f;
	[SerializeField] private Vector2 groundCheckOffset = new Vector2(0, -0.8f);

	[Header("Auto Jump Settings")]
	[SerializeField] private float stepCheckDistance = 0.5f;
	[SerializeField] private float autoJumpForce = 8f;
	[SerializeField] private Vector2 upperRayOffset = new Vector2(0, 0.5f);

	[Header("Juice (Coyote & Buffer)")]
	[SerializeField] private float coyoteTime = 0.15f;
	private float coyoteTimeCounter;
	[SerializeField] private float jumpBufferTime = 0.15f;
	private float jumpBufferCounter;

	private bool isGrounded;
	private bool isKnockedBack = false;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		controls = new PlayerControls();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;

		controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
		controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

		controls.Player.Jump.performed += ctx => jumpBufferCounter = jumpBufferTime;

		controls.Player.Sprint.performed += ctx => stats.speed = stats.sprint;
		controls.Player.Sprint.canceled += ctx => stats.speed = stats.normalSpeed;
	}

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void OnEnable() => controls.Enable();
	private void OnDisable() => controls.Disable();

	void Update()
	{
		if (isGrounded)
		{
			coyoteTimeCounter = coyoteTime;
		}
		else
		{
			coyoteTimeCounter -= Time.deltaTime;
		}

		jumpBufferCounter -= Time.deltaTime;

		if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
		{
			ExecuteJump();
		}

		HandleSpriteRotation();
	}

	private void FixedUpdate()
	{
		isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + groundCheckOffset, groundCheckRadius, groundLayer);

		if (isKnockedBack) return;

		if (isGrounded && Mathf.Abs(moveInput.x) > 0.1f)
		{
			CheckForAutoJump();
		}

		float targetSpeed = moveInput.x * stats.speed;
		float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? 50f : 30f;
		float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accelerationRate * Time.fixedDeltaTime);

		rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
	}

	private void HandleSpriteRotation()
	{
		if (spriteChild == null) return;

		if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
		{
			float rotationAmount = rb.linearVelocity.x * rotationSpeed * Time.deltaTime;
			spriteChild.Rotate(Vector3.forward, -rotationAmount);
		}
		else if (isGrounded)
		{
			spriteChild.rotation = Quaternion.Lerp(spriteChild.rotation, Quaternion.identity, Time.deltaTime * 10f);
		}
	}

	private void ExecuteJump()
	{
		rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpSpeed);

		jumpBufferCounter = 0f;
		coyoteTimeCounter = 0f;
	}

	private void CheckForAutoJump()
	{
		Vector2 direction = moveInput.x > 0 ? Vector2.right : Vector2.left;
		Vector2 lowerOrigin = (Vector2)transform.position + groundCheckOffset + (Vector2.up * 0.1f);
		Vector2 upperOrigin = (Vector2)transform.position + upperRayOffset;

		RaycastHit2D hitLower = Physics2D.Raycast(lowerOrigin, direction, stepCheckDistance, groundLayer);
		RaycastHit2D hitUpper = Physics2D.Raycast(upperOrigin, direction, stepCheckDistance, groundLayer);

		if (hitLower.collider != null && hitUpper.collider == null)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, autoJumpForce);
		}
	}

	public void ApplyKnockback(Vector2 force)
	{
		isKnockedBack = true;
		rb.linearVelocity = Vector2.zero;
		rb.AddForce(force, ForceMode2D.Impulse);
		Invoke(nameof(ResetKnockback), 0.2f);
	}

	private void ResetKnockback() => isKnockedBack = false;

	private void OnDrawGizmos()
	{
		Gizmos.color = isGrounded ? Color.green : Color.red;
		Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckOffset, groundCheckRadius);
	}
}