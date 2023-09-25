using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	#region PublicVariables
	public static Player instance;
	#endregion

	#region PrivateVariables
	private PlayerInput input;
	private PlayerMove move;
	private PlayerAim aim;
	private PlayerDash dash;

	private bool canAct = true;
	private bool isInvincible = false;
	#endregion

	#region PublicMethod
	public void SetActive(bool b)
	{
		canAct = b;
	}
	public void SetInvincibility(bool b)
	{
		isInvincible = b;
	}
	public void Hit()
	{
		if (isInvincible == true)
			return;
	}
	#endregion

	#region PrivateMethod
	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		input = new PlayerInput();
		TryGetComponent(out aim);
		aim.Initialize();
		TryGetComponent(out move);
		move.Initialize();
		TryGetComponent(out dash);
		dash.Initialize();
	}
	private void OnEnable()
	{
		input.Enable();
		input.Player.Move.performed += OnMovePerformed;
		input.Player.Move.canceled += OnMoveCanceled;
		input.Player.Attack.performed += OnAttackPerformed;
		input.Player.Attack.canceled += OnAttackCanceled;
		input.Player.Dash.performed += OnDashPerformed;
	}
	private void OnDisable()
	{
		input.Player.Move.performed -= OnMovePerformed;
		input.Player.Move.canceled -= OnMoveCanceled;
		input.Player.Attack.performed -= OnAttackPerformed;
		input.Player.Attack.canceled -= OnAttackCanceled;
		input.Player.Dash.performed -= OnDashPerformed;
		input.Disable();
	}
	private void Update()
	{
		if (canAct == true)
		{
			aim.HandleInput();
			move.HandleInput();
		}
	}
	private void OnMovePerformed(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		move.Move(_context.ReadValue<Vector2>());
	}
	private void OnMoveCanceled(InputAction.CallbackContext _context)
	{
		move.Stop();
	}
	private void OnAttackPerformed(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		aim.OpenFire();
	}
	private void OnAttackCanceled(InputAction.CallbackContext _context)
	{
		aim.HoldFire();
	}
	private void OnDashPerformed(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		dash.Dash();
	}
	#endregion
}
