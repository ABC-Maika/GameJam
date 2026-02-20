using UnityEngine;

public class CamMovement : MonoBehaviour
{
	[Header("Targeting")]
	[SerializeField] private Transform target;
	[SerializeField] private Vector3 offset = new Vector3(0, 2, -10);

	[Header("Smooth Settings")]
	[Range(0, 1)]
	[SerializeField] private float smoothTime = 0.125f;
	[SerializeField] private float lookAheadDistance = 2f;

	private Vector3 _currentVelocity;

	void LateUpdate()
	{
		if (target == null) return;

		Vector3 targetPosition = target.position + offset;

		float moveDirection = target.localScale.x > 0 ? 1 : -1;
		Vector3 lookAheadOffset = Vector3.right * (moveDirection * lookAheadDistance);

		Vector3 finalGoal = targetPosition + lookAheadOffset;

		transform.position = Vector3.SmoothDamp(
			transform.position,
			finalGoal,
			ref _currentVelocity,
			smoothTime
		);
	}
}
