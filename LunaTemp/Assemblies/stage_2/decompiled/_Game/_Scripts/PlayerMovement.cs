using UnityEngine;

namespace _Game._Scripts
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField]
		private FloatingJoystick _floatingJoystick;

		[SerializeField]
		private float _moveSpeed;

		[SerializeField]
		private Rigidbody2D _rigidbody2D;

		[SerializeField]
		private Transform _visualsTransform;

		private Vector2 m_MovementInput;

		private void Awake()
		{
			InitializeRigidbody();
		}

		private void Update()
		{
			ReadInput();
		}

		private void FixedUpdate()
		{
			Move();
			HandleFacingDirection();
		}

		private void InitializeRigidbody()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
		}

		private void ReadInput()
		{
			if (_floatingJoystick == null)
			{
				m_MovementInput = Vector2.zero;
			}
			else
			{
				m_MovementInput = new Vector2(_floatingJoystick.Horizontal, _floatingJoystick.Vertical);
			}
		}

		private void Move()
		{
			Vector2 targetVelocity = m_MovementInput * _moveSpeed;
			_rigidbody2D.velocity = targetVelocity;
		}

		private void HandleFacingDirection()
		{
			if (!(Mathf.Abs(m_MovementInput.x) < 0.01f))
			{
				Vector3 localScale = _visualsTransform.localScale;
				localScale.x = ((m_MovementInput.x > 0f) ? Mathf.Abs(localScale.x) : (0f - Mathf.Abs(localScale.x)));
				_visualsTransform.localScale = localScale;
			}
		}

		private void OnValidate()
		{
			if (!_rigidbody2D)
			{
				_rigidbody2D = GetComponent<Rigidbody2D>();
			}
		}
	}
}
