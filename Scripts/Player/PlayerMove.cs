using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] private string horizontalInputName, verticalInputName;
	[SerializeField] private float walkSpeed, runSpeed, duckSpeed;
	[SerializeField] private float runBuildUpSpeed;
	[SerializeField] private AnimationCurve jumpFallOff;
	[SerializeField] private float jumpMultiplier;
	[SerializeField] private float duckHeight;
	[SerializeField] private float duckUpAndDownSpeed;
	[SerializeField] private float slopeForce;
	[SerializeField] private float slopeForceRayLength;

	private CharacterController characterController;
	private float movementSpeed;
	private bool isJumping;
	private const float duckForce = 50f;
	private float normalHeight;
	private PlayerInput PI;

	[HideInInspector] public Vector3 allMovement;

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		normalHeight = characterController.height;
	}

	private void Start()
	{
		PI = PlayerInput.PI;
	}

	private void Update()
	{
		PlayerMovement();
	}

	private void PlayerMovement()
	{
		float verticalInput = Input.GetAxis(PI.VerticalAxis);
		float horizontalInput = Input.GetAxis(PI.HorizontalAxis);

		Vector3 forwardMovement = transform.forward * verticalInput;
		Vector3 rightMovement = transform.right * horizontalInput;
		allMovement = Vector3.ClampMagnitude(forwardMovement + rightMovement, 1f) * movementSpeed;

		characterController.Move(allMovement * Time.deltaTime);

		if ((verticalInput != 0 || horizontalInput != 0) && OnSlope())
			characterController.Move(Vector3.down * characterController.height / 2 * slopeForce * Time.deltaTime);

		SetMovementSpeed();
		JumpInput();
		DuckInput();

		if (!IsGrounded())
			StartCoroutine(OnAir());
	}

	private void SetMovementSpeed()
	{
		if (isJumping && !IsGrounded())
			return;

		if (Input.GetKey(PI.Duck))
		{
			movementSpeed = Mathf.Lerp(movementSpeed, duckSpeed, Time.deltaTime * runBuildUpSpeed);
		}
		else if (Input.GetKey(PI.Run))
		{
			movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
		}
		else
		{
			movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
		}
	}

	public bool IsGrounded()
	{
		if (Physics.Raycast(transform.position, Vector3.down, characterController.height / 2 + 0.1f - characterController.center.y))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private IEnumerator OnAir()
	{
		float timeInAir = 0f;

		while (!characterController.isGrounded)
		{
			timeInAir += Time.deltaTime / 2;
			float fallForce = Mathf.Sqrt(timeInAir);
			characterController.Move(Vector3.down * fallForce * Time.deltaTime);
			yield return null;
		}
	}

	private bool OnSlope()
	{
		if (isJumping)
			return false;

		RaycastHit hit;

		if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height / 2 * slopeForceRayLength))
			if (hit.normal != Vector3.up)
				return true;
		return false;
	}

	private void JumpInput()
	{
		if(Input.GetKey(PI.Jump) && !isJumping && IsGrounded())
		{
			isJumping = true;
			StartCoroutine(Jump());
		}
	}

	private IEnumerator Jump()
	{
		characterController.slopeLimit = 90f;
		float timeInAir = 0.0f;

		do
		{
			float jumpForce = jumpFallOff.Evaluate(timeInAir);
			characterController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
			timeInAir += Time.deltaTime;
			yield return null;
		}
		while (!IsGrounded());

		isJumping = false;
	}

	private void DuckInput()
	{
		if (Input.GetKey(PI.Duck))
			DuckDown();
		else
			DuckUp();
	}

	private void DuckDown()
	{
		if (isJumping)
			return;

		if (characterController.height - duckHeight > 0.01f && IsGrounded())
		{
			characterController.height = Mathf.Lerp(characterController.height, duckHeight, Time.deltaTime * duckUpAndDownSpeed);
			characterController.center = new Vector3(0, Mathf.Lerp(characterController.center.y, 0 + duckHeight / 2, Time.deltaTime * duckUpAndDownSpeed * 1.1f), 0);
			characterController.Move(Vector3.down * duckForce * Time.deltaTime);
		}
	}

	private void DuckUp()
	{
		if (isJumping)
			return;

		if (normalHeight - characterController.height > 0.01f)
		{
			characterController.Move((Vector3.up * characterController.center.y * 10) * Time.deltaTime);
			characterController.height = Mathf.Lerp(characterController.height, normalHeight, Time.deltaTime * duckUpAndDownSpeed);
			characterController.center = new Vector3(0, Mathf.Lerp(characterController.center.y, 0, Time.deltaTime * duckUpAndDownSpeed * 1f), 0);
		}
	}
}
