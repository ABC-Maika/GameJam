using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	private PlayerControls controls;
	private Vector2 moveInput;
	private Rigidbody2D rb;
	private PlayerStats stats = new PlayerStats();

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
		rb.linearVelocity = new Vector2(moveInput.x * stats.speed, rb.linearVelocity.y);
	}

	private void Jump()
	{
		rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpSpeed);
	}
}