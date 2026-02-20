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
	private float _smoothedLookAhead;

	void FixedUpdate()
	{
		if (target == null) return;

		float targetLookAhead = (target.localScale.x > 0 ? 1 : -1) * lookAheadDistance;
		_smoothedLookAhead = Mathf.MoveTowards(_smoothedLookAhead, targetLookAhead, Time.fixedDeltaTime * 10f);

		Vector3 targetPosition = target.position + offset + (Vector3.right * _smoothedLookAhead);

		transform.position = Vector3.SmoothDamp(
			transform.position,
			targetPosition,
			ref _currentVelocity,
			smoothTime
		);
	}
}
