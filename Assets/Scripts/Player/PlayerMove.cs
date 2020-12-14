using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	[HideInInspector] public bool isRunning = false;

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
	private Vector3 normalCenter;
	private PlayerAnim PA;
	private bool canRun = true;

	[HideInInspector] public Vector3 allMovement;

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		normalHeight = characterController.height;
		normalCenter = characterController.center;
	}

	private void Start()
	{
		PA = PlayerAnim.PA;
	}

	private void OnEnable()
	{
		WeaponVer2.ReloadStartEvent += CanNotRun;
		WeaponVer2.ReloadEndEvent += CanRun;
		PlayerWeaponVer2.SwitchStartEvent += CanNotRun;
		WeaponVer2.WeaponReadyEvent += CanRun;
	}

	private void OnDisable()
	{
		WeaponVer2.ReloadStartEvent -= CanNotRun;
		WeaponVer2.ReloadEndEvent -= CanRun;
		PlayerWeaponVer2.SwitchStartEvent -= CanNotRun;
		WeaponVer2.WeaponReadyEvent -= CanRun;
	}

	private void Update()
	{
		if (Time.timeScale != 1)
			return;

		PlayerMovement();
		//foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
		//{
		//	if (Input.GetKey(vKey))
		//	{
		//		Debug.Log(vKey.ToString());
		//	}
		//}
	}

	private void PlayerMovement()
	{
		float verticalInput = Input.GetAxis(PlayerInput.VerticalAxis);
		float horizontalInput = Input.GetAxis(PlayerInput.HorizontalAxis);

		

		Vector3 forwardMovement = transform.forward * verticalInput;
		Vector3 rightMovement = transform.right * horizontalInput;
		allMovement = Vector3.ClampMagnitude(forwardMovement + rightMovement, 1f) * movementSpeed;

		characterController.Move(allMovement * Time.deltaTime);

		if ((verticalInput != 0 || horizontalInput != 0) && OnSlope())
			characterController.Move(Vector3.down * characterController.height / 2 * slopeForce * Time.deltaTime);

		SetMovementSpeed();
		JumpInput();
		//DuckInput();

		//Vector2 AnimDirection = new Vector2(verticalInput * movementSpeed / runSpeed, horizontalInput * movementSpeed / runSpeed);	

		if (!IsGrounded())
			StartCoroutine(OnAir());

		PA.SetAnimationPose(horizontalInput * movementSpeed / walkSpeed, verticalInput * movementSpeed / walkSpeed);
	}

	private void SetMovementSpeed()
	{
		if (isJumping && !IsGrounded())
			return;

		if (Input.GetKey(PlayerInput.Duck))
		{
			movementSpeed = Mathf.Lerp(movementSpeed, duckSpeed, Time.deltaTime * runBuildUpSpeed);
			isRunning = false;
			PlayerAnim.PA.RunAnimation(false);
		}
		else if ((Input.GetKey(PlayerInput.Run) || Input.GetKey(KeyCode.Joystick1Button2)) && Vector3.Dot(allMovement.normalized, transform.forward) > 0.7f && canRun)
		{
			movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
			isRunning = true;
			PlayerAnim.PA.RunAnimation(true);
		}
		else
		{
			movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
			isRunning = false;
			PlayerAnim.PA.RunAnimation(false);
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
			timeInAir += Time.deltaTime / 2 * Time.timeScale;
			float fallForce = Mathf.Sqrt(timeInAir);
			characterController.Move(Vector3.down * fallForce * Time.deltaTime * Time.timeScale);
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
		if((Input.GetKey(PlayerInput.Jump) || Input.GetKey(KeyCode.Joystick1Button1)) && !isJumping && IsGrounded())
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
			characterController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime * Time.timeScale);
			timeInAir += Time.deltaTime * Time.timeScale;
			yield return null;
		}
		while (!IsGrounded());

		isJumping = false;
	}

	private void DuckInput()
	{
		if (Input.GetKey(PlayerInput.Duck))
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
			characterController.center = new Vector3(normalCenter.x, Mathf.Lerp(characterController.center.y, normalCenter.y + duckHeight / 2, Time.deltaTime * duckUpAndDownSpeed * 1.1f), normalCenter.z);
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
			characterController.center = new Vector3(normalCenter.x, Mathf.Lerp(characterController.center.y, normalCenter.y, Time.deltaTime * duckUpAndDownSpeed * 1f), normalCenter.z);
		}
	}

	private void CanNotRun()
	{
		canRun = false;
	}

	private void CanRun()
	{
		canRun = true;
	}
}
